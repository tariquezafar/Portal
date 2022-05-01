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
    public class SaleDashboardBL
    {
        
        public SaleDashboardBL()
        {
            
        }
        public List<QuatationCountViewModel> GetQuatationCountList(int companyId, int userId, int reportingUserId, int reportingRoleId, int finYearId,out string quatationCountList,out string totalCountList)
        {
            
            List<QuatationCountViewModel> bookBalanceList = new List<QuatationCountViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtquatationCount = sqlDbInterface.GetDashboard_QuatationCount(companyId, userId, reportingUserId, reportingRoleId,finYearId);
                if (dtquatationCount != null && dtquatationCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtquatationCount.Rows)
                    {
                        bookBalanceList.Add(new QuatationCountViewModel
                        {                         
                            Count_Head = Convert.ToString(dr["Count_Head"]),                          
                            TotalCount = Convert.ToString(dr["TotalCount"])
                        });
                    }
                }
                var countHead = (from temp in bookBalanceList
                                 select temp.Count_Head).ToList();

                var total_Count = (from temp in bookBalanceList
                                    select temp.TotalCount).ToList();

                quatationCountList = "'"+ string.Join("','", countHead) + "'";


                totalCountList = string.Join(",", total_Count);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookBalanceList;
        }
        public List<Container9ViewModel> GetContainer9List(int roleId, int companyId, int companyBranchId, int finYearId, int boxnumber)
        {
            List<Container9ViewModel> containerList = new List<Container9ViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtContainerList = sqlDbInterface.GetDashboard_SaleDashboardContainer9(roleId,companyId,companyBranchId,finYearId,boxnumber);
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
        public List<SOCountViewModel> GetSOCountList(int companyId, int userId, int reportingUserId, int reportingRoleId, int finYearId,int companyBranchId, out string sOCountList, out string sOtotalCountList)
        {

            List<SOCountViewModel> bookBalanceList = new List<SOCountViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtquatationCount = sqlDbInterface.GetDashboard_SaleOrderCount(companyId, userId, reportingUserId, reportingRoleId, finYearId, companyBranchId);
                if (dtquatationCount != null && dtquatationCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtquatationCount.Rows)
                    {
                        bookBalanceList.Add(new SOCountViewModel
                        {
                            sOCount_Head = Convert.ToString(dr["Count_Head"]),
                            sOTotalCount = Convert.ToString(dr["TotalCount"])
                        });
                    }
                }
                var SOcountHead = (from temp in bookBalanceList
                                 select temp.sOCount_Head).ToList();

                var SOtotal_Count = (from temp in bookBalanceList
                                   select temp.sOTotalCount).ToList();

                sOCountList = "'" + string.Join("','", SOcountHead) + "'";


                sOtotalCountList = string.Join(",", SOtotal_Count);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookBalanceList;
        }

        public List<SICountViewModel> GetSICountList(int companyId, int userId, int reportingUserId, int reportingRoleId, int finYearId,int companyBranchId, out string sICountList, out string sItotalCountList)
        {

            List<SICountViewModel> bookBalanceList = new List<SICountViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtquatationCount = sqlDbInterface.GetDashboard_SICount(companyId,userId,reportingUserId,reportingRoleId, finYearId, companyBranchId);
                if (dtquatationCount != null && dtquatationCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtquatationCount.Rows)
                    {
                        bookBalanceList.Add(new SICountViewModel
                        {
                            SICount_Head = Convert.ToString(dr["Count_Head"]),
                            SITotalCount = Convert.ToString(dr["TotalCount"])
                        });
                    }
                }
                var sIcountHead = (from temp in bookBalanceList
                                   select temp.SICount_Head).ToList();

                var sItotal_Count = (from temp in bookBalanceList
                                     select temp.SITotalCount).ToList();

                sICountList = "'" + string.Join("','", sIcountHead) + "'";


                sItotalCountList = string.Join(",", sItotal_Count);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookBalanceList;
        }



        public List<Container9ViewModel> GetInventoryDashboardList(int roleId, int companyId, int companyBranchId, int finYearId, int boxnumber)
        {
            List<Container9ViewModel> containerList = new List<Container9ViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtContainerList = sqlDbInterface.GetDashboard_InventoryContainerList(roleId, companyId, companyBranchId, finYearId, boxnumber);
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








