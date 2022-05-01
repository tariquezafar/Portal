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
   public class StoreRequisitionBL
    {
        DBInterface dbInterface;
        public StoreRequisitionBL()
        {
            dbInterface = new DBInterface();
        }

        #region StoreRequisition
        public ResponseOut AddEditStoreRequisition(StoreRequisitionViewModel storeRequisitionViewModel, List<StoreRequisitionProductDetailViewModel> storeRequisitionProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                StoreRequisition storeRequisition = new StoreRequisition
                {
                    RequisitionId = storeRequisitionViewModel.RequisitionId,
                    RequisitionDate =Convert.ToDateTime(storeRequisitionViewModel.RequisitionDate),
                    RequisitionType= storeRequisitionViewModel.RequisitionType,
                    CompanyBranchId = storeRequisitionViewModel.CompanyBranchId,
                    LocationId = storeRequisitionViewModel.LocationId,
                    RequisitionByUserId = storeRequisitionViewModel.RequisitionByUserId,
                    CustomerId = storeRequisitionViewModel.CustomerId,
                    CustomerBranchId =storeRequisitionViewModel.CustomerBranchId,
                    Remarks1= storeRequisitionViewModel.Remarks1,
                    Remarks2 = storeRequisitionViewModel.Remarks2,
                    FinYearId = storeRequisitionViewModel.FinYearId,
                    CompanyId = storeRequisitionViewModel.CompanyId,
                    CreatedBy= storeRequisitionViewModel.CreatedBy,
                    RequisitionStatus= storeRequisitionViewModel.RequisitionStatus,
                    WorkOrderId = storeRequisitionViewModel.WorkOrderId,
                    WorkOrderNo = storeRequisitionViewModel.WorkOrderNo
                };

               
                List<StoreRequisitionProductDetail> requisitionProductList = new List<StoreRequisitionProductDetail>();
                if (storeRequisitionProducts != null && storeRequisitionProducts.Count > 0)
                {
                    foreach (StoreRequisitionProductDetailViewModel item in storeRequisitionProducts)
                    {
                        requisitionProductList.Add(new StoreRequisitionProductDetail {
                            ProductId = item.ProductId,
                            ProductShortDesc= item.ProductShortDesc,
                            Quantity=item.Quantity,
                            IssuedQuantity=item.IssuedQuantity

                        });
                       
                    }
                }
              responseOut = sqlDbInterface.AddEditStoreRequisition(storeRequisition, requisitionProductList);
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

        public List<StoreRequisitionProductDetailViewModel> GetStoreRequisitionProductList(long requisitionId)
        {
            List<StoreRequisitionProductDetailViewModel> requisitionProducts = new List<StoreRequisitionProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisitionProducts = sqlDbInterface.GetStoreRequisitionProductList(requisitionId);
                if (dtRequisitionProducts != null && dtRequisitionProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisitionProducts.Rows)
                    {
                        requisitionProducts.Add(new StoreRequisitionProductDetailViewModel
                        {
                            RequisitionProductDetailId = Convert.ToInt32(dr["RequisitionProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            IssuedQuantity = Convert.ToDecimal(dr["IssuedQuantity"]),
                            Price = Convert.ToDecimal(dr["PurchasePrice"]),
                            TotalPrice = Convert.ToDecimal(dr["TotalPrice"])
                            
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return requisitionProducts;
        }

        public StoreRequisitionViewModel GetStoreRequisitionDetail(long requisitionId = 0)
        {
            StoreRequisitionViewModel storeRequisitions = new StoreRequisitionViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtStoreRequisitions = sqlDbInterface.GetStoreRequisitionDetail(requisitionId);
                if (dtStoreRequisitions != null && dtStoreRequisitions.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtStoreRequisitions.Rows)
                    {
                        storeRequisitions = new StoreRequisitionViewModel
                        {
                            RequisitionId = Convert.ToInt32(dr["RequisitionId"]),
                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            RequisitionDate = Convert.ToString(dr["RequisitionDate"]),
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo  = Convert.ToString(dr["WorkOrderNo"]),
                            WorkOrderDate= Convert.ToString(dr["WorkOrderDate"]),
                            RequisitionByUserId = Convert.ToInt32(dr["RequisitionByUserId"]),
                            FullName = Convert.ToString(dr["FullName"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            RequisitionType = Convert.ToString(dr["RequisitionType"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerBranchId = Convert.ToInt32(dr["CustomerBranchId"]),
                            RequisitionStatus = Convert.ToString(dr["RequisitionStatus"]),
                            LocationId = Convert.ToInt32(dr["LocationId"]),
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedByName"])) ? "" : Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedDate"])) ? "" : Convert.ToString(dr["ModifiedDate"]),

                            ApprovalStatus = string.IsNullOrEmpty(Convert.ToString(dr["ApprovalStatus"])) ? "" : Convert.ToString(dr["ApprovalStatus"]),

                            ApprovedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["ApprovedByName"])) ? "" : Convert.ToString(dr["ApprovedByName"]),
                            ApprovedDate = string.IsNullOrEmpty(Convert.ToString(dr["ApprovedDate"])) ? "" : Convert.ToString(dr["ApprovedDate"]),

                            RejectedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["RejectedByName"])) ? "" : Convert.ToString(dr["RejectedByName"]),
                            RejectedDate = string.IsNullOrEmpty(Convert.ToString(dr["RejectedDate"])) ? "" : Convert.ToString(dr["RejectedDate"]),
                            RejectedReason = string.IsNullOrEmpty(Convert.ToString(dr["RejectedReason"])) ? "" : Convert.ToString(dr["RejectedReason"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return storeRequisitions;
        }

        public List<WorkOrderViewModel> GetRequisitionWorkOrderList(string workOrderNo, int companyBranchId, string fromDate, string toDate,int companyId, string displayType = "", string approvalStatus = "")
        {
            List<WorkOrderViewModel> workOrders = new List<WorkOrderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetRequisitionWorkOrderList(workOrderNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        workOrders.Add(new WorkOrderViewModel
                        {
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            WorkOrderDate = Convert.ToString(dr["WorkOrderDate"]),
                            TargetFromDate = Convert.ToString(dr["TargetFromDate"]),
                            TargetToDate = Convert.ToString(dr["TargetToDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            WorkOrderStatus = Convert.ToString(dr["WorkOrderStatus"]),
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
            return workOrders;
        }

        public List<StoreRequisitionViewModel> GetStoreRequisitionList(string requisitionNo, string workOrderNo, string requisitionType, string customerName, int companyBranchId, DateTime fromDate, DateTime toDate, int companyId, string displayType = "", string approvalStatus = "")
        {
            List<StoreRequisitionViewModel> requisitions = new List<StoreRequisitionViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisitions = sqlDbInterface.GetStoreRequisitionList(requisitionNo, workOrderNo, requisitionType, customerName, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
                if (dtRequisitions != null && dtRequisitions.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisitions.Rows)
                    {
                        requisitions.Add(new StoreRequisitionViewModel
                        {
                            RequisitionId = Convert.ToInt32(dr["RequisitionId"]),
                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            RequisitionDate = Convert.ToString(dr["RequisitionDate"]),
                            RequisitionType= Convert.ToString(dr["RequisitionType"]),
                            LocationName = Convert.ToString(dr["LocationName"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerId = Convert.ToInt32(dr["CustomerID"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            WorkOrderDate=Convert.ToString(dr["WorkOrderDate"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            RequisitionStatus = string.IsNullOrEmpty(Convert.ToString(dr["RequisitionStatus"]))?"":Convert.ToString(dr["RequisitionStatus"]),
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
            return requisitions;
        }

        public List<StoreRequisitionProductDetailViewModel> GetWorkOrderBOMProductList(long workOrderId)
        {
            List<StoreRequisitionProductDetailViewModel> requisitionProducts = new List<StoreRequisitionProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisitionProducts = sqlDbInterface.GetWorkOrderBOMProductList(workOrderId);
                if (dtRequisitionProducts != null && dtRequisitionProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisitionProducts.Rows)
                    {
                        requisitionProducts.Add(new StoreRequisitionProductDetailViewModel
                        {
                            RequisitionProductDetailId = 0,
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            IssuedQuantity = Convert.ToDecimal(dr["IssuedQuantity"])


                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return requisitionProducts;
        }
      
        public DataTable GetStoreRequisitionDataTable(long requisitionId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtStoreRequisition = new DataTable();
            try
            {
                dtStoreRequisition = sqlDbInterface.GetStoreRequisitionDetail(requisitionId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtStoreRequisition;
        }

        public DataTable GetStoreRequisitionProductListDataTable(long requisitionId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetStoreRequisitionProductList(requisitionId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }
        #endregion

        #region  Approval StoreRequisition

        public ResponseOut ApproveRejectStoreRequisition(StoreRequisitionViewModel storeRequisitionViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            
            try
            {
                StoreRequisition storeRequisition = new StoreRequisition
                {
                    RequisitionId = storeRequisitionViewModel.RequisitionId,
                    RejectedReason = storeRequisitionViewModel.RejectedReason,
                    ApprovedBy = storeRequisitionViewModel.ApprovedBy,
                    ApprovalStatus = storeRequisitionViewModel.ApprovalStatus
                    
                };

                responseOut = dbInterface.ApproveRejectStoreRequisition(storeRequisition);
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

        public List<StoreRequisitionViewModel> GetStoreRequisitionApprovelList(string requisitionNo, string workOrderNo, string requisitionType, string customerName, int companyBranchId, DateTime fromDate, DateTime toDate, int companyId, string displayType = "", string approvalStatus = "")
        {
            List<StoreRequisitionViewModel> requisitions = new List<StoreRequisitionViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisitions = sqlDbInterface.GetStoreRequisitionApprovelList(requisitionNo, workOrderNo, requisitionType, customerName, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
                if (dtRequisitions != null && dtRequisitions.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisitions.Rows)
                    {
                        requisitions.Add(new StoreRequisitionViewModel
                        {
                            RequisitionId = Convert.ToInt32(dr["RequisitionId"]),
                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            RequisitionDate = Convert.ToString(dr["RequisitionDate"]),
                            RequisitionType = Convert.ToString(dr["RequisitionType"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerId = Convert.ToInt32(dr["CustomerID"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            ApprovedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["ApprovedByName"]))?"": Convert.ToString(dr["ApprovedByName"]),
                            ApprovedDate = string.IsNullOrEmpty(Convert.ToString(dr["ApprovedDate"]))?"": Convert.ToString(dr["ApprovedDate"]),
                            ApprovalStatus = string.IsNullOrEmpty(Convert.ToString(dr["ApprovalStatus"])) ? "" : Convert.ToString(dr["ApprovalStatus"]),
                            RejectedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["RejectedByName"]))?"": Convert.ToString(dr["RejectedByName"]),
                            RejectedDate = string.IsNullOrEmpty(Convert.ToString(dr["RejectedDate"]))?"": Convert.ToString(dr["RejectedDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return requisitions;
        }


        #endregion StoreRequisition

        #region Approval Update StoreRequisition

        public ResponseOut ApprovalStoreRequisitionUpdate(StoreRequisitionViewModel storeRequisitionViewModel)
        {
            ResponseOut responseOut = new ResponseOut();

            try
            {
                StoreRequisition storeRequisition = new StoreRequisition
                {
                    RequisitionId = storeRequisitionViewModel.RequisitionId,
                    RejectedReason = storeRequisitionViewModel.RejectedReason,
                    ApprovedBy = storeRequisitionViewModel.ApprovedBy,
                    ApprovalStatus = storeRequisitionViewModel.ApprovalStatus

                };

                responseOut = dbInterface.ApproveRejectStoreRequisition(storeRequisition);
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

        public List<StoreRequisitionViewModel> GetStoreRequisitionApprovalUpdateList(string requisitionNo, string workOrderNo, string requisitionType, string customerName, int companyBranchId, DateTime fromDate, DateTime toDate, int companyId, string displayType = "", string approvalStatus = "Approved")
        {
            List<StoreRequisitionViewModel> requisitions = new List<StoreRequisitionViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisitions = sqlDbInterface.GetStoreRequisitionApprovelUpdateList(requisitionNo, workOrderNo, requisitionType, customerName, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
                if (dtRequisitions != null && dtRequisitions.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisitions.Rows)
                    {
                        requisitions.Add(new StoreRequisitionViewModel
                        {
                            RequisitionId = Convert.ToInt32(dr["RequisitionId"]),
                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            RequisitionDate = Convert.ToString(dr["RequisitionDate"]),
                            RequisitionType = Convert.ToString(dr["RequisitionType"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerId = Convert.ToInt32(dr["CustomerID"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            ApprovedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["ApprovedByName"])) ? "" : Convert.ToString(dr["ApprovedByName"]),
                            ApprovedDate = string.IsNullOrEmpty(Convert.ToString(dr["ApprovedDate"])) ? "" : Convert.ToString(dr["ApprovedDate"]),
                            ApprovalStatus = string.IsNullOrEmpty(Convert.ToString(dr["ApprovalStatus"])) ? "" : Convert.ToString(dr["ApprovalStatus"]),
                            RejectedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["RejectedByName"])) ? "" : Convert.ToString(dr["RejectedByName"]),
                            RejectedDate = string.IsNullOrEmpty(Convert.ToString(dr["RejectedDate"])) ? "" : Convert.ToString(dr["RejectedDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return requisitions;
        }


        public List<StoreRequisitionProductDetailViewModel> GetStoreRequisitionUpdateProductUpdateList(long requisitionId)
        {
            List<StoreRequisitionProductDetailViewModel> requisitionProducts = new List<StoreRequisitionProductDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisitionProducts = sqlDbInterface.GetStoreRequisitionProductUpdateList(requisitionId);
                if (dtRequisitionProducts != null && dtRequisitionProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisitionProducts.Rows)
                    {
                        requisitionProducts.Add(new StoreRequisitionProductDetailViewModel
                        {
                            RequisitionProductDetailId = Convert.ToInt32(dr["RequisitionProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            IssuedQuantity = Convert.ToDecimal(dr["IssuedQuantity"]),
                            Price = Convert.ToDecimal(dr["PurchasePrice"]),
                            TotalPrice = Convert.ToDecimal(dr["TotalPrice"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return requisitionProducts;
        }


        public ResponseOut ApproveUpdateStoreRequisition(StoreRequisitionViewModel storeRequisitionViewModel, List<StoreRequisitionProductDetailViewModel> storeRequisitionProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                StoreRequisition storeRequisition = new StoreRequisition
                {
                    RequisitionId = storeRequisitionViewModel.RequisitionId,
                    RequisitionDate = Convert.ToDateTime(storeRequisitionViewModel.RequisitionDate),
                    RequisitionType = storeRequisitionViewModel.RequisitionType,
                    CompanyBranchId = storeRequisitionViewModel.CompanyBranchId,
                    LocationId = storeRequisitionViewModel.LocationId,
                    RequisitionByUserId = storeRequisitionViewModel.RequisitionByUserId,
                    CustomerId = storeRequisitionViewModel.CustomerId,
                    CustomerBranchId = storeRequisitionViewModel.CustomerBranchId,
                    Remarks1 = storeRequisitionViewModel.Remarks1,
                    Remarks2 = storeRequisitionViewModel.Remarks2,
                    FinYearId = storeRequisitionViewModel.FinYearId,
                    CompanyId = storeRequisitionViewModel.CompanyId,
                    CreatedBy = storeRequisitionViewModel.CreatedBy,
                    RequisitionStatus = storeRequisitionViewModel.RequisitionStatus,
                    WorkOrderId = storeRequisitionViewModel.WorkOrderId,
                    WorkOrderNo = storeRequisitionViewModel.WorkOrderNo,
                    RejectionStatus= storeRequisitionViewModel.RejectionStatus
                };


                List<StoreRequisitionProductDetail> requisitionProductList = new List<StoreRequisitionProductDetail>();
                if (storeRequisitionProducts != null && storeRequisitionProducts.Count > 0)
                {
                    foreach (StoreRequisitionProductDetailViewModel item in storeRequisitionProducts)
                    {
                        requisitionProductList.Add(new StoreRequisitionProductDetail
                        {
                            ProductId = item.ProductId,
                            ProductShortDesc = item.ProductShortDesc,
                            Quantity = item.Quantity,
                            IssuedQuantity = item.IssuedQuantity

                        });

                    }
                }
                responseOut = sqlDbInterface.AddEditStoreRequisitionUpdate(storeRequisition, requisitionProductList);
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
        #endregion
    }
}
