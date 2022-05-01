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
    public class AssemblingProcessBL
    {
        DBInterface dbInterface;
        public AssemblingProcessBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditAssemblingProcess(AssemblingProcessViewModel assemblingProcessViewModel, List<AssemblingProcessProductViewModel> assemblingProcessProducts, List<AssemblingProcessChasisSerialViewModel> assemblingProcessChasisSerialProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                AssemblingProcess assemblingProcess = new AssemblingProcess
                {
                    AssemblingProcessId= assemblingProcessViewModel.AssemblingProcessId,
                    AssemblingProcessDate = Convert.ToDateTime(assemblingProcessViewModel.AssemblingProcessDate),
                    PaintProcessId = assemblingProcessViewModel.PaintProcessId,
                    PaintProcessNo = assemblingProcessViewModel.PaintProcessNo,
                    WorkOrderId = assemblingProcessViewModel.WorkOrderId,
                    WorkOrderNo = assemblingProcessViewModel.WorkOrderNo,                    
                    CompanyId = assemblingProcessViewModel.CompanyId,
                    CompanyBranchId = assemblingProcessViewModel.CompanyBranchId,
                    Remarks1 = assemblingProcessViewModel.Remarks1,
                    Remarks2 = assemblingProcessViewModel.Remarks2,
                    CreatedBy = assemblingProcessViewModel.CreatedBy,
                    AssemblingProcessStatus = assemblingProcessViewModel.AssemblingProcessStatus
                };
                List<AssemblingProcessProductDetail> assemblingProcessProductList = new List<AssemblingProcessProductDetail>();
                if(assemblingProcessProducts != null && assemblingProcessProducts.Count>0)
                {
                    foreach(AssemblingProcessProductViewModel item in assemblingProcessProducts)
                    {
                        assemblingProcessProductList.Add(new AssemblingProcessProductDetail
                        {
                            ProductId=item.ProductId,
                            Quantity=item.Quantity
                        });
                    }
                }

                List<AssemblingProcessChasisDetail> assemblingProcessChasisSerialProductList = new List<AssemblingProcessChasisDetail>();
                if (assemblingProcessChasisSerialProducts != null && assemblingProcessChasisSerialProducts.Count > 0)
                {
                    foreach (AssemblingProcessChasisSerialViewModel item in assemblingProcessChasisSerialProducts)
                    {
                        assemblingProcessChasisSerialProductList.Add(new AssemblingProcessChasisDetail
                        {
                            ProductID = item.ProductId,
                            ChasisSerialNo = item.ChasisSerialNo,
                            MotorNo = item.MotorNo                           
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditAssemblingProcess(assemblingProcess, assemblingProcessProductList, assemblingProcessChasisSerialProductList);
             
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
    
       
        public List<AssemblingProcessViewModel> GetAssemblingProcessList(string assemblingProcessNo, string workOrderNo, int companyBranchId,string fromDate, string toDate, int companyId, string assemblingProcessStatus = "")
        {
            List<AssemblingProcessViewModel> assemblingProcessViewModel = new List<AssemblingProcessViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetAssemblingProcessList(assemblingProcessNo,workOrderNo, companyBranchId,  Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate),companyId, assemblingProcessStatus);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        assemblingProcessViewModel.Add(new AssemblingProcessViewModel
                        {
                            AssemblingProcessId = Convert.ToInt32(dr["AssemblingProcessId"]),
                            AssemblingProcessNo = Convert.ToString(dr["AssemblingProcessNo"]),
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            PaintProcessNo = Convert.ToString(dr["PaintProcessNo"]),
                            AssemblingProcessDate = Convert.ToString(dr["AssemblingProcessDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            AssemblingProcessStatus = Convert.ToString(dr["AssemblingProcessStatus"]),                          
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
            return assemblingProcessViewModel;
        }
        public AssemblingProcessViewModel GetAssemblingProcessDetail(long assemblingProcessId = 0)
        {
            AssemblingProcessViewModel assemblingProcessViewModel = new AssemblingProcessViewModel();           
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtAssemblingProcess = sqlDbInterface.GetAssemblingProcessDetail(assemblingProcessId);
                if (dtAssemblingProcess != null && dtAssemblingProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAssemblingProcess.Rows)
                    {
                        assemblingProcessViewModel = new AssemblingProcessViewModel
                        {
                            AssemblingProcessId = Convert.ToInt32(dr["AssemblingProcessId"]),
                            AssemblingProcessNo = Convert.ToString(dr["AssemblingProcessNo"]),
                            PaintProcessId = Convert.ToInt32(dr["PaintProcessId"]),
                            PaintProcessNo = Convert.ToString(dr["PaintProcessNo"]),
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            AssemblingProcessDate = Convert.ToString(dr["AssemblingProcessDate"]),                           
                            CompanyBranchId =Convert.ToInt32(dr["CompanyBranchId"]),
                            AssemblingProcessStatus = Convert.ToString(dr["AssemblingProcessStatus"]),
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            TotalQuantity = Convert.ToDecimal(dr["TotalQuantity"]),
                            PaintProcessQuantity = Convert.ToDecimal(dr["PaintProcessQuantity"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return assemblingProcessViewModel;
        }
    
 
        public List<AssemblingProcessProductViewModel> GetAssemblingProcessProductList(long assemblingProcessId)
        {
            List<AssemblingProcessProductViewModel> assemblingProcessProducts = new List<AssemblingProcessProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtAssemblingProcess = sqlDbInterface.GetAssemblingProcessProductList(assemblingProcessId);
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
                            TotalPaintQuantity = Convert.ToDecimal(dr["TotalPaintQuantity"]),
                            TotalRecivedAssembledQuantity = Convert.ToDecimal(dr["TotalRecivedAssembledQuantity"]),
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

        public DataTable GetAssemblingProcessProductListDataTable(long assemblingProcessId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetAssemblingProcessProductListPrint(assemblingProcessId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public DataTable GetAssemblingProcessDataTable(long assemblingProcessId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtAssemblingProcess = new DataTable();
            try
            {
                dtAssemblingProcess = sqlDbInterface.GetAssemblingProcessDetail(assemblingProcessId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtAssemblingProcess;
        }

        public List<WorkOrderViewModel> GetAssemblingProcessWorkOrderList(string workOrderNo, int companyBranchId, string fromDate, string toDate, int companyId, string displayType = "", string approvalStatus = "")
        {
            List<WorkOrderViewModel> workOrders = new List<WorkOrderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetAssemblingProcessWorkOrderList(workOrderNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
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


        public List<PaintProcessViewModel> GetAssemblingProcessPaintProcessList(string paintProcessNo, int companyBranchId, string fromDate, string toDate, int companyId, string displayType = "", string approvalStatus = "")
        {
            List<PaintProcessViewModel> paintProcess = new List<PaintProcessViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetAssemblingProcessPaintProcessList(paintProcessNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        paintProcess.Add(new PaintProcessViewModel
                        {
                            PaintProcessId = Convert.ToInt32(dr["PaintProcessId"]),
                            PaintProcessNo = Convert.ToString(dr["PaintProcessNo"]),
                            PaintProcessDate= Convert.ToString(dr["PaintProcessDate"]),
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),                           
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            PaintProcessStatus = Convert.ToString(dr["PaintProcessStatus"]),
                            PaintProcessQuantity = Convert.ToInt32(dr["PaintProcessQuantity"]),
                            Quantity = Convert.ToInt32(dr["WorkOrderQuantity"]),
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
        public decimal GetAssemblingProcessProducedQuantityAgainstWorkOrder(long workOrderId)
        {
            decimal productQuantity = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                productQuantity = sqldbinterface.GetAssemblingProducedQuantityAgainstWorkOrder(workOrderId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productQuantity;
        }
        public List<ProductSerialDetailViewModel> GetProductSerialProduct()
        {
            List<ProductSerialDetailViewModel> ProductSerialDetailProducts = new List<ProductSerialDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPaintProcess = sqlDbInterface.GetPaintProcessProductSerialProduct();
                if (dtPaintProcess != null && dtPaintProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPaintProcess.Rows)
                    {
                        ProductSerialDetailProducts.Add(new ProductSerialDetailViewModel
                        {
                            ProductId=Convert.ToInt32(dr["ProductID"]),
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
        public List<AssemblingProcessChasisSerialViewModel> GetAssemblingProcessProductSerialProductList(long assemblingProcessId)
        {
            List<AssemblingProcessChasisSerialViewModel> assemblingProcessChasisSerialProducts = new List<AssemblingProcessChasisSerialViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPaintProcess = sqlDbInterface.GetAssemblingProcessProductSerialProductList(assemblingProcessId);
                if (dtPaintProcess != null && dtPaintProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPaintProcess.Rows)
                    {
                        assemblingProcessChasisSerialProducts.Add(new AssemblingProcessChasisSerialViewModel
                        {
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            MatchProductId= Convert.ToInt32(dr["MatchProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ChasisSerialNo = Convert.ToString(dr["ChasisSerialNo"]),
                            MotorNo = Convert.ToString(dr["MotorNo"]),
                            AssembledFlag = Convert.ToString(dr["AssemblingStatus"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return assemblingProcessChasisSerialProducts;
        }

        public List<PaintProcessProductViewModel> GetPaintProcessProductList(long paintProcessId)
        {
            List<PaintProcessProductViewModel> paintProcessProducts = new List<PaintProcessProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPaintProcess = sqlDbInterface.GetAssemblingPaintProcessProductList(paintProcessId);
                if (dtPaintProcess != null && dtPaintProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPaintProcess.Rows)
                    {
                        paintProcessProducts.Add(new PaintProcessProductViewModel
                        {
                            PaintProcessDetailId = Convert.ToInt32(dr["PaintProcessDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            TotalPaintQuantity = Convert.ToDecimal(dr["TotalPaintQuantity"]),
                            TotalRecivedAssembledQuantity = Convert.ToDecimal(dr["TotalRecivedAssembledQuantity"]),
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
            return paintProcessProducts;
        }

        public DataTable GetAssemblingProcessChasisPrint(long assemblingProcessId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetAssemblingProcessChasisPrint(assemblingProcessId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }
        public ResponseOut CancelAP(AssemblingProcessViewModel assemblingProcessViewModel)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                AssemblingProcess assemblingProcess = new AssemblingProcess
                {
                    AssemblingProcessId = assemblingProcessViewModel.AssemblingProcessId,
                    AssemblingProcessNo = assemblingProcessViewModel.AssemblingProcessNo,
                    CancelStatus = "Cancel",
                    AssemblingProcessStatus = "Cancelled",
                    CreatedBy = assemblingProcessViewModel.CreatedBy,
                    CancelReason = assemblingProcessViewModel.CancelReason,
                    CompanyId = assemblingProcessViewModel.CompanyId,
                    CompanyBranchId = assemblingProcessViewModel.CompanyBranchId
                };
                responseOut = sqlDbInterface.CancelAP(assemblingProcess);
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
