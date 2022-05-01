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
    public class FinishedGoodProcessBL
    {
        DBInterface dbInterface;
        public FinishedGoodProcessBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditFinishedGoodProcess(FinishedGoodProcessViewModel finishedGoodProcessViewModel, List<FinishedGoodProcessProductViewModel> finishedGoodProcessProducts,List<FinishedGoodProcessChasisSerialViewModel> finishedGoodProcessChasisSerialList)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                FinishedGoodProcess finishedGoodProcess = new FinishedGoodProcess
                {
                    FinishedGoodProcessId = finishedGoodProcessViewModel.FinishedGoodProcessId,
                    FinishedGoodProcessDate = Convert.ToDateTime(finishedGoodProcessViewModel.FinishedGoodProcessDate),
                    AssemblingProcessId = finishedGoodProcessViewModel.AssemblingProcessId,
                    AssemblingProcessNo = finishedGoodProcessViewModel.AssemblingProcessNo,
                    WorkOrderId = finishedGoodProcessViewModel.WorkOrderId,
                    WorkOrderNo = finishedGoodProcessViewModel.WorkOrderNo,                    
                    CompanyId = finishedGoodProcessViewModel.CompanyId,
                    CompanyBranchId = finishedGoodProcessViewModel.CompanyBranchId,
                    Remarks1 = finishedGoodProcessViewModel.Remarks1,
                    Remarks2 = finishedGoodProcessViewModel.Remarks2,
                    CreatedBy = finishedGoodProcessViewModel.CreatedBy,
                    FinishedGoodProcessStatus = finishedGoodProcessViewModel.FinishedGoodProcessStatus
                };
                List<FinishedGoodProcessProductDetail> finishedGoodProcessProductList = new List<FinishedGoodProcessProductDetail>();
                if(finishedGoodProcessProducts != null && finishedGoodProcessProducts.Count>0)
                {
                    foreach(FinishedGoodProcessProductViewModel item in finishedGoodProcessProducts)
                    {
                        finishedGoodProcessProductList.Add(new FinishedGoodProcessProductDetail
                        {
                            ProductId=item.ProductId,
                            Quantity=item.Quantity
                        });
                    }
                }
                List<FinishedGoodProcessChasisDetail> finishedGoodProcessChasisSerialProductList = new List<FinishedGoodProcessChasisDetail>();
                if (finishedGoodProcessChasisSerialList != null && finishedGoodProcessChasisSerialList.Count > 0)
                {
                    foreach (FinishedGoodProcessChasisSerialViewModel item in finishedGoodProcessChasisSerialList)
                    {
                        finishedGoodProcessChasisSerialProductList.Add(new FinishedGoodProcessChasisDetail
                        {
                            ProductID = item.ProductId,
                            ChasisSerialNo = item.ChasisSerialNo,
                            MotorNo = item.MotorNo
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditFinishedGoodProcess(finishedGoodProcess, finishedGoodProcessProductList, finishedGoodProcessChasisSerialProductList);
             
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
    
       
        public List<FinishedGoodProcessViewModel> GetFinishedGoodProcessList(string finishedGoodProcessNo, string workOrderNo, int companyBranchId,string fromDate, string toDate, int companyId, string finishedGoodProcessStatus = "")
        {
            List<FinishedGoodProcessViewModel> finishedGoodProcessViewModel = new List<FinishedGoodProcessViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtFinishedGoods = sqlDbInterface.GetFinishedGoodProcessList(finishedGoodProcessNo, workOrderNo, companyBranchId,  Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate),companyId, finishedGoodProcessStatus);
                if (dtFinishedGoods != null && dtFinishedGoods.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFinishedGoods.Rows)
                    {
                        finishedGoodProcessViewModel.Add(new FinishedGoodProcessViewModel
                        {
                            FinishedGoodProcessId = Convert.ToInt32(dr["FinishedGoodProcessId"]),
                            FinishedGoodProcessNo = Convert.ToString(dr["FinishedGoodProcessNo"]),
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            FinishedGoodProcessDate = Convert.ToString(dr["FinishedGoodProcessDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            FinishedGoodProcessStatus = Convert.ToString(dr["FinishedGoodProcessStatus"]),                          
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
            return finishedGoodProcessViewModel;
        }
        public FinishedGoodProcessViewModel GetFinishedGoodProcessDetail(long finishedGoodProcessId = 0)
        {
            FinishedGoodProcessViewModel finishedGoodProcessViewModel = new FinishedGoodProcessViewModel();           
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtFinishedGoodProcess = sqlDbInterface.GetFinishedGoodProcessDetail(finishedGoodProcessId);
                if (dtFinishedGoodProcess != null && dtFinishedGoodProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFinishedGoodProcess.Rows)
                    {
                        finishedGoodProcessViewModel = new FinishedGoodProcessViewModel
                        {
                            FinishedGoodProcessId = Convert.ToInt32(dr["FinishedGoodProcessId"]),
                            FinishedGoodProcessNo = Convert.ToString(dr["FinishedGoodProcessNo"]),
                            AssemblingProcessId = Convert.ToInt32(dr["AssemblingProcessId"]),
                            AssemblingProcessNo = Convert.ToString(dr["AssemblingProcessNo"]),
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            FinishedGoodProcessDate = Convert.ToString(dr["FinishedGoodProcessDate"]),                           
                            CompanyBranchId =Convert.ToInt32(dr["CompanyBranchId"]),
                            FinishedGoodProcessStatus = Convert.ToString(dr["FinishedGoodProcessStatus"]),
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            TotalQuantity = Convert.ToDecimal(dr["TotalQuantity"])                            
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return finishedGoodProcessViewModel;
        }
    
 
        public List<FinishedGoodProcessProductViewModel> GetFinishedGoodProductList(long finishedGoodProcessId)
        {
            List<FinishedGoodProcessProductViewModel> finishedGoodProcessProducts = new List<FinishedGoodProcessProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtFinishedGoodProcess = sqlDbInterface.GetFinishedGoodProcessProductList(finishedGoodProcessId);
                if (dtFinishedGoodProcess != null && dtFinishedGoodProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFinishedGoodProcess.Rows)
                    {
                        finishedGoodProcessProducts.Add(new FinishedGoodProcessProductViewModel
                        {
                            FinishedGoodProcessDetailId = Convert.ToInt32(dr["FinishedGoodProcessDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            TotalAssembledQuantity = Convert.ToDecimal(dr["TotalAssembledQuantity"]),
                            TotalRecivedFinishedGoodQuantity = Convert.ToDecimal(dr["TotalRecivedFinishedGoodQuantity"]),
                            RecivedQuantity = Convert.ToDecimal(dr["RecivedQuantity"]),
                            PendingQuantity = Convert.ToDecimal(dr["PendingQunatity"]),
                            IsThirdPartyProduct = Convert.ToString(dr["IsThirdPartyProduct"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return finishedGoodProcessProducts;
        }

        public DataTable GetFinishedGoodProcessProductListDataTable(long finishedGoodProcessId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetFinishedGoodProcessProductListPrint(finishedGoodProcessId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }
        public DataTable GetFinishedGoodProcessChasiSerialProductListPrint(long finishedGoodProcessId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetFinishedGoodProcessChasiSerialProductListPrint(finishedGoodProcessId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public DataTable GetFinishedGoodProcessDataTable(long finishedGoodProcessId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtFinishedGoodProcess = new DataTable();
            try
            {
                dtFinishedGoodProcess = sqlDbInterface.GetFinishedGoodProcessDetail(finishedGoodProcessId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtFinishedGoodProcess;
        }

        public List<WorkOrderViewModel> GetFinishedGoodProcessWorkOrderList(string workOrderNo, int companyBranchId, string fromDate, string toDate, int companyId, string displayType = "", string approvalStatus = "")
        {
            List<WorkOrderViewModel> workOrders = new List<WorkOrderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetFinishedGoodProcessWorkOrderList(workOrderNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
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
                            Quantity = Convert.ToInt32(dr["Quantity"]),
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

        public List<AssemblingProcessViewModel> GetFinishedGoodAssemblingProcessList(string assemblingProcessNo, int companyBranchId, string fromDate, string toDate, int companyId, string displayType = "", string approvalStatus = "")
        {
            List<AssemblingProcessViewModel> paintProcess = new List<AssemblingProcessViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetFinishedGoodAssemblingProcessList(assemblingProcessNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        paintProcess.Add(new AssemblingProcessViewModel
                        {

                            AssemblingProcessId = Convert.ToInt32(dr["AssemblingProcessId"]),
                            AssemblingProcessNo = Convert.ToString(dr["AssemblingProcessNo"]),
                            AssemblingProcessDate = Convert.ToString(dr["AssemblingProcessDate"]),
                            PaintProcessId = Convert.ToInt32(dr["PaintProcessId"]),
                            PaintProcessNo = Convert.ToString(dr["PaintProcessNo"]),
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            AssemblingProcessStatus = Convert.ToString(dr["AssemblingProcessStatus"]),
                            PaintProcessQuantity = Convert.ToInt32(dr["PaintProcessQuantity"]),
                            TotalQuantity = Convert.ToInt32(dr["WorkOrderQuantity"]),
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
            return paintProcess;
        }
        public List<ProductSerialDetailViewModel> GetFinishedGoodProductSerialProduct()
        {
            List<ProductSerialDetailViewModel> ProductSerialDetailProducts = new List<ProductSerialDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPaintProcess = sqlDbInterface.GetFinishedGoodProductSerialProduct();
                if (dtPaintProcess != null && dtPaintProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPaintProcess.Rows)
                    {
                        ProductSerialDetailProducts.Add(new ProductSerialDetailViewModel
                        {
                            ProductId=Convert.ToInt32(dr["ProductId"]),
                            ChasisSerialNo = Convert.ToString(dr["ChasisSerialNo"]),
                            MotorNo = Convert.ToString(dr["MotorNo"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return ProductSerialDetailProducts;
        }
        public List<AssemblingProcessProductViewModel> GetFinishedGoodAssemblingProcessProductList(long assemblingProcessId)
        {
            List<AssemblingProcessProductViewModel> assemblingProcessProducts = new List<AssemblingProcessProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtAssemblingProcess = sqlDbInterface.GetFinishedGoodAssemblingProcessProductList(assemblingProcessId);
                if (dtAssemblingProcess != null && dtAssemblingProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAssemblingProcess.Rows)
                    {
                        assemblingProcessProducts.Add(new AssemblingProcessProductViewModel
                        {
                            AssemblingProcessDetailId = Convert.ToInt32(dr["AssemblingProcessDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            TotalAssembledQuantity = Convert.ToDecimal(dr["TotalAssembledQuantity"]),
                            TotalRecivedFinishedGoodQuantity = Convert.ToDecimal(dr["TotalRecivedFinishedGoodQuantity"]),
                            RecivedQuantity = Convert.ToDecimal(dr["RecivedQuantity"]),
                            PendingQuantity = Convert.ToDecimal(dr["PendingQunatity"]),
                            IsThirdPartyProduct = Convert.ToString(dr["IsThirdPartyProduct"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return assemblingProcessProducts;
        }

        public List<FinishedGoodProcessChasisSerialViewModel> GetFinishedGoodProductSerialProductList(long finishedGoodProcessId)
        {
            List<FinishedGoodProcessChasisSerialViewModel> finishedGoodProcessChasisSerialProducts = new List<FinishedGoodProcessChasisSerialViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPaintProcess = sqlDbInterface.GetFinishedGoodProductSerialProductList(finishedGoodProcessId);
                if (dtPaintProcess != null && dtPaintProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPaintProcess.Rows)
                    {
                        finishedGoodProcessChasisSerialProducts.Add(new FinishedGoodProcessChasisSerialViewModel
                        {
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            MatchProductId = Convert.ToInt32(dr["MatchProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ChasisSerialNo = Convert.ToString(dr["ChasisSerialNo"]),
                            MotorNo = Convert.ToString(dr["MotorNo"]),
                            FinishedGoodStatus = Convert.ToString(dr["FinishedGoodStatus"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return finishedGoodProcessChasisSerialProducts;
        }
        public ResponseOut CancelFGP(FinishedGoodProcessViewModel finishedGoodProcessViewModel, List<FinishedGoodProcessProductViewModel> finishedGoodProcessProducts)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                FinishedGoodProcess finishedGoodProcess = new FinishedGoodProcess
                {
                    FinishedGoodProcessId = finishedGoodProcessViewModel.FinishedGoodProcessId,
                    FinishedGoodProcessNo = finishedGoodProcessViewModel.FinishedGoodProcessNo,
                    CancelStatus = "Cancel",
                    FinishedGoodProcessStatus = "Cancelled",
                    CreatedBy = finishedGoodProcessViewModel.CreatedBy,
                    CancelReason = finishedGoodProcessViewModel.CancelReason,
                    CompanyId = finishedGoodProcessViewModel.CompanyId,
                    CompanyBranchId = finishedGoodProcessViewModel.CompanyBranchId
                };
                List<FinishedGoodProcessProductDetail> finishedGoodProcessProductList = new List<FinishedGoodProcessProductDetail>();
                if (finishedGoodProcessProducts != null && finishedGoodProcessProducts.Count > 0)
                {
                    foreach (FinishedGoodProcessProductViewModel item in finishedGoodProcessProducts)
                    {
                        finishedGoodProcessProductList.Add(new FinishedGoodProcessProductDetail
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity
                        });
                    }
                }
                responseOut = sqlDbInterface.CancelFGP(finishedGoodProcess, finishedGoodProcessProductList);
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
    }
}
