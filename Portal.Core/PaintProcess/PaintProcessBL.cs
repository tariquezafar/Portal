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
    public class PaintProcessBL
    {
        DBInterface dbInterface;
        public PaintProcessBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut CancelPaintProcess(PaintProcessViewModel paintProcessViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                PaintProcess paintProcess = new PaintProcess
                {

                    PaintProcessId= paintProcessViewModel.PaintProcessId,
                    CancelStatus = "CANCELLED",
                    PaintProcessStatus = "CANCELLED",
                    CreatedBy = paintProcessViewModel.CreatedBy,
                    CompanyId = paintProcessViewModel.CompanyId,
                    CompanyBranchId = paintProcessViewModel.CompanyBranchId,
                    CancelReason = paintProcessViewModel.CancelReason,
                    FinYearId = paintProcessViewModel.FinYearId
                };
                responseOut = sqlDbInterface.CancelPaintProcess(paintProcess);
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



        public ResponseOut AddEditPaintProcess(PaintProcessViewModel paintProcessViewModel, List<PaintProcessProductViewModel> paintProcessProducts,List<PaintProcessChasisSerialViewModel> paintProcessChasisSerialProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                PaintProcess paintProcess = new PaintProcess
                {
                    PaintProcessId= paintProcessViewModel.PaintProcessId,
                    PaintProcessDate = Convert.ToDateTime(paintProcessViewModel.PaintProcessDate),
                    WorkOrderId = paintProcessViewModel.WorkOrderId,
                    WorkOrderNo = paintProcessViewModel.WorkOrderNo,                    
                    CompanyId = paintProcessViewModel.CompanyId,
                    CompanyBranchId = paintProcessViewModel.CompanyBranchId,
                    Remarks1 = paintProcessViewModel.Remarks1,
                    Remarks2 = paintProcessViewModel.Remarks2,
                    CreatedBy = paintProcessViewModel.CreatedBy,
                    PaintProcessStatus = paintProcessViewModel.PaintProcessStatus
                };
                List<PaintProcessProductDetail> paintProcessProductList = new List<PaintProcessProductDetail>();
                if(paintProcessProducts != null && paintProcessProducts.Count>0)
                {
                    foreach(PaintProcessProductViewModel item in paintProcessProducts)
                    {
                        paintProcessProductList.Add(new PaintProcessProductDetail
                        {
                            ProductId=item.ProductId,
                            Quantity=item.Quantity,
                            WOQTY=item.WorkorderQuantity,
                            AdjProductId = item.AdjProDuctID,
                            NewlyAdd=item.NewProduct,
                        });
                    }
                }

                List<PaintProcessChasisSerialDetail> paintProcessChasisSerialProductList = new List<PaintProcessChasisSerialDetail>();
                if (paintProcessChasisSerialProducts != null && paintProcessChasisSerialProducts.Count > 0)
                {
                    foreach (PaintProcessChasisSerialViewModel item in paintProcessChasisSerialProducts)
                    {
                        paintProcessChasisSerialProductList.Add(new PaintProcessChasisSerialDetail
                        {                          
                            ProductID = item.ProductId,
                            ChasisSerialNo=item.ChasisSerialNo,
                            MotorNo=item.MotorNo,
                            PaintStatus=Convert.ToBoolean(item.PaintFlag)

                        });
                    }
                }



                responseOut = sqlDbInterface.AddEditPaintProcess(paintProcess, paintProcessProductList, paintProcessChasisSerialProductList);
             
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
    
       
        public List<PaintProcessViewModel> GetPaintProcessList(string paintProcessNo, string workOrderNo, int companyBranchId,string fromDate, string toDate, int companyId, string paintProcessStatus = "")
        {
            List<PaintProcessViewModel> paintProcessViewModel = new List<PaintProcessViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPaintProcess = sqlDbInterface.GetPaintProcessList(paintProcessNo,workOrderNo, companyBranchId,  Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate),companyId, paintProcessStatus);
                if (dtPaintProcess != null && dtPaintProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPaintProcess.Rows)
                    {
                        paintProcessViewModel.Add(new PaintProcessViewModel
                        {
                            PaintProcessId = Convert.ToInt32(dr["PaintProcessId"]),
                            PaintProcessNo = Convert.ToString(dr["PaintProcessNo"]),
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            PaintProcessDate = Convert.ToString(dr["PaintProcessDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            PaintProcessStatus = Convert.ToString(dr["PaintProcessStatus"]),                          
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
            return paintProcessViewModel;
        }
        public PaintProcessViewModel GetPaintProcessDetail(long paintProcessId = 0)
        {
            PaintProcessViewModel paintProcessViewModel = new PaintProcessViewModel();           
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPaintProcess = sqlDbInterface.GetPaintProcessDetail(paintProcessId);
                if (dtPaintProcess != null && dtPaintProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPaintProcess.Rows)
                    {
                        paintProcessViewModel = new PaintProcessViewModel
                        {
                            PaintProcessId= Convert.ToInt32(dr["PaintProcessId"]),
                            PaintProcessNo= Convert.ToString(dr["PaintProcessNo"]),
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            WorkOrderDate= Convert.ToString(dr["WorkOrderDate"]),
                            PaintProcessDate = Convert.ToString(dr["PaintProcessDate"]),                           
                            CompanyBranchId =Convert.ToInt32(dr["CompanyBranchId"]),
                            PaintProcessStatus = Convert.ToString(dr["PaintProcessStatus"]),
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            TotalQuantity=Convert.ToDecimal(dr["TotalQuantity"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return paintProcessViewModel;
        }
    
 
        public List<PaintProcessProductViewModel> GetPaintProcessProductList(long paintProcessId)
        {
            List<PaintProcessProductViewModel> paintProcessProducts = new List<PaintProcessProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPaintProcess = sqlDbInterface.GetPaintProcessProductList(paintProcessId);
                if (dtPaintProcess != null && dtPaintProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPaintProcess.Rows)
                    {
                        paintProcessProducts.Add(new PaintProcessProductViewModel
                        {
                            PaintProcessDetailId = Convert.ToInt32(dr["PaintProcessDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            AdjProDuctID= Convert.ToInt32(dr["AdjProDuctID"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            WorkorderQuantity = Convert.ToDecimal(dr["WorkorderQuantity"]),
                            TotalPaintQuantity = Convert.ToDecimal(dr["TotalPaintQuantity"]),
                            RecivedQuantity = Convert.ToDecimal(dr["RecivedQuantity"]),
                            PendingQuantity = Convert.ToDecimal(dr["PendingQunatity"]),
                            RepQTY = Convert.ToDecimal(dr["RepQTY"]),
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

        public DataTable GetPaintProcessProductListDataTable(long paintProcessId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPaintProcess = new DataTable();
            try
            {
                dtPaintProcess = sqlDbInterface.GetPaintProcessProductListPrint(paintProcessId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPaintProcess;
        }

        public DataTable GetPaintProcessDataTable(long paintProcessId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPaintProcess = new DataTable();
            try
            {
                dtPaintProcess = sqlDbInterface.GetPaintProcessDetail(paintProcessId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPaintProcess;
        }

        public List<WorkOrderViewModel> GetPaintProcessWorkOrderList(string workOrderNo, int companyBranchId, string fromDate, string toDate, int companyId, string displayType = "", string approvalStatus = "")
        {
            List<WorkOrderViewModel> workOrders = new List<WorkOrderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetPaintProcessWorkOrderList(workOrderNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
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
                            Quantity = Convert.ToInt32(dr["Quantity"]),
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
        public decimal GetPaintProcessProducedQuantityAgainstWorkOrder(long workOrderId)
        {
            decimal productQuantity = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                productQuantity = sqldbinterface.GetPaintProcessProducedQuantityAgainstWorkOrder(workOrderId);
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
                DataTable dtPaintProcess = sqlDbInterface.GetProductSerialProduct();
                if (dtPaintProcess != null && dtPaintProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPaintProcess.Rows)
                    {
                        ProductSerialDetailProducts.Add(new ProductSerialDetailViewModel
                        {                           
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
        public List<PaintProcessChasisSerialViewModel> GetPaintProcessProductSerialProductList(long paintProcessId)
        {
            List<PaintProcessChasisSerialViewModel> paintProcessChasisSerialProducts = new List<PaintProcessChasisSerialViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPaintProcess = sqlDbInterface.GetPaintProcessProductSerialProductList(paintProcessId);
                if (dtPaintProcess != null && dtPaintProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPaintProcess.Rows)
                    {
                        paintProcessChasisSerialProducts.Add(new PaintProcessChasisSerialViewModel
                        {                                                    
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ChasisSerialNo = Convert.ToString(dr["ChasisSerialNo"]),
                            MotorNo = Convert.ToString(dr["MotorNo"]),
                            PaintFlag=Convert.ToString(dr["PaintStatus"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return paintProcessChasisSerialProducts;
        }

        public List<WorkOrderProductViewModel> GetPaintProcessWorkOrderProducts(long workOrderId)
        {
            List<WorkOrderProductViewModel> workOrderProducts = new List<WorkOrderProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetPaintProcessWorkOrderProducts(workOrderId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        workOrderProducts.Add(new WorkOrderProductViewModel
                        {
                            WorkOrderProductDetailId = Convert.ToInt32(dr["WorkOrderProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            WorkorderQuantity = Convert.ToDecimal(dr["WorkorderQuantity"]),
                            TotalPaintQuantity = Convert.ToDecimal(dr["TotalPaintQuantity"]),
                            RecivedQuantity = Convert.ToDecimal(dr["RecivedQuantity"]),
                            PendingQuantity = Convert.ToDecimal(dr["PendingQunatity"]),
                            RepQTY = Convert.ToDecimal(dr["RepQTY"]),
                            IsThirdPartyProduct=Convert.ToString(dr["IsThirdPartyProduct"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return workOrderProducts;
        }

        public DataTable GetPaintProcessChasisProductList(long paintProcessId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtPaintProcess = new DataTable();
            try
            {
                dtPaintProcess = sqlDbInterface.GetPaintProcessChasisProductList(paintProcessId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtPaintProcess;
        }
        public List<WorkOrderProductViewModel> GetWOProductList(long WorkOrderID)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            List<WorkOrderProductViewModel> products = new List<WorkOrderProductViewModel>();
            try
            {
                DataTable productList = sqlDbInterface.GetWOProductList(WorkOrderID);
                if (productList != null && productList.Rows.Count > 0)
                {
                    foreach (DataRow dr in productList.Rows)
                    {
                        products.Add(new WorkOrderProductViewModel
                        {
                            ProductId = Convert.ToInt32(dr["ProductID"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),

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
    }
}
