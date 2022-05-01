using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core.ViewModel;
using Portal.DAL;
using Portal.Common;
using System.Reflection;
using System.Data;
using System.Transactions;
namespace Portal.Core
{
    public class PurchaseReturnBL
    {
        DBInterface dbInterface;
        public PurchaseReturnBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditPurchaseReturn(PurchaseReturnViewModel purchaseReturnViewModel, List<PurchaseReturnProductViewModel> purchaseReturnProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                PurchaseReturn purchaseReturn = new PurchaseReturn
                {
                    PurchaseReturnId = purchaseReturnViewModel.PurchaseReturnId,
                    PurchaseReturnDate = Convert.ToDateTime(purchaseReturnViewModel.PurchaseReturnDate),
                    ReturnType = purchaseReturnViewModel.ReturnType,
                    InvoiceNo = purchaseReturnViewModel.InvoiceNo,
                    InvoiceId = purchaseReturnViewModel.InvoiceId,
                    CompanyBranchId = purchaseReturnViewModel.CompanyBranchId,
                    VendorId = purchaseReturnViewModel.VendorId,
                    VendorName = purchaseReturnViewModel.VendorName,
                    BillingAddress = purchaseReturnViewModel.BillingAddress,
                    City = purchaseReturnViewModel.City,
                    StateId = purchaseReturnViewModel.StateId,
                    CountryId = purchaseReturnViewModel.CountryId,
                    PinCode = purchaseReturnViewModel.PinCode,
                    GSTNo = purchaseReturnViewModel.GSTNo,
                    RefNo = purchaseReturnViewModel.RefNo,
                    RefDate = string.IsNullOrEmpty(purchaseReturnViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(purchaseReturnViewModel.RefDate),
                    BasicValue = purchaseReturnViewModel.BasicValue,
                    TotalValue = purchaseReturnViewModel.TotalValue,
                    FreightValue = purchaseReturnViewModel.FreightValue,
                    FreightCGST_Perc = purchaseReturnViewModel.FreightCGST_Perc,
                    FreightCGST_Amt = purchaseReturnViewModel.FreightCGST_Amt,
                    FreightSGST_Perc = purchaseReturnViewModel.FreightSGST_Perc,
                    FreightSGST_Amt = purchaseReturnViewModel.FreightSGST_Amt,
                    FreightIGST_Perc = purchaseReturnViewModel.FreightIGST_Perc,
                    FreightIGST_Amt = purchaseReturnViewModel.FreightIGST_Amt,
                    LoadingValue = purchaseReturnViewModel.LoadingValue,
                    LoadingCGST_Perc = purchaseReturnViewModel.LoadingCGST_Perc,
                    LoadingCGST_Amt = purchaseReturnViewModel.LoadingCGST_Amt,
                    LoadingSGST_Perc = purchaseReturnViewModel.LoadingSGST_Perc,
                    LoadingSGST_Amt = purchaseReturnViewModel.LoadingSGST_Amt,
                    LoadingIGST_Perc = purchaseReturnViewModel.LoadingIGST_Perc,
                    LoadingIGST_Amt = purchaseReturnViewModel.LoadingIGST_Amt,
                    InsuranceValue = purchaseReturnViewModel.InsuranceValue,
                    InsuranceCGST_Perc = purchaseReturnViewModel.InsuranceCGST_Perc,
                    InsuranceCGST_Amt = purchaseReturnViewModel.InsuranceCGST_Amt,
                    InsuranceSGST_Perc = purchaseReturnViewModel.InsuranceSGST_Perc,
                    InsuranceSGST_Amt = purchaseReturnViewModel.InsuranceSGST_Amt,
                    InsuranceIGST_Perc = purchaseReturnViewModel.InsuranceIGST_Perc,
                    InsuranceIGST_Amt = purchaseReturnViewModel.InsuranceIGST_Amt,
                    Remarks = purchaseReturnViewModel.Remarks,
                    ApprovalStatus = purchaseReturnViewModel.ApprovalStatus,
                    InvoiceStatus = purchaseReturnViewModel.InvoiceStatus,
                    FinYearId = purchaseReturnViewModel.FinYearId,
                    CompanyId = purchaseReturnViewModel.CompanyId,
                    CreatedBy = purchaseReturnViewModel.CreatedBy,
                    RoundOfValue = purchaseReturnViewModel.RoundOfValue,
                    GrossValue = purchaseReturnViewModel.GrossValue,
                };
                List<PurchaseReturnProductDetail> purchaseReturnProductList = new List<PurchaseReturnProductDetail>();
                if (purchaseReturnProducts != null && purchaseReturnProducts.Count > 0)
                {
                    foreach (PurchaseReturnProductViewModel item in purchaseReturnProducts)
                    {
                        purchaseReturnProductList.Add(new PurchaseReturnProductDetail
                        {
                            ProductId = item.ProductId,
                            ProductShortDesc = item.ProductShortDesc,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            DiscountPercentage = item.DiscountPercentage,
                            DiscountAmount = item.DiscountAmount,
                            TotalPrice = item.TotalPrice,
                            CGST_Perc = item.CGST_Perc,
                            CGST_Amount = item.CGST_Amount,
                            SGST_Perc = item.SGST_Perc,
                            SGST_Amount = item.SGST_Amount,
                            IGST_Perc = item.IGST_Perc,
                            IGST_Amount = item.IGST_Amount,
                            HSN_Code = item.HSN_Code
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditPurchaseReturn(purchaseReturn, purchaseReturnProductList);

            }

            catch (Exception ex)
            {
                responseOut.status = ActionStatus.Fail;
                responseOut.message = ActionMessage.ApplicationException;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }

            return responseOut;
        }
        public List<PurchaseReturnViewModel> GetPurchaseReturnList(string purchaseReturnNo, string vendorName, string dispatchrefNo, string fromDate, string toDate, string approvalStatus, int companyId,string companyBranch)
        {
            List<PurchaseReturnViewModel> purchaseReturnViewModel = new List<PurchaseReturnViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDeliveryChallans = sqlDbInterface.GetPurchaseReturnList(purchaseReturnNo, vendorName, dispatchrefNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), approvalStatus, companyId, companyBranch);
                if (dtDeliveryChallans != null && dtDeliveryChallans.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDeliveryChallans.Rows)
                    {
                        purchaseReturnViewModel.Add(new PurchaseReturnViewModel
                        {
                            PurchaseReturnId = Convert.ToInt32(dr["PurchaseReturnId"]),
                            PurchaseReturnNo = Convert.ToString(dr["PurchaseReturnNo"]),
                            PurchaseReturnDate = Convert.ToString(dr["PurchaseReturnDate"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            ReturnType = Convert.ToString(dr["ReturnType"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            companyBranch = Convert.ToString(dr["BranchName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return purchaseReturnViewModel;
        }
        public PurchaseReturnViewModel GetPurchaseReturnDetail(long purchaseReturnId = 0)
        {
            PurchaseReturnViewModel purchaseReturnViewModel = new PurchaseReturnViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetPurchaseReturnDetail(purchaseReturnId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        purchaseReturnViewModel = new PurchaseReturnViewModel
                        {
                            PurchaseReturnId = Convert.ToInt32(dr["PurchaseReturnId"]),
                            PurchaseReturnNo = Convert.ToString(dr["PurchaseReturnNo"]),
                            PurchaseReturnDate = Convert.ToString(dr["PurchaseReturnDate"]),
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),


                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            ReturnType = Convert.ToString(dr["ReturnType"]),

                            CompanyBranchId = Convert.ToInt32(string.IsNullOrEmpty(dr["CompanyBranchId"].ToString()) ? "0" : dr["CompanyBranchId"]),

                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),

                            GrossValue = Convert.ToDecimal(dr["GrossValue"]),
                            RoundOfValue = Convert.ToDecimal(dr["RoundOfValue"]),


                            FreightValue = Convert.ToDecimal(dr["FreightValue"]),
                            FreightCGST_Perc = Convert.ToDecimal(dr["FreightCGST_Perc"]),
                            FreightCGST_Amt = Convert.ToDecimal(dr["FreightCGST_Amt"]),
                            FreightSGST_Perc = Convert.ToDecimal(dr["FreightSGST_Perc"]),
                            FreightSGST_Amt = Convert.ToDecimal(dr["FreightSGST_Amt"]),
                            FreightIGST_Perc = Convert.ToDecimal(dr["FreightIGST_Perc"]),
                            FreightIGST_Amt = Convert.ToDecimal(dr["FreightIGST_Amt"]),

                            LoadingValue = Convert.ToDecimal(dr["LoadingValue"]),
                            LoadingCGST_Perc = Convert.ToDecimal(dr["LoadingCGST_Perc"]),
                            LoadingCGST_Amt = Convert.ToDecimal(dr["LoadingCGST_Amt"]),
                            LoadingSGST_Perc = Convert.ToDecimal(dr["LoadingSGST_Perc"]),
                            LoadingSGST_Amt = Convert.ToDecimal(dr["LoadingSGST_Amt"]),
                            LoadingIGST_Perc = Convert.ToDecimal(dr["LoadingIGST_Perc"]),
                            LoadingIGST_Amt = Convert.ToDecimal(dr["LoadingIGST_Amt"]),

                            InsuranceValue = Convert.ToDecimal(dr["InsuranceValue"]),
                            InsuranceCGST_Perc = Convert.ToDecimal(dr["InsuranceCGST_Perc"]),
                            InsuranceCGST_Amt = Convert.ToDecimal(dr["InsuranceCGST_Amt"]),
                            InsuranceSGST_Perc = Convert.ToDecimal(dr["InsuranceSGST_Perc"]),
                            InsuranceSGST_Amt = Convert.ToDecimal(dr["InsuranceSGST_Amt"]),
                            InsuranceIGST_Perc = Convert.ToDecimal(dr["InsuranceIGST_Perc"]),
                            InsuranceIGST_Amt = Convert.ToDecimal(dr["InsuranceIGST_Amt"]),

                            Remarks = Convert.ToString(dr["Remarks"]),

                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return purchaseReturnViewModel;
        }
        public List<PurchaseReturnProductViewModel> GetPurchaseReturnProductList(long purchaseReturnId)
        {
            List<PurchaseReturnProductViewModel> purchaseReturnProducts = new List<PurchaseReturnProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetPurchaseReturnProductList(purchaseReturnId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        purchaseReturnProducts.Add(new PurchaseReturnProductViewModel
                        {
                            PurchaseReturnProductDetailId = Convert.ToInt32(dr["PurchaseReturnProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            DiscountPercentage = Convert.ToDecimal(dr["DiscountPercentage"]),
                            DiscountAmount = Convert.ToDecimal(dr["DiscountAmount"]),
                            TotalPrice = Convert.ToDecimal(dr["TotalPrice"]),
                            CGST_Perc = Convert.ToDecimal(dr["CGST_Perc"]),
                            CGST_Amount = Convert.ToDecimal(dr["CGST_Amount"]),
                            SGST_Perc = Convert.ToDecimal(dr["SGST_Perc"]),
                            SGST_Amount = Convert.ToDecimal(dr["SGST_Amount"]),
                            IGST_Perc = Convert.ToDecimal(dr["IGST_Perc"]),
                            IGST_Amount = Convert.ToDecimal(dr["IGST_Amount"]),
                            HSN_Code = Convert.ToString(dr["HSN_Code"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return purchaseReturnProducts;
        }
        public DataTable GetPurchaseReturnDetailDataTable(long purchaseReturnId = 0)
        {
            PurchaseReturnViewModel purchaseReturnViewModel = new PurchaseReturnViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPurchaseReturn = new DataTable();
            try
            {
                dtPurchaseReturn = sqlDbInterface.GetPurchaseReturnDetail(purchaseReturnId);
                
                
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPurchaseReturn;
        }
        public DataTable GetPurchaseReturnProductListDataTable(long challanId)
        {

            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetPurchaseReturnProductList(challanId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }
        public List<PurchaseInvoiceProductDetailViewModel> GetPurchaseReturnPIProductList(long InvoiceId)
        {
            List<PurchaseInvoiceProductDetailViewModel> piProducts = new List<PurchaseInvoiceProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetPurchaseReturnPIProductList(InvoiceId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        piProducts.Add(new PurchaseInvoiceProductDetailViewModel
                        {
                            InvoiceProductDetailId = Convert.ToInt32(dr["InvoiceProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            DiscountPercentage = Convert.ToDecimal(dr["DiscountPercentage"]),
                            DiscountAmount = Convert.ToDecimal(dr["DiscountAmount"]),
                            TaxId = Convert.ToInt32(dr["TaxId"]),
                            TaxName = Convert.ToString(dr["TaxName"]),
                            TaxPercentage = Convert.ToDecimal(dr["TaxPercentage"]),
                            TaxAmount = Convert.ToDecimal(dr["TaxAmount"]),
                            TotalPrice = Convert.ToDecimal(dr["TotalPrice"]),
                            SurchargeName_1 = Convert.ToString(dr["SurchargeName_1"]),
                            SurchargePercentage_1 = Convert.ToDecimal(dr["SurchargePercentage_1"]),
                            SurchargeAmount_1 = Convert.ToDecimal(dr["SurchargeAmount_1"]),
                            SurchargeName_2 = Convert.ToString(dr["SurchargeName_2"]),
                            SurchargePercentage_2 = Convert.ToDecimal(dr["SurchargePercentage_2"]),
                            SurchargeAmount_2 = Convert.ToDecimal(dr["SurchargeAmount_2"]),
                            SurchargeName_3 = Convert.ToString(dr["SurchargeName_3"]),
                            SurchargePercentage_3 = Convert.ToDecimal(dr["SurchargePercentage_3"]),
                            SurchargeAmount_3 = Convert.ToDecimal(dr["SurchargeAmount_3"]),
                            CGST_Perc = Convert.ToDecimal(dr["CGST_Perc"]),
                            CGST_Amount = Convert.ToDecimal(dr["CGST_Amount"]),
                            SGST_Perc = Convert.ToDecimal(dr["SGST_Perc"]),
                            SGST_Amount = Convert.ToDecimal(dr["SGST_Amount"]),
                            IGST_Perc = Convert.ToDecimal(dr["IGST_Perc"]),
                            IGST_Amount = Convert.ToDecimal(dr["IGST_Amount"]),
                            HSN_Code = Convert.ToString(dr["HSN_Code"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return piProducts;
        }
      
        public List<PurchaseInvoiceViewModel> GetPurchasereturnPIList(string piNo, string vendorName, string refNo, string fromDate, string toDate, int companyId, string approvalStatus = "", string displayType = "", string vendorCode = "", string companyBranchId = "")
        {
            List<PurchaseInvoiceViewModel> pos = new List<PurchaseInvoiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPIs = sqlDbInterface.GetPurchaseReturnPIList(piNo, vendorName, refNo, fromDate, toDate, companyId, approvalStatus, displayType, vendorCode, companyBranchId);
                if (dtPIs != null && dtPIs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPIs.Rows)
                    {
                        pos.Add(new PurchaseInvoiceViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pos;
        }
    }
}
