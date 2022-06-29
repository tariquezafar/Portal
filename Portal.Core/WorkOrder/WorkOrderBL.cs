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
    public class WorkOrderBL
    {
        DBInterface dbInterface;
        public WorkOrderBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditWorkOrder(WorkOrderViewModel workOrderViewModel,List<WorkOrderProductViewModel> workOrderProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                WorkOrder workOrder= new WorkOrder
                {
                    WorkOrderId = workOrderViewModel.WorkOrderId,
                    WorkOrderDate = Convert.ToDateTime(workOrderViewModel.WorkOrderDate),
                    TargetFromDate = Convert.ToDateTime(workOrderViewModel.TargetFromDate),
                    TargetToDate = Convert.ToDateTime(workOrderViewModel.TargetToDate),
                    CompanyId = workOrderViewModel.CompanyId,
                    CompanyBranchId = workOrderViewModel.CompanyBranchId,
                    Remarks1 = workOrderViewModel.Remarks1,
                    Remarks2 = workOrderViewModel.Remarks2,
                    CreatedBy = workOrderViewModel.CreatedBy,
                    SOId = workOrderViewModel.SOId,
                    SONo = workOrderViewModel.SONo,
                    WorkOrderStatus = workOrderViewModel.WorkOrderStatus,
                    LocationId=workOrderViewModel.LocationId,
                };
                List<WorkOrderProductDetail> workOrderProductList = new List<WorkOrderProductDetail>();
                if(workOrderProducts != null && workOrderProducts.Count>0)
                {
                    foreach(WorkOrderProductViewModel item in workOrderProducts)
                    {
                        workOrderProductList.Add(new WorkOrderProductDetail
                        {
                            ProductId=item.ProductId,
                            Quantity=item.Quantity
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditWorkOrder(workOrder, workOrderProductList);
             

             
             
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
    
       
        public List<WorkOrderViewModel> GetWorkOrderList(string workOrderNo, int companyBranchId,string fromDate, string toDate, int companyId, string displayType = "",string approvalStatus="")
        {
            List<WorkOrderViewModel> workOrders = new List<WorkOrderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetWorkOrderList(workOrderNo, companyBranchId,  Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate),companyId, displayType, approvalStatus);
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
                            AssemblyType= Convert.ToString(dr["AssemblyType"]),
                            SONo = Convert.ToString(dr["SONo"]),
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
        public WorkOrderViewModel GetWorkOrderDetail(long workOrderId = 0)
        {
            WorkOrderViewModel workOrder = new WorkOrderViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCompanies = sqlDbInterface.GetWorkOrderDetail(workOrderId);
                if (dtCompanies != null && dtCompanies.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCompanies.Rows)
                    {
                        workOrder = new WorkOrderViewModel
                        {
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            WorkOrderDate = Convert.ToString(dr["WorkOrderDate"]),
                            TargetFromDate = Convert.ToString(dr["TargetFromDate"]),
                            TargetToDate = Convert.ToString(dr["TargetToDate"]),
                            CompanyBranchId =Convert.ToInt32(dr["CompanyBranchId"]),
                            WorkOrderStatus = Convert.ToString(dr["WorkOrderStatus"]),
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            SOId = Convert.ToInt32(dr["SOId"]),
                            SONo = Convert.ToString(dr["SONo"]),
                            SODate = Convert.ToString(dr["SODate"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            LocationId=Convert.ToInt32(dr["LocationId"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return workOrder;
        }
    
 
        public List<WorkOrderProductViewModel> GetWorkOrderProductList(long workOrderId)
        {
            List<WorkOrderProductViewModel> workOrderProducts = new List<WorkOrderProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetWorkOrderProductList(workOrderId);
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
                            AssemblyType=Convert.ToString(dr["AssemblyType"]),
                            
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

        public DataTable GetWorkOrderProductListDataTable(long workOrderId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtProducts = new DataTable();
            try
            {
                dtProducts = sqlDbInterface.GetWorkOrderProductList(workOrderId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtProducts;
        }

        public DataTable GetWorkOrderDataTable(long workOrderId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtworkOrder = new DataTable();
            try
            {
                dtworkOrder = sqlDbInterface.GetWorkOrderDetail(workOrderId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtworkOrder;
        }
        public List<ProductViewModel> GetProductAutoCompleteBOMList(string searchTerm, int companyId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            List<ProductViewModel> products = new List<ProductViewModel>();
            try
            {
                DataTable productList = sqlDbInterface.GetProductAutoCompleteBOMList(searchTerm, companyId);
                if (productList != null && productList.Rows.Count > 0)
                {
                    foreach (DataRow dr in productList.Rows)
                    {
                        products.Add(new ProductViewModel
                        {
                            Productid = Convert.ToInt32(dr["Productid"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            CGST_Perc = Convert.ToDecimal(dr["CGST_Perc"]),
                            SGST_Perc = Convert.ToDecimal(dr["SGST_Perc"]),
                            IGST_Perc = Convert.ToDecimal(dr["IGST_Perc"]),
                            HSN_Code = Convert.ToString(dr["HSN_Code"]),
                            AssemblyType=Convert.ToString(dr["AssemblyType"]),
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

        public List<WorkOrderViewModel> GetWorkOrderAutoCompleteList(string searchTerm)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            List<WorkOrderViewModel> products = new List<WorkOrderViewModel>();
            try
            {
                DataTable productList = sqlDbInterface.GetWorkOrderAutoCompleteList(searchTerm);
                if (productList != null && productList.Rows.Count > 0)
                {
                    foreach (DataRow dr in productList.Rows)
                    {
                        products.Add(new WorkOrderViewModel
                        {
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),                           
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

        public DataTable GetWIPReport(long workOrderID, int companyId)
        {

            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtBOMReports = new DataTable();
            try
            {
                dtBOMReports = sqlDbInterface.GetWIPReport(workOrderID, companyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtBOMReports;
        }

        public List<SOViewModel> GetWOSOList(string soNo, string customerName, string refNo, string fromDate, string toDate, int companyId,int companyBranchId, string approvalStatus = "", string displayType = "")
        {
            List<SOViewModel> sos = new List<SOViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSOs = sqlDbInterface.GetWOSOList(soNo, customerName, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranchId, approvalStatus, displayType);
                if (dtSOs != null && dtSOs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSOs.Rows)
                    {
                        sos.Add(new SOViewModel
                        {
                            SOId = Convert.ToInt32(dr["SOId"]),
                            SONo = Convert.ToString(dr["SONo"]),
                            SODate = Convert.ToString(dr["SODate"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            QuotationNo = Convert.ToString(dr["QuotationNo"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            BasicValue = Convert.ToDecimal(dr["BasicValue"]),
                            TotalValue = Convert.ToDecimal(dr["TotalValue"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sos;
        }
        public List<SOProductViewModel> GetWOSOProductList(long soId)
        {
            List<SOProductViewModel> soProducts = new List<SOProductViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetWOSOProductList(soId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        soProducts.Add(new SOProductViewModel
                        {
                            SOProductDetailId = Convert.ToInt32(dr["SOProductDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ProductCode = Convert.ToString(dr["ProductCode"]),
                            ProductShortDesc = Convert.ToString(dr["ProductShortDesc"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            Quantity = Convert.ToDecimal(dr["Quantity"]),
                            DiscountPercentage = Convert.ToDecimal(dr["DiscountPercentage"]),
                            DiscountAmount = Convert.ToDecimal(dr["DiscountAmount"]),
                            TaxId = Convert.ToInt32(dr["TaxId"]),
                            TaxName = Convert.ToString(dr["TaxName"]),
                            TaxPercentage = Convert.ToDecimal(dr["TaxPercentage"]),
                            TaxAmount = Convert.ToDecimal(dr["TaxAmount"]),
                            TotalPrice = Convert.ToDecimal(dr["TotalPrice"]),
                            AssemblyType=Convert.ToString(dr["AssemblyType"]),
                           
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return soProducts;
        }

        public List<WorkOrderViewModel> GetCostWorkOrderList(string workOrderNo, int companyBranchId, string fromDate, string toDate, int companyId, string displayType = "", string approvalStatus = "")
        {
            List<WorkOrderViewModel> workOrders = new List<WorkOrderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtQuotations = sqlDbInterface.GetCostWorkOrderList(workOrderNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
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
                            Quantity = Convert.ToInt32(dr["Quantity"])
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

        public DataTable GetWorkOrderCostDetail(long workOrderId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtworkOrder = new DataTable();
            try
            {
                dtworkOrder = sqlDbInterface.GetWorkOrderCostDetail(workOrderId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtworkOrder;
        }
        public ResponseOut CancelWO(WorkOrderViewModel workOrderViewModel)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                WorkOrder wo = new WorkOrder
                {
                    WorkOrderId = workOrderViewModel.WorkOrderId,
                    WorkOrderNo = workOrderViewModel.WorkOrderNo,
                    CancelStatus = "Cancel",
                    WorkOrderStatus = "Cancelled",
                    CreatedBy = workOrderViewModel.CreatedBy,
                    CancelReason = workOrderViewModel.CancelReason
                };
                responseOut = sqlDbInterface.CancelWO(wo);
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
