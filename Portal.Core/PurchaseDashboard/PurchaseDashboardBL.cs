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
    public class PurchaseDashboardBL
    {
        public List<PurchaseIndentViewModel> GetPendingIndentList(int companyId, int finYearId)
        {
            List<PurchaseIndentViewModel> qcs = new List<PurchaseIndentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisitions = sqlDbInterface.GetPendingIndentList(companyId, finYearId);
                if (dtRequisitions != null && dtRequisitions.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisitions.Rows)
                    {
                        qcs.Add(new PurchaseIndentViewModel
                        {

                            IndentId = Convert.ToInt32(dr["IndentId"]),
                            IndentNo = Convert.ToString(dr["IndentNo"]),
                            IndentDate = Convert.ToString(dr["IndentDate"]),
                            CreatedByUserName = Convert.ToString(dr["UserName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return qcs;
        }
        public PurchaseDashboardBL()
        {
            
        }
        public List<POCountViewModel> GetPOCountList(int companyId,int companyBranchId, int userId, int reportingUserId, int reportingRoleId, int finYearId, out string pOCountList,out string pOtotalCountList)
        {
            
            List<POCountViewModel> bookBalanceList = new List<POCountViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPOCount = sqlDbInterface.GetDashboard_POCount(companyId, companyBranchId, userId, reportingUserId, reportingRoleId, finYearId);
                if (dtPOCount != null && dtPOCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPOCount.Rows)
                    {
                        bookBalanceList.Add(new POCountViewModel
                        {                         
                            POCount_Head = Convert.ToString(dr["Count_Head"]),                          
                            POTotalCount = Convert.ToString(dr["TotalCount"])
                        });
                    }
                }
                var PocountHead = (from temp in bookBalanceList
                                 select temp.POCount_Head).ToList();

                var POtotal_Count = (from temp in bookBalanceList
                                    select temp.POTotalCount).ToList();

                pOCountList = "'"+ string.Join("','", PocountHead) + "'";


                pOtotalCountList = string.Join(",", POtotal_Count);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookBalanceList;
        }

        public List<PICountViewModel> GetPICountList(int companyId, int companyBranchId, int userId, int reportingUserId, int reportingRoleId, int finYearId, out string pICountList, out string pItotalCountList)
        {

            List<PICountViewModel> bookBalanceList = new List<PICountViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPICount = sqlDbInterface.GetDashboard_PICount(companyId, companyBranchId, userId, reportingUserId, reportingRoleId, finYearId);
                if (dtPICount != null && dtPICount.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPICount.Rows)
                    {
                        bookBalanceList.Add(new PICountViewModel
                        {
                            PICount_Head = Convert.ToString(dr["Count_Head"]),
                            PITotalCount = Convert.ToString(dr["TotalCount"])
                        });
                    }
                }
                var PIcountHead = (from temp in bookBalanceList
                                 select temp.PICount_Head).ToList();

                var PItotal_Count = (from temp in bookBalanceList
                                   select temp.PITotalCount).ToList();

                pICountList = "'" + string.Join("','", PIcountHead) + "'";


                pItotalCountList = string.Join(",", PItotal_Count);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return bookBalanceList;
        }


        public POCountViewModel GetDashboard_TodayPOSumAmount(int companyId,int companyBranchId, int userId, int reportingUserId, int reportingRoleId)
        {
            POCountViewModel pOCountViewModel = new POCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtcustomerList = sqldbinterface.GetDashboard_TodayPOSumAmount(companyId, companyBranchId, userId, reportingUserId, reportingRoleId);

                if (dtcustomerList != null && dtcustomerList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtcustomerList.Rows)
                    {
                        pOCountViewModel = new POCountViewModel
                        {
                            TodayPOSumAmount = Convert.ToString(dr["TodayPOSum"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pOCountViewModel;
        }

        public PICountViewModel GetDashboard_TodayPISumAmount(int companyId,int companyBranchId, int userId, int reportingUserId, int reportingRoleId)
        {
            PICountViewModel pICountViewModel = new PICountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtPIList = sqldbinterface.GetDashboard_TodayPISumAmount(companyId, companyBranchId, userId, reportingUserId, reportingRoleId);

                if (dtPIList != null && dtPIList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPIList.Rows)
                    {
                        pICountViewModel = new PICountViewModel
                        {
                            TodayPISumAmount = Convert.ToString(dr["TodayPISum"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pICountViewModel;
        }


        public string GetPendingPOCountList(int companyId,int companyBranchId, int userId, int reportingUserId, int reportingRoleId, int finYearId)
        {
            POCountViewModel PO = new POCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            string TotalPendingPOCount = "";
            try
            {
                DataTable dtPOList = sqldbinterface.GetPendingPOCountList(companyId, companyBranchId, userId, reportingUserId, reportingRoleId, finYearId);

                if (dtPOList != null && dtPOList.Rows.Count > 0)
                {

                    TotalPendingPOCount = Convert.ToString(dtPOList.Rows[0]["PendingApprovalCount"]);


                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return TotalPendingPOCount;
        }

        public List<POCountViewModel> GetPendingPOList(int companyId, int companyBranchId, int finYearId)
        {
            List<POCountViewModel> pOCountList = new List<POCountViewModel>();
            SQLDbInterface sqldbinterface = new SQLDbInterface();           
            try
            {
                DataTable dtPOList = sqldbinterface.GetPendingPOList(companyId, companyBranchId, finYearId);

                if (dtPOList != null && dtPOList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPOList.Rows)
                    {
                        pOCountList.Add(new POCountViewModel
                        {
                            POID = Convert.ToInt32(dr["POID"]),
                            PONO = Convert.ToString(dr["PONo"]),
                            Status = Convert.ToString(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pOCountList;
        }

        public string GetPendingPQCountList(int companyId, int companyBranchId,  int finYearId)
        {
            POCountViewModel PO = new POCountViewModel();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            string TotalPQCount = "";
            try
            {
                DataTable dtPOList = sqldbinterface.GetPendingPQCountList(companyId, companyBranchId, finYearId);

                if (dtPOList != null && dtPOList.Rows.Count > 0)
                {

                    TotalPQCount = Convert.ToString(dtPOList.Rows[0]["PQCount"]);


                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return TotalPQCount;
        }

        public List<PurchaseDashboardItemViewModel> GetPurchaseDashboardItems(int roleId, int companyId, int companyBranchId, int finYearId)
        {
            List<PurchaseDashboardItemViewModel> saleDashboardItems = new List<PurchaseDashboardItemViewModel>();
            SQLDbInterface sqldbinterface = new SQLDbInterface();
            try
            {
                DataTable dtPurchaseDashboardItemList = sqldbinterface.PurchaseDashboardItems(roleId, companyId, companyBranchId, finYearId);

                if (dtPurchaseDashboardItemList != null && dtPurchaseDashboardItemList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPurchaseDashboardItemList.Rows)
                    {
                        saleDashboardItems.Add(new PurchaseDashboardItemViewModel
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
            return saleDashboardItems;
        }

        public List<Container9ViewModel> GetPurchaseDashboardPOList(int roleId, int companyId, int companyBranchId, int finYearId, int boxnumber)
        {
            List<Container9ViewModel> containerList = new List<Container9ViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtContainerList = sqlDbInterface.PurchaseDashboardPOItems(roleId, companyId, companyBranchId, finYearId, boxnumber);
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








