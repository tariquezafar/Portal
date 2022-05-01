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
    public class ComplaintServiceBL
    {
        DBInterface dbInterface;
        public ComplaintServiceBL()
        {
            dbInterface = new DBInterface();
        }
        
        public ResponseOut AddEditComplaintService(ComplaintServiceViewModel complaintServiceViewModel, List<ComplaintServiceProductDetailViewModel> complaintProduct)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
                {
                    ComplaintService complaintService = new ComplaintService
                    {
                        ComplaintId = complaintServiceViewModel.ComplaintId,
                        EnquiryType= complaintServiceViewModel.EnquiryType,
                        ComplaintMode = complaintServiceViewModel.ComplaintMode,                       
                        ComplaintDescription = complaintServiceViewModel.ComplaintDescription,
                        CustomerName = complaintServiceViewModel.CustomerName,
                        CustomerMobile = complaintServiceViewModel.CustomerMobile,
                        CustomerEmail = complaintServiceViewModel.CustomerEmail,
                        CustomerAddress = complaintServiceViewModel.CustomerAddress,
                        Status = complaintServiceViewModel.ComplaintService_Status,
                        BranchID=complaintServiceViewModel.BranchID,
                       ComplaintDate= Convert.ToDateTime(complaintServiceViewModel.ComplaintDate),
                       InvoiceNo=complaintServiceViewModel.InvoiceNo,


                    };
                List<ComplaintServiceProductDetail> complaintProductList = new List<ComplaintServiceProductDetail>();
                if (complaintProduct != null && complaintProduct.Count > 0)
                {
                    foreach (ComplaintServiceProductDetailViewModel item in complaintProduct)
                    {
                        complaintProductList.Add(new ComplaintServiceProductDetail
                        {
                            ComplaintProductDetailID = item.ComplaintProductDetailID,
                            ComplaintId=item.ComplaintId,
                           ProductId=item.ProductId,
                          Remarks =item.Remarks
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditComplaintService(complaintService, complaintProductList);

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

        public List<ComplaintServiceViewModel> GetComplaintServiceList(string complaintNo, string enquiryType, string complaintMode, string customerMobile, string customerName, string approvalStatus,int companyBranchId)
        {
            List<ComplaintServiceViewModel> complaints = new List<ComplaintServiceViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtComplaints = sqlDbInterface.GetComplaintServiceList(complaintNo, enquiryType, complaintMode, customerMobile, customerName, approvalStatus, companyBranchId);
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
                            CustomerMobile= Convert.ToString(dr["CustomerMobile"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            ComplaintService_Status =Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"]),
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
    }
}
