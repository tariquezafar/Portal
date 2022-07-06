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
    public class DispatchPlanBL
    {
        DBInterface dbInterface;
        public DispatchPlanBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut AddEditDispatchPlan(DispatchPlanViewModel dispatchPlanViewModel, List<DispatchPlanProductDetailViewModel> dispatchPlanProductDetailViewModels)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DispatchPlan dispatchPlan = new DispatchPlan
                {
                    DispatchPlanID = dispatchPlanViewModel.DispatchPlanID,
                    DispatchPlanDate = Convert.ToDateTime(dispatchPlanViewModel.DispatchPlanDate),
                    CustomerID = dispatchPlanViewModel.CustomerID,
                    CompanyBranchID = dispatchPlanViewModel.CompanyBranchID,
                    CreatedBy = dispatchPlanViewModel.CreatedBy,
                    ApprovalStatus = dispatchPlanViewModel.ApprovalStatus
                };

                List<DispatchPlanProductDetail> dispatchPlanProductDetails = new List<DispatchPlanProductDetail>();
                if (dispatchPlanProductDetailViewModels != null && dispatchPlanProductDetailViewModels.Count > 0)
                {
                    foreach (DispatchPlanProductDetailViewModel item in dispatchPlanProductDetailViewModels)
                    {
                        dispatchPlanProductDetails.Add(new DispatchPlanProductDetail
                        {
                            SOId = item.SOId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Priority = item.Priority
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditDispatchPlan(dispatchPlan, dispatchPlanProductDetails);

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

        public DispatchPlanViewModel GetDispatchPlanDetail(int dispatchPlanID = 0)
        {
            DispatchPlanViewModel dispatchPlanViewModel = new DispatchPlanViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDispatchPlanProducts = sqlDbInterface.GetDispatchPlanDetail(dispatchPlanID);
                if (dtDispatchPlanProducts != null && dtDispatchPlanProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDispatchPlanProducts.Rows)
                    {
                        dispatchPlanViewModel = new DispatchPlanViewModel
                        {
                            DispatchPlanID = Convert.ToInt32(dr["DispatchPlanID"]),
                            DispatchPlanDate = Convert.ToString(dr["DispatchPlanDate"]),
                            DispatchPlanNo = Convert.ToString(dr["DispatchPlanNo"]),
                            CustomerID = Convert.ToInt32(dr["CustomerID"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CompanyBranchID = Convert.ToInt32(dr["CompanyBranchID"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"])

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dispatchPlanViewModel;
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

        
        public List<CustomerSOViewModel> GetSOList(int customerID = 0, string sONO = "", string quotationNo = "", string fromDate = "", string toDate = "", int companyBranchId = 0)
        {
            List<CustomerSOViewModel> customerSOViewModel = new List<CustomerSOViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSaleInvoices = sqlDbInterface.GetSOList(customerID, sONO, quotationNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyBranchId);
                if (dtSaleInvoices != null && dtSaleInvoices.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSaleInvoices.Rows)
                    {
                        customerSOViewModel.Add(new CustomerSOViewModel
                        {
                            SOId = Convert.ToInt32(dr["SOId"]),
                            SONo = Convert.ToString(dr["SONo"]),
                            SODate = Convert.ToString(dr["SODate"]),
                            QuotationNo = Convert.ToString(dr["QuotationNo"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerSOViewModel;
        }

        public List<SOProductViewModel> GetCustomerSOProductList(string sOIds, bool isDispatchPlan)
        {
            List<SOProductViewModel> sOProductViewModellst = new List<SOProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetCustomerSOProductList(sOIds, isDispatchPlan);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        sOProductViewModellst.Add(new SOProductViewModel
                        {
                            SOId = Convert.ToInt32(dr["SOId"]),
                            SONo = Convert.ToString(dr["SONo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            City = Convert.ToString(dr["City"]),
                            Priority = Convert.ToDecimal(dr["Priority"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sOProductViewModellst;
        }

        public List<DispatchPlanViewModel> GetDispatchPlanList(string dispatchPlanNo, string customerName, int companyBranchId, string fromDate, string toDate, string approvalStatus)
        {
            List<DispatchPlanViewModel> dispatchPlanViewModels = new List<DispatchPlanViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtdispatchPlans = sqlDbInterface.GetDispatchPlanList(dispatchPlanNo, customerName, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), approvalStatus);
                if (dtdispatchPlans != null && dtdispatchPlans.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtdispatchPlans.Rows)
                    {
                        dispatchPlanViewModels.Add(new DispatchPlanViewModel
                        {
                            DispatchPlanID = Convert.ToInt32(dr["DispatchPlanID"]),
                            DispatchPlanNo = Convert.ToString(dr["DispatchPlanNo"]),
                            DispatchPlanDate = Convert.ToString(dr["DispatchPlanDate"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            CustomerID = Convert.ToInt32(dr["CustomerId"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dispatchPlanViewModels;
        }
        public List<DispatchPlanViewModel> GetApproveDispatchPlanList(string dispatchPlanNo, string customerName, int companyBranchId, string fromDate, string toDate, string approvalStatus)
        {
            List<DispatchPlanViewModel> dispatchPlanViewModels = new List<DispatchPlanViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtdispatchPlans = sqlDbInterface.GetApproveDispatchPlanList(dispatchPlanNo, customerName, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), approvalStatus);
                if (dtdispatchPlans != null && dtdispatchPlans.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtdispatchPlans.Rows)
                    {
                        dispatchPlanViewModels.Add(new DispatchPlanViewModel
                        {
                            DispatchPlanID = Convert.ToInt32(dr["DispatchPlanID"]),
                            DispatchPlanNo = Convert.ToString(dr["DispatchPlanNo"]),
                            DispatchPlanDate = Convert.ToString(dr["DispatchPlanDate"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            CustomerID = Convert.ToInt32(dr["CustomerId"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dispatchPlanViewModels;
        }

        public List<DispatchPlanViewModel> GetDispatchPlanListForDispatch(string dispatchPlanNo, string customerName, int companyBranchId, string fromDate, string toDate, string approvalStatus)
        {
            List<DispatchPlanViewModel> dispatchPlanViewModels = new List<DispatchPlanViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtdispatchPlans = sqlDbInterface.GetDispatchPlanListForDispatch(dispatchPlanNo, customerName, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), approvalStatus);
                if (dtdispatchPlans != null && dtdispatchPlans.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtdispatchPlans.Rows)
                    {
                        dispatchPlanViewModels.Add(new DispatchPlanViewModel
                        {
                            DispatchPlanID = Convert.ToInt32(dr["DispatchPlanID"]),
                            DispatchPlanNo = Convert.ToString(dr["DispatchPlanNo"]),
                            DispatchPlanDate = Convert.ToString(dr["DispatchPlanDate"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
                            CustomerID = Convert.ToInt32(dr["CustomerId"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dispatchPlanViewModels;
        }
    }
}
