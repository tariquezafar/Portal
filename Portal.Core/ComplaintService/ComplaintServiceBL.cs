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
using Portal.Common.ViewModel;

namespace Portal.Core
{
    public class ComplaintServiceBL
    {
        DBInterface dbInterface;
        public ComplaintServiceBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut AddEditComplaintService(ComplaintServiceViewModel complaintServiceViewModel, List<ComplaintServiceProductDetailViewModel> complaintProduct, List<ComplaintServiceSupportingDocumentViewModel> complaintDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                ComplaintService complaintService = new ComplaintService
                {
                    ComplaintId = complaintServiceViewModel.ComplaintId,
                    EnquiryType = complaintServiceViewModel.EnquiryType,
                    ComplaintMode = complaintServiceViewModel.ComplaintMode,
                    ComplaintDescription = complaintServiceViewModel.ComplaintDescription,
                    CustomerName = complaintServiceViewModel.CustomerName,
                    CustomerMobile = complaintServiceViewModel.CustomerMobile,
                    CustomerEmail = complaintServiceViewModel.CustomerEmail,
                    CustomerAddress = complaintServiceViewModel.CustomerAddress,
                    Status = complaintServiceViewModel.ComplaintService_Status,
                    BranchID = complaintServiceViewModel.BranchID,
                    ComplaintDate = Convert.ToDateTime(complaintServiceViewModel.ComplaintDate),
                    InvoiceNo = complaintServiceViewModel.InvoiceNo,
                    EmployeeID = complaintServiceViewModel.EmployeeID,
                    DealerID = complaintServiceViewModel.DealerID,
                    InvoiceDate = string.IsNullOrEmpty(complaintServiceViewModel.InvoiceDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(complaintServiceViewModel.InvoiceDate),
                    ComplaintStatus = complaintServiceViewModel.ComplaintStatus
                };
                List<ComplaintServiceProductDetail> complaintProductList = new List<ComplaintServiceProductDetail>();
                if (complaintProduct != null && complaintProduct.Count > 0)
                {
                    foreach (ComplaintServiceProductDetailViewModel item in complaintProduct)
                    {
                        complaintProductList.Add(new ComplaintServiceProductDetail
                        {
                            ComplaintProductDetailID = item.ComplaintProductDetailID,
                            ComplaintId = item.ComplaintId,
                            ProductId = item.ProductId,
                            Remarks = item.Remarks,
                            Quantity = item.Quantity
                        });
                    }
                }

                List<ComplaintSupportingDocument> complaintDocumentsList = new List<ComplaintSupportingDocument>();
                if (complaintDocuments != null && complaintDocuments.Count > 0)
                {
                    foreach (var item in complaintDocuments)
                    {
                        complaintDocumentsList.Add(new ComplaintSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditComplaintService(complaintService, complaintProductList, complaintDocumentsList);

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

        public ComplaintServiceViewModel GetComplaintServiceDetail(int ComplaintId = 0)
        {
            ComplaintServiceViewModel complaints = new ComplaintServiceViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtComplaintProducts = sqlDbInterface.GetComplaintServiceDetail(ComplaintId);
                if (dtComplaintProducts != null && dtComplaintProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtComplaintProducts.Rows)
                    {
                        complaints = new ComplaintServiceViewModel
                        {
                            ComplaintId = Convert.ToInt32(dr["ComplaintId"]),
                            ComplaintDate= Convert.ToString(dr["ComplaintDate"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            ComplaintNo = Convert.ToString(dr["ComplaintNo"]),
                            EnquiryType = Convert.ToString(dr["EnquiryType"]),
                            ComplaintMode = Convert.ToString(dr["ComplaintMode"]),
                            CustomerMobile = Convert.ToString(dr["CustomerMobile"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerAddress = Convert.ToString(dr["CustomerAddress"]),
                            CustomerEmail = Convert.ToString(dr["CustomerEmail"]),
                            ComplaintService_Status = Convert.ToBoolean(dr["Status"]),
                            ComplaintDescription=Convert.ToString(dr["ComplaintDescription"]),
                            BranchID= Convert.ToInt32(dr["BranchID"]),
                            EmployeeID = Convert.ToInt32(dr["EmployeeID"]),
                            DealerID = Convert.ToInt32(dr["DealerID"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            ComplaintStatus = Convert.ToInt32(dr["ComplaintStatus"]),

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return complaints;
        }



        public List<ComplaintServiceProductDetailViewModel> GetComplaintServiceProductList(long ComplaintId)
        {
            List<ComplaintServiceProductDetailViewModel> complaintProducts = new List<ComplaintServiceProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtComplaintProducts = sqlDbInterface.GetComplaintServiceProductList(ComplaintId);
                if (dtComplaintProducts != null && dtComplaintProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtComplaintProducts.Rows)
                    {
                        complaintProducts.Add(new ComplaintServiceProductDetailViewModel
                        {
                            ComplaintProductDetailID = Convert.ToInt32(dr["ComplaintProductDetailID"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            Quantity = Convert.ToInt32(dr["Quantity"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return complaintProducts;
        }

        public List<ComplaintServiceViewModel> GetComplaintServiceList(string complaintNo, string enquiryType, string complaintMode, string customerMobile, string customerName, string approvalStatus, int companyBranchId, int serviceEngineerId, int dealerId, int complaintStatus)
        {
            List<ComplaintServiceViewModel> complaints = new List<ComplaintServiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtComplaints = sqlDbInterface.GetComplaintServiceList(complaintNo, enquiryType, complaintMode, customerMobile, customerName, approvalStatus, companyBranchId, serviceEngineerId, dealerId, complaintStatus);
                if (dtComplaints != null && dtComplaints.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtComplaints.Rows)
                    {
                        complaints.Add(new ComplaintServiceViewModel
                        {
                            ComplaintId = Convert.ToInt32(dr["ComplaintId"]),
                            ComplaintNo = Convert.ToString(dr["ComplaintNo"]),
                            EnquiryType = Convert.ToString(dr["EnquiryType"]),
                            ComplaintMode = Convert.ToString(dr["ComplaintMode"]),
                            CustomerMobile = Convert.ToString(dr["CustomerMobile"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            ComplaintService_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            DealerName = Convert.ToString(dr["DealerName"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            ComplaintStatus = Convert.ToInt32(dr["ComplaintStatus"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return complaints;
        }

        public List<ComplaintServiceSupportingDocumentViewModel> GetComplaintSupportingDocumentList(long ComplaintId)
        {
            List<ComplaintServiceSupportingDocumentViewModel> employeeDocumentsList = new List<ComplaintServiceSupportingDocumentViewModel>();
            try
            {
                List<ComplaintSupportingDocument> employeeDocuments = dbInterface.GetComplaintDocumentTypeList(ComplaintId);
                if (employeeDocuments != null && employeeDocuments.Count > 0)
                {
                    foreach (var item in employeeDocuments)
                    {
                        employeeDocumentsList.Add(new ComplaintServiceSupportingDocumentViewModel
                        {
                            ComplaintDocId = item.ComplaintDocId,
                            ComplaintId = item.ComplaintId ?? 0,
                            DocumentTypeId = item.DocumentTypeId ?? 0,
                            DocumentTypeDesc = "Sales",
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeDocumentsList;
        }

        public List<SaleInvoiceProductViewModel> GetComplaintServiceSIProductList(long saleinvoiceId)
        {
            List<SaleInvoiceProductViewModel> saleinvoiceProducts = new List<SaleInvoiceProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetComplaintServiceSIProductList(saleinvoiceId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        saleinvoiceProducts.Add(new SaleInvoiceProductViewModel
                        {
                            InvoiceProductDetailId = Convert.ToInt32(dr["InvoiceProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            WarrantyStartDate  = Convert.ToString(dr["WarrantyStartDate"]),
                            WarrantyEndDate= Convert.ToString(dr["WarrantyEndDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return saleinvoiceProducts;
        }

        /// <summary>
        /// This method is used to get customer Type List.
        /// Author By : Dheeraj kumar on 21 May, 2022
        /// </summary>
        /// <param name="customerTypeId">primary key of the table</param>
        /// <returns>
        /// This method retruns list of customer based on parameters.
        /// </returns>
        public List<SelectListModel> GetCustomerTypeList(int customerTypeId)
        {
            List<SelectListModel> lstSelectListModel = new List<SelectListModel>();
            try
            {
                lstSelectListModel = dbInterface.GetCustomerTypeList(customerTypeId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return lstSelectListModel;
        }
    }
}
