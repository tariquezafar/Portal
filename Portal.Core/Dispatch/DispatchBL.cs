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
    public class DispatchBL
    {
        DBInterface dbInterface;
        public DispatchBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut AddEditDispatch(DispatchViewModel dispatchViewModel, List<DispatchProductDetailViewModel> dispatchProductDetailViewModels)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                Dispatch dispatch = new Dispatch
                {
                    DispatchID = dispatchViewModel.DispatchID,
                    DispatchDate = Convert.ToDateTime(dispatchViewModel.DispatchDate),
                    DispatchPlanID = dispatchViewModel.DispatchPlanID,
                    CompanyBranchID = dispatchViewModel.CompanyBranchID,
                    CreatedBy = dispatchViewModel.CreatedBy,
                    ApprovalStatus = dispatchViewModel.ApprovalStatus
                };

                List<DispatchProductDetail> dispatchPlanProductDetails = new List<DispatchProductDetail>();
                if (dispatchProductDetailViewModels != null && dispatchProductDetailViewModels.Count > 0)
                {
                    foreach (DispatchProductDetailViewModel item in dispatchProductDetailViewModels)
                    {
                        dispatchPlanProductDetails.Add(new DispatchProductDetail
                        {
                            SOId = item.SOId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Priority = item.Priority
                        });
                    }
                }
                responseOut = sqlDbInterface.AddEditDispatch(dispatch, dispatchPlanProductDetails);

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

        public DispatchViewModel GetDispatchDetail(int dispatchID = 0)
        {
            DispatchViewModel dispatch = new DispatchViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDispatchPlanProducts = sqlDbInterface.GetDispatchDetail(dispatchID);
                if (dtDispatchPlanProducts != null && dtDispatchPlanProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDispatchPlanProducts.Rows)
                    {
                        dispatch = new DispatchViewModel
                        {
                            DispatchID = Convert.ToInt32(dr["DispatchID"]),
                            DispatchDate = Convert.ToString(dr["DispatchDate"]),
                            DispatchNo = Convert.ToString(dr["DispatchNo"]),
                            DispatchPlanID = Convert.ToInt32(dr["DispatchPlanID"]),
                            DispatchPlanNo = Convert.ToString(dr["DispatchPlanNo"]),
                            DispatchPlanDate = Convert.ToString(dr["DispatchPlanDate"]),
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
            return dispatch;
        }

        public List<DispatchProductDetailViewModel> GetDispatchProductList(int dispatchID)
        {
            List<DispatchProductDetailViewModel> dispatchProductDetailViewModels = new List<DispatchProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetDispatchProductList(dispatchID);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        dispatchProductDetailViewModels.Add(new DispatchProductDetailViewModel
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
            return dispatchProductDetailViewModels;
        }

        public List<DispatchViewModel> GetDispatchList(string dispatchNo, string dispatchPlanNo, int companyBranchId, string fromDate, string toDate, string approvalStatus)
        {
            List<DispatchViewModel> dispatchViewModels = new List<DispatchViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtdispatchPlans = sqlDbInterface.GetDispatchList(dispatchNo, dispatchPlanNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), approvalStatus);
                if (dtdispatchPlans != null && dtdispatchPlans.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtdispatchPlans.Rows)
                    {
                        dispatchViewModels.Add(new DispatchViewModel
                        {
                            DispatchID = Convert.ToInt32(dr["DispatchID"]),
                            DispatchNo = Convert.ToString(dr["DispatchNo"]),
                            DispatchPlanNo = Convert.ToString(dr["DispatchPlanNo"]),
                            DispatchDate = Convert.ToString(dr["DispatchDate"]),
                            BranchName = Convert.ToString(dr["BranchName"]),
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
            return dispatchViewModels;
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
    }
}
