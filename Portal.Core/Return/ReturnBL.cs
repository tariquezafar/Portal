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
    public class ReturnBL
    {
        DBInterface dbInterface;
        public ReturnBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditReturn(ReturnViewModel returnViewModel, List<ReturnedProductDetailViewModel> returnedProductDetail)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                Return returns = new Return
                {
                    ReturnedID = returnViewModel.ReturnedID,
                    ReturnedDate = Convert.ToDateTime(returnViewModel.ReturnedDate),
                    InvoiceID = Convert.ToInt32(returnViewModel.InvoiceID),
                    InvoiceNo = Convert.ToString(returnViewModel.InvoiceNo),
                    CompanyId = returnViewModel.CompanyId,
                    CompanyBranchId = returnViewModel.CompanyBranchId,
                    CreatedBy = returnViewModel.CreatedBy,
                    FinYearId = returnViewModel.FinYearId,
                    ApprovalStatus = returnViewModel.ApprovalStatus,
                    Warranty= returnViewModel.Warranty
                };
                List<ReturnedProductDetail> returnedProductList = new List<ReturnedProductDetail>();
                if (returnedProductDetail != null && returnedProductDetail.Count > 0)
                {
                    foreach (ReturnedProductDetailViewModel item in returnedProductDetail)
                    {
                        returnedProductList.Add(new ReturnedProductDetail
                        {
                            ReturnedDetailID = item.ReturnedDetailID,
                            ReturnedID = item.ReturnedID,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            ReplacedQTY = item.ReplacedQTY,
                            ReturnedQty = item.ReturnedQty,
                            Status = item.Status,
                            Remarks = item.Remarks,
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditReturn(returns, returnedProductList);
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

        public List<ReturnViewModel> GetReturnList(string returnedNo, string invoiceNo, string approvalStatus, int companyBranchId, int companyId, string fromDate, string toDate)
        {
            List<ReturnViewModel> returnOrders = new List<ReturnViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetReturnList(returnedNo, invoiceNo, approvalStatus, companyBranchId, companyId,  Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate));
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        returnOrders.Add(new ReturnViewModel
                        {
                            ReturnedID = Convert.ToInt32(dr["ReturnedID"]),
                            ReturnedNo = Convert.ToString(dr["ReturnedNo"]),
                            ReturnedDate = Convert.ToString(dr["ReturnedDate"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),                        
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),                            
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return returnOrders;
        }
        public ReturnViewModel GetReturnDetail(long returnedID)
        {
            ReturnViewModel returnViewModel = new ReturnViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtJobWorks = sqlDbInterface.GetReturnDetail(returnedID);
                if (dtJobWorks != null && dtJobWorks.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtJobWorks.Rows)
                    {
                        returnViewModel = new ReturnViewModel
                        {
                            ReturnedID = Convert.ToInt32(dr["ReturnedID"]),
                            ReturnedNo = Convert.ToString(dr["ReturnedNo"]),
                            ReturnedDate = Convert.ToString(dr["ReturnedDate"]),
                            InvoiceID = Convert.ToInt32(dr["InvoiceID"]),
                            WarrantyID = Convert.ToInt32(dr["WarrantyID"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            CompanyBranchId =Convert.ToInt32(dr["CompanyBranchId"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]) ,
                            InvoicePackingListNo = Convert.ToString(dr["InvoicePackingListNo"]),
                            Warranty = Convert.ToString(dr["Warranty"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return returnViewModel;
        }
    
 
        public List<ReturnedProductDetailViewModel> GetReturnProductList(long returnedID)
        {
            List<ReturnedProductDetailViewModel> returnedProducts = new List<ReturnedProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtJobs = sqlDbInterface.GetReturnProductList(returnedID);
                if (dtJobs != null && dtJobs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtJobs.Rows)
                    {
                        returnedProducts.Add(new ReturnedProductDetailViewModel
                        {
                            ProductId = Convert.ToInt32(dr["Productid"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ReplacedQTY = Convert.ToDecimal(dr["ReplacedQTY"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            ReturnedQty = Convert.ToDecimal(dr["ReturnedQty"]),
                            WarrantyPeriodMonth = Convert.ToInt32(dr["WarrantyPeriodMonth"]),
                            WarrantyStartDate = Convert.ToString(dr["WarrantyStartDate"]),
                            WarrantyEndDate = Convert.ToString(dr["WarrantyEndDate"]),
                            Status = Convert.ToString(dr["Status"]),
                            Remarks = Convert.ToString(dr["Remarks"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return returnedProducts;
        }

        public List<JobWorkINProductDetailViewModel> GetJobWorkProductInList(long jobWorkId)
        {
            List<JobWorkINProductDetailViewModel> jobWorkProducts = new List<JobWorkINProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtJobInProducts = sqlDbInterface.GetJobWorkProductInList(jobWorkId);
                if (dtJobInProducts != null && dtJobInProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtJobInProducts.Rows)
                    {
                        jobWorkProducts.Add(new JobWorkINProductDetailViewModel
                        {
                            JobWorkProductInDetailId = Convert.ToInt32(dr["JobWorkProductInDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UomId = Convert.ToInt32(dr["UomId"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            Weight= Convert.ToDecimal(dr["Weight"]),
                            ProductHSNCode = Convert.ToString(dr["HSN_Code"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return jobWorkProducts;
        }

        public List<WarrantyViewModel> GetSaleInvoiceReturnList(string saleInvoiceNo, string invoicePackingListNo,int companyBranchId)
        {
            List<WarrantyViewModel> jobWorks = new List<WarrantyViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtJobWorks = sqlDbInterface.GetSaleInvoiceReturnList(saleInvoiceNo, invoicePackingListNo,companyBranchId);
                if (dtJobWorks != null && dtJobWorks.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtJobWorks.Rows)
                    {
                        jobWorks.Add(new WarrantyViewModel  
                        {
                            WarrantyID = Convert.ToInt32(dr["WarrantyID"]),                           
                            WarrantyDate = Convert.ToString(dr["WarrantyDate"]),
                            InvoicePackingListNo = Convert.ToString(dr["InvoicePackingListNo"]),                         
                            InvoicePackingListDate = Convert.ToString(dr["InvoicePackingListDate"]),
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            DispatchDate = Convert.ToString(dr["DispatchDate"]), 
                                                      
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return jobWorks;
        }

        public List<ComplaintViewModel> GetComplaintInvoiceReturnList(string complaintInvoiceNo, string customerMobileNo, int companyBranchId)
        {
            List<ComplaintViewModel> jobWorks = new List<ComplaintViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtJobWorks = sqlDbInterface.GetComplaintInvoiceReturnList(complaintInvoiceNo, customerMobileNo, companyBranchId);
                if (dtJobWorks != null && dtJobWorks.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtJobWorks.Rows)
                    {
                        jobWorks.Add(new ComplaintViewModel
                        {
                            ComplaintId = Convert.ToInt32(dr["ComplaintId"]),
                            CustomerMobile = Convert.ToString(dr["CustomerMobile"]),
                            ////ReplacedQTY = Convert.ToDecimal(dr["ReplacedQTY"]),
                            ComplaintDate = Convert.ToString(dr["ComplaintDate"]),
                            ////ReturnedQty = Convert.ToDecimal(dr["ReturnedQty"]),
                            EnquiryType = Convert.ToString(dr["EnquiryType"]),
                            ComplaintNo = Convert.ToString(dr["ComplaintNo"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return jobWorks;
        }

        public List<ReturnedProductDetailViewModel> GetProductDetail(string complaintId, int companyBranch)
        {
            List<ReturnedProductDetailViewModel> so = new List<ReturnedProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetProductDetail(complaintId, companyBranch);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        so.Add(new ReturnedProductDetailViewModel
                        {
                            ////ComplaintId = Convert.ToInt32(dr["ComplaintId"]),
                            ProductId = Convert.ToInt32(dr["Productid"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            WarrantyPeriodMonth = Convert.ToInt32(dr["WarrantyInMonth"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            Status = Convert.ToString(dr["Status"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return so;
        }


        public List<WarrantyProductDetailViewModel> GetProductAutoCompleteWarrantyList(string searchTerm, long warrantyID,int warrantyStatus)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            List<WarrantyProductDetailViewModel> products = new List<WarrantyProductDetailViewModel>();
            try
            {
                DataTable productList = sqlDbInterface.GetProductAutoCompleteWarrantyList(searchTerm, warrantyID, warrantyStatus);
                if (productList != null && productList.Rows.Count > 0)
                {
                    foreach (DataRow dr in productList.Rows)
                    {
                        products.Add(new WarrantyProductDetailViewModel
                        {
                            Productid = Convert.ToInt32(dr["Productid"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),                            
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            WarrantyPeriodMonth = Convert.ToInt32(dr["WarrantyPeriodMonth"]),
                            WarrantyStartDate = Convert.ToString(dr["WarrantyStartDate"]),
                            WarrantyEndDate = Convert.ToString(dr["WarrantyEndDate"]),                          
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return products;
        }


        public DataTable GenerateReturnSummaryReports(string returnedNo, string companyBranchId, string approvalStatus, string invoiceNo, DateTime fromDate, DateTime toDate, string customerName, int userId, int companyId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtReturnProducts = new DataTable();
            try
            {
                dtReturnProducts = sqlDbInterface.GenerateReturnSummaryReports(returnedNo,companyBranchId,approvalStatus,invoiceNo,fromDate,toDate,customerName,userId,companyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtReturnProducts;
        }


        public DataTable GetJobWorkProductListDataTable(long jobWorkId=0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetJobWorkProductList(jobWorkId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public DataTable GetJobWorkDataTable(long jobworkId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtjobWork = new DataTable();
            try
            {
                dtjobWork = sqlDbInterface.GetJobWorkDetail(jobworkId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtjobWork;
        }

        public DataTable GetJobWorkTotalValue(long jobWorkId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTotalValue = new DataTable();
            try
            {
                dtTotalValue = sqlDbInterface.GetJobWorkTotalValue(jobWorkId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTotalValue;
        }

        

    }
}
