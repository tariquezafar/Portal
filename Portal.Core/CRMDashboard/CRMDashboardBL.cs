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
    public class CRMDashboardBL
    {
        
        public CRMDashboardBL()
        {
            
        }
        public List<LeadStatusCountViewModel> GetLeadStatusCountList(int companyId,int userId, int reportingUserId, int reportingRoleId)
        {
            
            List<LeadStatusCountViewModel> leadStatusCountList = new List<LeadStatusCountViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCount= sqlDbInterface.GetDashboard_LeadStatusCount(companyId,userId, reportingUserId, reportingRoleId);
                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCount.Rows)
                    {
                        leadStatusCountList.Add(new LeadStatusCountViewModel
                        {
                            LeadStatusId = Convert.ToInt16(dr["LeadStatusId"]),
                            LeadStatusName = Convert.ToString(dr["LeadStatusName"]),
                            TotalLeadCount = Convert.ToInt16(dr["TotalLeadCount"]),
                            TodayCount = Convert.ToInt16(dr["TodayCount"]),
                            MTDCount = Convert.ToInt16(dr["MTDCount"]),
                            YTDCount = Convert.ToInt16(dr["YTDCount"])
                        });
                    }
                }
               
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadStatusCountList;
        }
        public List<LeadSourceCountViewModel> GetLeadSourceCountList(int companyId, int userId, int reportingUserId, int reportingRoleId)
        {

            List<LeadSourceCountViewModel> leadSourceCountList = new List<LeadSourceCountViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCount = sqlDbInterface.GetDashboard_LeadSourceCount(companyId, userId, reportingUserId, reportingRoleId);
                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCount.Rows)
                    {
                        leadSourceCountList.Add(new LeadSourceCountViewModel
                        {
                            LeadSourceId = Convert.ToInt16(dr["LeadSourceId"]),
                            LeadSourceName = Convert.ToString(dr["LeadSourceName"]),
                            TotalLeadCount = Convert.ToInt16(dr["TotalLeadCount"]),
                            TodayCount = Convert.ToInt16(dr["TodayCount"]),
                            MTDCount = Convert.ToInt16(dr["MTDCount"]),
                            YTDCount = Convert.ToInt16(dr["YTDCount"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadSourceCountList;
        }
        public List<LeadFollowUpCountViewModel> GetLeadFollowUpCountList(int companyId, int userId, int reportingUserId,int reportingRoleId)
        {

            List<LeadFollowUpCountViewModel> leadFollowUpCountList = new List<LeadFollowUpCountViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCount = sqlDbInterface.GetDashboard_LeadFollowUpCount(companyId, userId, reportingUserId, reportingRoleId);
                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCount.Rows)
                    {
                        leadFollowUpCountList.Add(new LeadFollowUpCountViewModel
                        {
                            FollowUpActivityTypeId = Convert.ToInt16(dr["FollowUpActivityTypeId"]),
                            FollowUpActivityTypeName = Convert.ToString(dr["FollowUpActivityTypeName"]),
                            TodayCount = Convert.ToInt16(dr["TodayCount"]),
                            TommorowCount = Convert.ToInt16(dr["TommorowCount"]),
                            WeekCount = Convert.ToInt16(dr["WeekCount"]),
                            MonthCount = Convert.ToInt16(dr["MonthCount"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadFollowUpCountList;
        }
        public List<LeadConversionCountViewModel> GetDateWiseLeadConversionCountList(int companyId, int userId, int reportingUserId, int reportingRoleId, out string dateList,out string totalLeadCountList,out string newOpportunityCountList,out string quotationCountList)
        {
            List<LeadConversionCountViewModel> leadConversionCountList = new List<LeadConversionCountViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCount = sqlDbInterface.GetDashboard_DateWiseLeadConversionCount(companyId, userId, reportingUserId, reportingRoleId);
                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCount.Rows)
                    {
                        leadConversionCountList.Add(new LeadConversionCountViewModel
                        {
                            DateValue = Convert.ToString(dr["DateValue"]),
                            TotalLead = Convert.ToInt16(dr["TotalLead"]),
                            NewOpertunity = Convert.ToInt16(dr["NewOpertunity"]),
                            Quotation = Convert.ToInt16(dr["Quotation"])
                        });
                    }
                }
                var dateValues = (from temp in leadConversionCountList
                                  select temp.DateValue).ToList();

                var totalLeads = (from temp in leadConversionCountList
                                        select temp.TotalLead).ToList();
                var newOpertunities = (from temp in leadConversionCountList
                                  select temp.NewOpertunity).ToList();
                var quotations = (from temp in leadConversionCountList
                                  select temp.Quotation).ToList();

                dateList = "'" + string.Join("','", dateValues) + "'";
                totalLeadCountList = string.Join(",", totalLeads);
                newOpportunityCountList = string.Join(",", newOpertunities);
                quotationCountList = string.Join(",", quotations);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadConversionCountList;
        }
        public List<LeadFollowUpReminderDueDateCountViewModel> GetLeadFollowUpReminderDueDateCountList(int companyId, int userId, int reportingUserId, int reportingRoleId)
        {

            List<LeadFollowUpReminderDueDateCountViewModel> leadFollowUpCountList = new List<LeadFollowUpReminderDueDateCountViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCount = sqlDbInterface.GetDashboard_LeadFollowUpReminderDueDateCount(companyId, userId, reportingUserId, reportingRoleId);
                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCount.Rows)
                    {
                        leadFollowUpCountList.Add(new LeadFollowUpReminderDueDateCountViewModel
                        {
                            FollowUpActivityTypeId = Convert.ToInt16(dr["FollowUpActivityTypeId"]),
                            FollowUpActivityTypeName = Convert.ToString(dr["FollowUpActivityTypeName"]),
                            ReminderCount = Convert.ToInt16(dr["ReminderCount"]),
                            DueDateCount = Convert.ToInt16(dr["DueDateCount"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadFollowUpCountList;
        }

        public List<LeadFollowUpReminderDueDateListViewModel> GetLeadFollowUpReminderDueDateList(int companyId, int userId, int reportingUserId, int reportingRoleId, int FollowUpActivityTypeId)
        {

            List<LeadFollowUpReminderDueDateListViewModel> leadFollowUpList = new List<LeadFollowUpReminderDueDateListViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtList = sqlDbInterface.GetDashboard_LeadFollowUpReminderDueDateList(companyId, userId, reportingUserId, reportingRoleId, FollowUpActivityTypeId);
                if (dtList != null && dtList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtList.Rows)
                    {
                        leadFollowUpList.Add(new LeadFollowUpReminderDueDateListViewModel
                        {
                            LeadId = Convert.ToInt16(dr["leadid"]),
                            LeadCode = Convert.ToString(dr["LeadCode"]),
                            CompanyName = Convert.ToString(dr["CompanyName"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadFollowUpList;
        }

        public List<InventoryDashboardItemsViewModel> GetCRMDashboardItems(int roleId, int companyId, int companyBranchId, int finYearId)
        {
            List<InventoryDashboardItemsViewModel> inventoryDashboardItems = new List<InventoryDashboardItemsViewModel>();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtInventoryDashboardItemList = sqldbinterface.HRDashboardItems(roleId, companyId, companyBranchId, finYearId);

                if (dtInventoryDashboardItemList != null && dtInventoryDashboardItemList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtInventoryDashboardItemList.Rows)
                    {
                        inventoryDashboardItems.Add(new InventoryDashboardItemsViewModel
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
            return inventoryDashboardItems;
        }
    }
}








