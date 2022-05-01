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
    public class PurchaseQuotationBL
    {
        DBInterface dbInterface;
        public PurchaseQuotationBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditPurchaseQuotation(PurchaseQuotationViewModel quotationViewModel,List<PurchaseQuotationProductViewModel> quotationProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                PurchaseQuotation quotation = new PurchaseQuotation
                {
                    QuotationId = quotationViewModel.QuotationId,
                    QuotationDate = Convert.ToDateTime(quotationViewModel.QuotationDate),
                    RequisitionId= quotationViewModel.RequisitionId,
                    RequisitionNo= quotationViewModel.RequisitionNo,
                    CompanyBranchId = quotationViewModel.CompanyBranchId,
                    CurrencyCode = quotationViewModel.CurrencyCode,
                    VendorId = quotationViewModel.VendorId,
                    VendorName = quotationViewModel.VendorName,
                    DeliveryDays = quotationViewModel.DeliveryDays,
                    DeliveryAt= quotationViewModel.DeliveryAt,
                    RefNo = quotationViewModel.RefNo,
                    RefDate = string.IsNullOrEmpty(quotationViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(quotationViewModel.RefDate),
                    BasicValue = quotationViewModel.BasicValue,
                    TotalValue = quotationViewModel.TotalValue,
                    FreightValue = quotationViewModel.FreightValue,
                    FreightCGST_Perc = quotationViewModel.FreightCGST_Perc,
                    FreightCGST_Amt = quotationViewModel.FreightCGST_Amt,
                    FreightSGST_Perc = quotationViewModel.FreightSGST_Perc,
                    FreightSGST_Amt = quotationViewModel.FreightSGST_Amt,
                    FreightIGST_Perc = quotationViewModel.FreightIGST_Perc,
                    FreightIGST_Amt = quotationViewModel.FreightIGST_Amt,
                    LoadingValue = quotationViewModel.LoadingValue,
                    LoadingCGST_Perc = quotationViewModel.LoadingCGST_Perc,
                    LoadingCGST_Amt = quotationViewModel.LoadingCGST_Amt,
                    LoadingSGST_Perc = quotationViewModel.LoadingSGST_Perc,
                    LoadingSGST_Amt = quotationViewModel.LoadingSGST_Amt,
                    LoadingIGST_Perc = quotationViewModel.LoadingIGST_Perc,
                    LoadingIGST_Amt = quotationViewModel.LoadingIGST_Amt,
                    InsuranceValue = quotationViewModel.InsuranceValue,
                    InsuranceCGST_Perc = quotationViewModel.InsuranceCGST_Perc,
                    InsuranceCGST_Amt = quotationViewModel.InsuranceCGST_Amt,
                    InsuranceSGST_Perc = quotationViewModel.InsuranceSGST_Perc,
                    InsuranceSGST_Amt = quotationViewModel.InsuranceSGST_Amt,
                    InsuranceIGST_Perc = quotationViewModel.InsuranceIGST_Perc,
                    InsuranceIGST_Amt = quotationViewModel.InsuranceIGST_Amt,
                    Remarks1 = quotationViewModel.Remarks1,
                    Remarks2 = quotationViewModel.Remarks2,
                    FinYearId = quotationViewModel.FinYearId,
                    CompanyId = quotationViewModel.CompanyId,
                    CreatedBy = quotationViewModel.CreatedBy,
                    ApprovalStatus= quotationViewModel.ApprovalStatus

                };
                List<PurchaseQuotationProductDetail> quotationProductList = new List<PurchaseQuotationProductDetail>();
                if(quotationProducts!=null && quotationProducts.Count>0)
                {
                    foreach(PurchaseQuotationProductViewModel item in quotationProducts)
                    {
                        quotationProductList.Add(new PurchaseQuotationProductDetail
                        {
                            ProductId=item.ProductId,
                            ProductShortDesc=item.ProductShortDesc,
                            Price=item.Price,
                            Quantity=item.Quantity,
                            DiscountPercentage=item.DiscountPercentage,
                            DiscountAmount=item.DiscountAmount,
                            TotalPrice=item.TotalPrice,
                            HSN_Code = item.HSN_Code
                        });
                    }
                }
             responseOut = sqlDbInterface.AddEditPurchaseQuotation(quotation, quotationProductList);
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
        public ResponseOut RemoveCustomerBranch(long customerBranchId)
        {
            
            ResponseOut responseOut = new ResponseOut();
            try
                {
                    responseOut = dbInterface.RemoveCustomerBranch(customerBranchId);
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
        public ResponseOut RemoveCustomerProduct(long mappingId)
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {
                responseOut = dbInterface.RemoveCustomerProduct(mappingId);
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
        public List<PurchaseQuotationViewModel> GetPurchaseQuotationList(string quotationNo, string vendorName, string refNo, string fromDate, string toDate, int companyId, string displayType = "",string approvalStatus="",string companyBranch="")
        {
            List<PurchaseQuotationViewModel> quotations = new List<PurchaseQuotationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetPurchaseQuotationList(quotationNo, vendorName, refNo, Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate),companyId, displayType, approvalStatus, companyBranch);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        quotations.Add(new PurchaseQuotationViewModel
                        {
                            QuotationId = Convert.ToInt32(dr["QuotationId"]),
                            QuotationNo = Convert.ToString(dr["QuotationNo"]),
                            QuotationDate = Convert.ToString(dr["QuotationDate"]),
                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            VendorName= Convert.ToString(dr["VendorName"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode= Convert.ToString(dr["VendorCode"]),
                            DeliveryDays=Convert.ToInt32(dr["DeliveryDays"]),
                            DeliveryAt =Convert.ToString(dr["DeliveryAt"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            CreatedByUser = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ApprovalStatus=Convert.ToString(dr["ApprovalStatus"]),
                            ModifiedByUser = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            companyBranch= Convert.ToString(dr["companyBranch"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return quotations;
        }
        public PurchaseQuotationViewModel GetPurchaseQuotationDetail(long quotationId = 0)
        {
            PurchaseQuotationViewModel quotation = new PurchaseQuotationViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetPurchaseQuotationDetail(quotationId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        quotation = new PurchaseQuotationViewModel
                        {
                            QuotationId = Convert.ToInt32(dr["QuotationId"]),
                            QuotationNo = Convert.ToString(dr["QuotationNo"]),
                            QuotationDate = Convert.ToString(dr["QuotationDate"]),
                            RequisitionId=Convert.ToInt32(dr["IndentId"]),
                            RequisitionNo = Convert.ToString(dr["IndentNo"]),
                            RequisitionDate = Convert.ToString(dr["IndentDate"]),
                            CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"]),
                            CurrencyCode = Convert.ToString(dr["CurrencyCode"]),
                            VendorId=Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            DeliveryAt = Convert.ToString(dr["DeliveryAt"]),
                            DeliveryDays = Convert.ToInt32(dr["DeliveryDays"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            BasicValue = string.IsNullOrEmpty(dr["BasicValue"].ToString()) ? Convert.ToDecimal("0.0") : Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = string.IsNullOrEmpty(dr["TotalValue"].ToString()) ? Convert.ToDecimal("0.0") : Convert.ToDecimal(dr["TotalValue"]),                         
                            Remarks1 = string.IsNullOrEmpty(dr["Remarks1"].ToString())?"":Convert.ToString(dr["Remarks1"]),
                            Remarks2 = string.IsNullOrEmpty(dr["Remarks2"].ToString()) ? "" : Convert.ToString(dr["Remarks2"]),
                            CreatedByUser = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUser = Convert.ToString(dr["ModifiedByName"]),
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
            return quotation;
        }
        public DataTable GetQuotationDetailDataTable(long quotationId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtQuotation = new DataTable();
            try
            {
                 dtQuotation = sqlDbInterface.GetQuotationDetail(quotationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtQuotation;
        }
        public List<CustomerBranchViewModel> GetCustomerBranchList(int customerId)
        {
            List<CustomerBranchViewModel> customerBranchs = new List<CustomerBranchViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetCustomerBranchList(customerId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        customerBranchs.Add(new CustomerBranchViewModel
                        {
                            CustomerBranchId = Convert.ToInt32(dr["CustomerBranchId"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            PrimaryAddress = Convert.ToString(dr["PrimaryAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            CSTNo = Convert.ToString(dr["CSTNo"]),
                            TINNo = Convert.ToString(dr["TINNo"]),
                            PANNo = Convert.ToString(dr["PANNo"]),
                            GSTNo = Convert.ToString(dr["GSTNo"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]),
                            Designation = Convert.ToString(dr["Designation"]),
                            Email = Convert.ToString(dr["Email"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            Fax = Convert.ToString(dr["Fax"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerBranchs;
        }
        public List<PurchaseQuotationProductViewModel> GetPurchaseQuotationProductList(long quotationId)
        {
            List<PurchaseQuotationProductViewModel> quotationProducts = new List<PurchaseQuotationProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetPurchaseQuotationProductList(quotationId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        quotationProducts.Add(new PurchaseQuotationProductViewModel
                        {
                            QuotationProductDetailId = Convert.ToInt32(dr["QuotationProductDetailId"]),
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
            return quotationProducts;
        } 
        public DataTable GetQuotationProductListDataTable(long quotationId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                 dtProducts = sqlDbInterface.GetPurchaseQuotationProductList(quotationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }
        public DataTable GetPurchaseQuotationComparisonList(long indentId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtQuotations = new DataTable();
            try
            {
                dtQuotations = sqlDbInterface.GetPurchaseQuotationComparisonList(indentId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtQuotations;
        }
        public List<PurchaseIndentViewModel> GetPurchaseQuotationIndentList(string indentNo, string indentType, string customerName, int companyBranchId, DateTime fromDate, DateTime toDate, int companyId, string displayType = "", string approvalStatus = "0",int createBy=0)
        {
            List<PurchaseIndentViewModel> indents = new List<PurchaseIndentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtIndents = sqlDbInterface.GetPurchaseQuotationIndentList(indentNo, indentType, customerName, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus, Convert.ToString(createBy));
                if (dtIndents != null && dtIndents.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtIndents.Rows)
                    {
                        indents.Add(new PurchaseIndentViewModel
                        {
                            IndentId = Convert.ToInt32(dr["IndentId"]),
                            IndentNo = Convert.ToString(dr["IndentNo"]),
                            IndentDate = Convert.ToString(dr["IndentDate"]),
                            IndentType = Convert.ToString(dr["IndentType"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerId = Convert.ToInt32(dr["CustomerID"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            IndentStatus = string.IsNullOrEmpty(Convert.ToString(dr["IndentStatus"])) ? "" : Convert.ToString(dr["IndentStatus"]),
                           
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return indents;
        }

        public DataTable GetPurchaseQuotationProductListDataTable(long quotationID)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetPurchaseQuotationProductList(quotationID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public DataTable GetPurchaseQuotationDataTable(long quotationID = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPurchaseQuotation = new DataTable();
            try
            {
                dtPurchaseQuotation = sqlDbInterface.GetPurchaseQuotationDetail(quotationID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPurchaseQuotation;
        }
    }
}
