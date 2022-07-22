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
    public class SaleReturnBL
    {
        DBInterface dbInterface;
        public SaleReturnBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditSaleReturn(SaleReturnViewModel saleReturnViewModel, List<SaleReturnProductViewModel> saleReturnProducts,List<SaleRetrunProductSerialDetailViewModel> saleRetrunProductSerialDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                SaleReturn saleReturn = new SaleReturn
                {
                    SaleReturnId = saleReturnViewModel.SaleReturnId,
                    SaleReturnDate = Convert.ToDateTime(saleReturnViewModel.SaleReturnDate),
                    ReturnType=saleReturnViewModel.ReturnType,
                    InvoiceNo = saleReturnViewModel.InvoiceNo,
                    InvoiceId = saleReturnViewModel.InvoiceId,
                    CompanyBranchId = saleReturnViewModel.CompanyBranchId,
                    CustomerId = saleReturnViewModel.CustomerId,
                    CustomerName = saleReturnViewModel.CustomerName,                   
                    ContactPerson = saleReturnViewModel.ContactPerson,                   
                    BillingAddress = saleReturnViewModel.BillingAddress,
                    City = saleReturnViewModel.City,
                    StateId = saleReturnViewModel.StateId,
                    CountryId = saleReturnViewModel.CountryId,
                    PinCode = saleReturnViewModel.PinCode,
                    GSTNo = saleReturnViewModel.GSTNo,
                    Email = saleReturnViewModel.Email,
                    MobileNo = saleReturnViewModel.MobileNo,
                    ContactNo = saleReturnViewModel.ContactNo,
                    Fax = saleReturnViewModel.Fax,                    
                    RefNo = saleReturnViewModel.RefNo,                   
                    RefDate = string.IsNullOrEmpty(saleReturnViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(saleReturnViewModel.RefDate),
                    BasicValue = saleReturnViewModel.BasicValue,
                    TotalValue = saleReturnViewModel.TotalValue,
                    FreightValue = saleReturnViewModel.FreightValue,
                    FreightCGST_Perc = saleReturnViewModel.FreightCGST_Perc,
                    FreightCGST_Amt = saleReturnViewModel.FreightCGST_Amt,
                    FreightSGST_Perc = saleReturnViewModel.FreightSGST_Perc,
                    FreightSGST_Amt = saleReturnViewModel.FreightSGST_Amt,
                    FreightIGST_Perc = saleReturnViewModel.FreightIGST_Perc,
                    FreightIGST_Amt = saleReturnViewModel.FreightIGST_Amt,
                    LoadingValue = saleReturnViewModel.LoadingValue,
                    LoadingCGST_Perc = saleReturnViewModel.LoadingCGST_Perc,
                    LoadingCGST_Amt = saleReturnViewModel.LoadingCGST_Amt,
                    LoadingSGST_Perc = saleReturnViewModel.LoadingSGST_Perc,
                    LoadingSGST_Amt = saleReturnViewModel.LoadingSGST_Amt,
                    LoadingIGST_Perc = saleReturnViewModel.LoadingIGST_Perc,
                    LoadingIGST_Amt = saleReturnViewModel.LoadingIGST_Amt,
                    InsuranceValue = saleReturnViewModel.InsuranceValue,
                    InsuranceCGST_Perc = saleReturnViewModel.InsuranceCGST_Perc,
                    InsuranceCGST_Amt = saleReturnViewModel.InsuranceCGST_Amt,
                    InsuranceSGST_Perc = saleReturnViewModel.InsuranceSGST_Perc,
                    InsuranceSGST_Amt = saleReturnViewModel.InsuranceSGST_Amt,
                    InsuranceIGST_Perc = saleReturnViewModel.InsuranceIGST_Perc,
                    InsuranceIGST_Amt = saleReturnViewModel.InsuranceIGST_Amt,
                    Remarks = saleReturnViewModel.Remarks,
                    ApprovalStatus= saleReturnViewModel.ApprovalStatus,
                    InvoiceStatus = saleReturnViewModel.InvoiceStatus,
                    FinYearId = saleReturnViewModel.FinYearId,
                    CompanyId = saleReturnViewModel.CompanyId,
                    CreatedBy = saleReturnViewModel.CreatedBy,
                    RoundOfValue = saleReturnViewModel.RoundOfValue,
                    GrossValue = saleReturnViewModel.GrossValue,


                };
                List<SaleReturnProductDetail> saleReturnProductList = new List<SaleReturnProductDetail>();
                if (saleReturnProducts != null && saleReturnProducts.Count > 0)
                {
                    foreach (SaleReturnProductViewModel item in saleReturnProducts)
                    {
                        saleReturnProductList.Add(new SaleReturnProductDetail
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

                //

                List<SaleRetrunProductSerialDetail> saleRetrunProductSerialDetailList = new List<SaleRetrunProductSerialDetail>();
                if (saleRetrunProductSerialDetails != null && saleRetrunProductSerialDetails.Count > 0)
                {
                    foreach (SaleRetrunProductSerialDetailViewModel item in saleRetrunProductSerialDetails)
                    {
                        saleRetrunProductSerialDetailList.Add(new SaleRetrunProductSerialDetail
                        {
                           ProductId=item.ProductId,
                           InvoiceSerialId=item.InvoiceSerialId,
                           RefSerial1= item.RefSerial1,
                            status=item.serialStatus
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditSaleReturn(saleReturn, saleReturnProductList, saleRetrunProductSerialDetailList); 

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


        public ResponseOut CancelSaleReturn(SaleReturnViewModel saleReturnViewModel)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                SaleReturn saleReturn = new SaleReturn
                {
                    SaleReturnId = saleReturnViewModel.SaleReturnId,
                    SaleReturnNo = saleReturnViewModel.SaleReturnNo,
                    CancelStatus = "Cancel",
                    ApprovalStatus = "Cancelled",
                    CreatedBy = saleReturnViewModel.CreatedBy,
                    CancelReason = saleReturnViewModel.CancelReason,
                    CompanyId = saleReturnViewModel.CompanyId,
                    FinYearId = saleReturnViewModel.FinYearId
                };
                responseOut = sqlDbInterface.CancelSaleReturn(saleReturn);
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
        public List<SaleReturnViewModel> GetSaleReturnList(string saleReturnNo, string customerName, string dispatchrefNo, string fromDate, string toDate, string approvalStatus, int companyId, string CreatedByUserName, int companyBranchId,int CustomerId=0)
        {
            List<SaleReturnViewModel> saleReturnViewModel = new List<SaleReturnViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDeliveryChallans = sqlDbInterface.GetSaleReturnList(saleReturnNo, customerName, dispatchrefNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), approvalStatus, companyId, CreatedByUserName, companyBranchId,CustomerId);
                if (dtDeliveryChallans != null && dtDeliveryChallans.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDeliveryChallans.Rows)
                    {
                        saleReturnViewModel.Add(new SaleReturnViewModel
                        {
                            SaleReturnId = Convert.ToInt32(dr["SaleReturnId"]),
                            SaleReturnNo = Convert.ToString(dr["SaleReturnNo"]),
                            SaleReturnDate = Convert.ToString(dr["SaleReturnDate"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            BillingAddress=Convert.ToString(dr["BillingAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            ReturnType=Convert.ToString(dr["ReturnType"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            ApprovalStatus=Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"])
                            
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return saleReturnViewModel;
        }
        public SaleReturnViewModel GetSaleReturnDetail(long saleReturnId = 0)
        {
            SaleReturnViewModel saleReturnViewModel = new SaleReturnViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetSaleReturnDetail(saleReturnId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        saleReturnViewModel = new SaleReturnViewModel
                        {
                            SaleReturnId = Convert.ToInt32(dr["SaleReturnId"]),
                            SaleReturnNo = Convert.ToString(dr["SaleReturnNo"]),
                            SaleReturnDate = Convert.ToString(dr["SaleReturnDate"]),
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                           

                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),

                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),

                            ContactPerson = Convert.ToString(dr["ContactPerson"]),
                            BillingAddress = Convert.ToString(dr["BillingAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            ReturnType = Convert.ToString(dr["ReturnType"]),
                            Email = Convert.ToString(dr["Email"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            Fax = Convert.ToString(dr["Fax"]),

                            CompanyBranchId = Convert.ToInt32(string.IsNullOrEmpty(dr["CompanyBranchId"].ToString())? "0": dr["CompanyBranchId"]),
                           
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),                                                          
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),

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
                            GrossValue = Convert.ToDecimal(dr["GrossValue"]),
                            RoundOfValue = Convert.ToDecimal(dr["RoundOfValue"]),

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
            return saleReturnViewModel;
        }        
        public List<SaleReturnProductViewModel> GetSaleReturnProductList(long saleReturnId)
        {
            List<SaleReturnProductViewModel> saleReturnProducts = new List<SaleReturnProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetSaleReturnProductList(saleReturnId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        saleReturnProducts.Add(new SaleReturnProductViewModel
                        {
                            SaleReturnProductDetailId = Convert.ToInt32(dr["SaleReturnProductDetailId"]),
                            SequenceNo=Convert.ToInt32(dr["SNo"]),
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
                            HSN_Code = Convert.ToString(dr["HSN_Code"]),
                            IsSerializedProduct= Convert.ToString(dr["IsSerializedProduct"]) 
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return saleReturnProducts;
        }

        public DataTable GetSaleReturnDetailDataTable(long saleReturnId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtSaleReturnDetails = new DataTable();
            try
            {
                dtSaleReturnDetails = sqlDbInterface.GetSaleReturnDetail(saleReturnId);
                            
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtSaleReturnDetails;
        }
        public DataTable GetSaleReturnProductListDataTable(long saleReturnId)
        {
            DataTable dtSaleReturnProducts = new DataTable();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtSaleReturnProducts = sqlDbInterface.GetSaleReturnProductList(saleReturnId);
                }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtSaleReturnProducts;
        }
        public List<SaleInvoiceViewModel> GetSaleReturnSIList(string saleinvoiceNo, string customerName, string refNo, string fromDate, string toDate, int companyId, string invoiceType = "", string displayType = "", string approvalStatus = "", string customerCode = "", int companyBranchId = 0)
        {
            List<SaleInvoiceViewModel> saleinvoices = new List<SaleInvoiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSaleInvoices = sqlDbInterface.GetSaleReturnSIList(saleinvoiceNo, customerName, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, invoiceType, displayType, approvalStatus, customerCode, companyBranchId);
                if (dtSaleInvoices != null && dtSaleInvoices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSaleInvoices.Rows)
                    {
                        saleinvoices.Add(new SaleInvoiceViewModel
                        {
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            SONo = Convert.ToString(dr["SONo"]),
                            InvoiceType = Convert.ToString(dr["InvoiceType"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            ConsigneeId = Convert.ToInt32(dr["ConsigneeId"]),
                            ConsigneeName = Convert.ToString(dr["ConsigneeName"]),
                            ConsigneeCode = Convert.ToString(dr["ConsigneeCode"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
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
            return saleinvoices;
        }




    }
}
