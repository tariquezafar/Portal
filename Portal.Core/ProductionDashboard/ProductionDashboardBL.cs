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

namespace Portal.Core
{
    public class ProductionDashboardBL
    {
        
        public ProductionDashboardBL()
        {
            
        }
       
        //public int GetTotalProductCount(int CompanyId)
        //{
        //    int totalProductCount = 0;
        //    SQLDbInterface sqldbinterface = new SQLDbInterface();
        //    try
        //    {
        //        totalProductCount = sqldbinterface.GetTotalProductCount(CompanyId);              
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //        throw ex;
        //    }
        //    return totalProductCount;
        //}

        //public int GetTodayProduct(int CompanyId)
        //{
        //    int todayProductCount = 0;
        //    SQLDbInterface sqldbinterface = new SQLDbInterface();
        //    try
        //    {
        //        todayProductCount = sqldbinterface.GetTodayProduct(CompanyId);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //        throw ex;
        //    }
        //    return todayProductCount;
        //}

        public List<SOPendingViewModel> GetSOPendingList(int companyId, int finyear)
        {

            List<SOPendingViewModel> sOPendingCountList = new List<SOPendingViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSOCount = sqlDbInterface.GetDashboard_SOPendingCount(companyId, finyear);
                if (dtSOCount != null && dtSOCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSOCount.Rows)
                    {
                        sOPendingCountList.Add(new SOPendingViewModel
                        {
                            SOId = Convert.ToInt32(dr["SOId"]),
                            SONo = Convert.ToString(dr["SONo"]),
                            QuotationNo= Convert.ToString(dr["QuotationNo"]),
                            SODate = Convert.ToString(dr["SODate"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerCode = Convert.ToString(dr["CustomerCode"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByName= Convert.ToString(dr["CreatedByName"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sOPendingCountList;
        }

        public List<ProductionSummaryReportViewModel> GetProdctionSummaryReportList(int companyId, int finyear,int CompanyBranchId)
        {

            List<ProductionSummaryReportViewModel> productionSummaryReportList = new List<ProductionSummaryReportViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProdctionSummaryReport = sqlDbInterface.GetDashboard_ProdctionSummaryReport(companyId, finyear, CompanyBranchId);
                if (dtProdctionSummaryReport != null && dtProdctionSummaryReport.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProdctionSummaryReport.Rows)
                    {
                        productionSummaryReportList.Add(new ProductionSummaryReportViewModel
                        {
                            Sno = Convert.ToInt32(dr["TrnId"]),
                            Nature = Convert.ToString(dr["Nature"]),
                            TotalValue = Convert.ToInt32(dr["TotalValue"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productionSummaryReportList;
        }

        public List<WorkOrderViewModel> GetWOPendingList(int companyId)
        {

            List<WorkOrderViewModel> wOPendingCountList = new List<WorkOrderViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSOCount = sqlDbInterface.GetDashboard_WOPending(companyId);
                if (dtSOCount != null && dtSOCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSOCount.Rows)
                    {
                        wOPendingCountList.Add(new WorkOrderViewModel
                        {
                            SOId = Convert.ToInt32(dr["SOId"]),
                            SONo = Convert.ToString(dr["SONo"]),
                            WorkOrderId = Convert.ToInt32(dr["WorkOrderId"]),
                            WorkOrderNo = Convert.ToString(dr["WorkOrderNo"]),
                            WOQTY = Convert.ToDecimal(dr["WorkOrderQTY"]),
                            FinishedGoodQTY = Convert.ToDecimal(dr["FinishedGoodQTY"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return wOPendingCountList;
        }

        public List<ProductionDashboardItemsViewModel> GetProductionDashboardItems(int roleId, int companyId, int companyBranchId, int finYearId)
        {
            List<ProductionDashboardItemsViewModel> productionDashboardItems = new List<ProductionDashboardItemsViewModel>();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtProductionDashboardItemList = sqldbinterface.ProductionDashboardItems(roleId, companyId, companyBranchId, finYearId);

                if (dtProductionDashboardItemList != null && dtProductionDashboardItemList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProductionDashboardItemList.Rows)
                    {
                        productionDashboardItems.Add(new ProductionDashboardItemsViewModel
                        {
                            SrNo = Convert.ToInt32(dr["SrNo"]),
                            ContainerItemKey = Convert.ToString(dr["ContainerItemKey"]),
                            ContainerItemValue = Convert.ToString(dr["ContainerItemValue"]),
                            BoxNumber = Convert.ToString(dr["BoxNumber"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productionDashboardItems;
        }

        public List<Container9ViewModel> GetProductionDashboardList(int roleId, int companyId, int companyBranchId, int finYearId, int boxnumber)
        {
            List<Container9ViewModel> containerList = new List<Container9ViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtContainerList = sqlDbInterface.GetDashboard_ProdctionContainerList(roleId, companyId, companyBranchId, finYearId, boxnumber);
                if (dtContainerList != null && dtContainerList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtContainerList.Rows)
                    {
                        containerList.Add(new Container9ViewModel
                        {
                            ContainerKey = Convert.ToString(dr["ContainerItemKey"]),
                            ContainerValue = Convert.ToString(dr["ContainerItemValue"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return containerList;
        }
    }
}








