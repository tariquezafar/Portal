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
    public class FabricationBL
    {
        DBInterface dbInterface;
        public FabricationBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditFabrication(FabricationViewModel fabricationViewModel, List<FabricationProductViewModel> fabricationProducts, List<FabricationChasisSerialViewModel> fabricationChasisSerialProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                Fabrication fabrication = new Fabrication
                {
                    FabricationId = fabricationViewModel.FabricationId,
                    FabricationDate = Convert.ToDateTime(fabricationViewModel.FabricationDate),
                    WorkOrderId = fabricationViewModel.WorkOrderId,
                    WorkOrderNo = fabricationViewModel.WorkOrderNo,
                    CompanyId = fabricationViewModel.CompanyId,
                    CompanyBranchId = fabricationViewModel.CompanyBranchId,
                    Remarks1 = fabricationViewModel.Remarks1,
                    Remarks2 = fabricationViewModel.Remarks2,
                    CreatedBy = fabricationViewModel.CreatedBy,
                    FabricationStatus = fabricationViewModel.FabricationStatus
                };
                List<FabricationProductDetail> fabricationProductList = new List<FabricationProductDetail>();
                if (fabricationProducts != null && fabricationProducts.Count > 0)
                {
                    foreach (FabricationProductViewModel item in fabricationProducts)
                    {
                        fabricationProductList.Add(new FabricationProductDetail
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity
                        });
                    }
                }

                List<FabricationChasisSerialDetail> fabricationChasisSerialProductList = new List<FabricationChasisSerialDetail>();
                if (fabricationChasisSerialProducts != null && fabricationChasisSerialProducts.Count > 0)
                {
                    foreach (FabricationChasisSerialViewModel item in fabricationChasisSerialProducts)
                    {
                            fabricationChasisSerialProductList.Add(new FabricationChasisSerialDetail
                            {
                                FabricationId = item.FabricationId,
                                FabricationDetailId = item.FabricationDetailId,
                                ProductId = item.ProductId,
                                ChasisSerialNo = item.ChasisSerialNo,
                                MotorNo = item.MotorNo
                            });
                    }
                }

                responseOut = sqlDbInterface.AddEditFabrication(fabrication, fabricationProductList, fabricationChasisSerialProductList);
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


        public List<FabricationViewModel> GetFabricationList(string fabricationNo, string workOrderNo, int companyBranchId, string fromDate, string toDate, int companyId, string fabricationStatus = "")
        {
            List<FabricationViewModel> fabricationViewModel = new List<FabricationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetFabricationList(fabricationNo, workOrderNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, fabricationStatus);
                if (dtQuotations != null && dtQuotations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtQuotations.Rows)
                    {
                        fabricationViewModel.Add(new FabricationViewModel
                        {
                            FabricationId = Convert.ToInt32(dr["FabricationId"]),
                            FabricationNo = Convert.ToString(dr["FabricationNo"]),
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            FabricationDate = Convert.ToString(dr["FabricationDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            FabricationStatus = Convert.ToString(dr["FabricationStatus"]),
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
            return fabricationViewModel;
        }
        public FabricationViewModel GetFabricationDetail(long fabricationId = 0)
        {
            FabricationViewModel fabricationViewModel = new FabricationViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetFabricationDetail(fabricationId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        fabricationViewModel = new FabricationViewModel
                        {
                            FabricationId = Convert.ToInt32(dr["FabricationId"]),
                            FabricationNo = Convert.ToString(dr["FabricationNo"]),
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            WorkOrderDate = Convert.ToString(dr["WorkOrderDate"]),
                            FabricationDate = Convert.ToString(dr["FabricationDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            FabricationStatus = Convert.ToString(dr["FabricationStatus"]),
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            WorkOrderQuantity = Convert.ToDecimal(dr["WorkOrderQuantity"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
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
            return fabricationViewModel;
        }


        public List<FabricationProductViewModel> GetFabricationProductList(long fabricationId)
        {
            List<FabricationProductViewModel> fabricationProducts = new List<FabricationProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetFabricationProductList(fabricationId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        fabricationProducts.Add(new FabricationProductViewModel
                        {
                            FabricationDetailId = Convert.ToInt32(dr["FabricationDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            IsSerializedProduct=Convert.ToString(dr["IsSerializedProduct"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            WorkorderQuantity = Convert.ToDecimal(dr["WorkorderQuantity"]),
                            TotalRecivedFabQuantity = Convert.ToDecimal(dr["TotalRecivedFabQuantity"]),
                            RecivedQuantity = Convert.ToDecimal(dr["RecivedQuantity"]),
                            PendingQuantity = Convert.ToDecimal(dr["PendingQunatity"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return fabricationProducts;
        }

        public DataTable GetFabricationProductListDataTable(long fabricationId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetFabricationProductListPrint(fabricationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }
        public DataTable GetFabricationChasisPrint(long fabricationId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetFabricationChasisPrint(fabricationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public DataTable GetFabricationDataTable(long fabricationId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtFabrication = new DataTable();
            try
            {
                dtFabrication = sqlDbInterface.GetFabricationDetail(fabricationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtFabrication;
        }

        public List<WorkOrderViewModel> GetFabricationWorkOrderList(string workOrderNo, int companyBranchId, string fromDate, string toDate, int companyId, string displayType = "", string approvalStatus = "")
        {
            List<WorkOrderViewModel> workOrders = new List<WorkOrderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetFabricationWorkOrderList(workOrderNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
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
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
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

        public decimal GetFabricationProducedQuantityAgainstWorkOrder(long workOrderId)
        {
            decimal productQuantity = 0;
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                productQuantity = sqldbinterface.GetFabricationProducedQuantityAgainstWorkOrder(workOrderId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productQuantity;
        }

        public List<FabricationChasisSerialViewModel> GetFabricationChasisSerials()
        {
            List<FabricationChasisSerialViewModel> FabricationChasisPlanDetails = new List<FabricationChasisSerialViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtChasis = sqlDbInterface.GetFabricationChasisSerials();
                if (dtChasis != null && dtChasis.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtChasis.Rows)
                    {
                        FabricationChasisPlanDetails.Add(new FabricationChasisSerialViewModel
                        {
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            FabricationId = Convert.ToInt32(dr["FabricationId"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ChasisSerialNo = Convert.ToString(dr["ChasisSerialNo"]),
                            MotorNo = Convert.ToString(dr["MotorNo"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return FabricationChasisPlanDetails;
        }

        public List<WorkOrderProductViewModel> GetFabricationWorkOrderProductList(long workOrderId)
        {
            List<WorkOrderProductViewModel> workOrderProducts = new List<WorkOrderProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetFabricationWorkOrderProductList(workOrderId);
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
                            IsSerializedProduct=Convert.ToString(dr["IsSerializedProduct"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            WorkorderQuantity = Convert.ToDecimal(dr["WorkorderQuantity"]),
                            TotalRecivedFabQuantity = Convert.ToDecimal(dr["TotalRecivedFabQuantity"]),
                            RecivedQuantity = Convert.ToDecimal(dr["RecivedQuantity"]),
                            PendingQuantity = Convert.ToDecimal(dr["PendingQunatity"]),

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
        public List<ProductSerialDetailViewModel> GetProductSerialProduct()
        {
            List<ProductSerialDetailViewModel> ProductSerialDetailProducts = new List<ProductSerialDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPaintProcess = sqlDbInterface.GetFabricationChasisSerialProduct();
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

        public List<FabricationChasisSerialViewModel> GetFabricationProductSerialProductList(long fabricationID)
        {
            List<FabricationChasisSerialViewModel> fabricationChasisSerialProducts = new List<FabricationChasisSerialViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPaintProcess = sqlDbInterface.GetFabricationProductSerialProductList(fabricationID);
                if (dtPaintProcess != null && dtPaintProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPaintProcess.Rows)
                    {
                        fabricationChasisSerialProducts.Add(new FabricationChasisSerialViewModel
                        {
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ChasisSerialNo = Convert.ToString(dr["ChasisSerialNo"]),
                            MotorNo = Convert.ToString(dr["MotorNo"]),
                            FabricatedFlag = Convert.ToString(dr["FabricatedStatus"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return fabricationChasisSerialProducts;
        }
        public ResponseOut CancelFabrication(FabricationViewModel fabricationViewModel)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Fabrication fabrication = new Fabrication
                {
                    FabricationId = fabricationViewModel.FabricationId,
                    FabricationNo = fabricationViewModel.FabricationNo,
                    CancelStatus = "Cancel",
                    FabricationStatus = "Cancelled",
                    CreatedBy = fabricationViewModel.CreatedBy,
                    CancelReason = fabricationViewModel.CancelReason,
                    CompanyId= fabricationViewModel.CompanyId,
                    CompanyBranchId= fabricationViewModel.CompanyBranchId
                };
                responseOut = sqlDbInterface.CancelFabrication(fabrication);
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
