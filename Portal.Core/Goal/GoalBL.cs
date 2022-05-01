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
using Portal.DAL.Infrastructure;
namespace Portal.Core
{
    public class GoalBL
    {
        HRMSDBInterface dbInterface;
        public GoalBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditGoal(PMSGoalViewModel pMSGoalViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_PMS_Goal pMSGoal = new HR_PMS_Goal
                {
                    GoalId = pMSGoalViewModel.GoalId,
                    GoalName = pMSGoalViewModel.GoalName,
                    GoalDescription = pMSGoalViewModel.GoalDescription,
                    SectionId = pMSGoalViewModel.SectionId,
                    GoalCategoryId = pMSGoalViewModel.GoalCategoryId,
                    PerformanceCycleId = pMSGoalViewModel.PerformanceCycleId,
                    CompanyId = pMSGoalViewModel.CompanyId,
                    StartDate = Convert.ToDateTime(pMSGoalViewModel.StartDate),
                    DueDate = Convert.ToDateTime(pMSGoalViewModel.DueDate),
                    Weight = pMSGoalViewModel.Weight,
                    CreatedBy = pMSGoalViewModel.CreatedBy,
                    FinYearId = pMSGoalViewModel.FinYearId,
                    Status = pMSGoalViewModel.GoalStatus,
                    CompanyBranchId=pMSGoalViewModel.CompanyBranchId

                };
                responseOut = dbInterface.AddEditGoal(pMSGoal);
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
       
        public List<PMSGoalViewModel> GetGoalList(string goalName, int sectionId, int goalCategoryId, int performanceCycleId, string goalStatus, string fromDate, string toDate, int companyId,int companyBranchId)
        {
            List<PMSGoalViewModel> pMSGoalViewModel = new List<PMSGoalViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtPMSGoal = sqlDbInterface.GetGoalList(goalName, sectionId, goalCategoryId, performanceCycleId, goalStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranchId);
                if (dtPMSGoal != null && dtPMSGoal.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPMSGoal.Rows)
                    {
                        pMSGoalViewModel.Add(new PMSGoalViewModel
                        {
                            GoalId = Convert.ToInt32(dr["GoalId"]),
                            GoalName = Convert.ToString(dr["GoalName"]),
                            GoalCategoryId = Convert.ToInt32(dr["GoalCategoryId"]),
                            GoalCategoryName= Convert.ToString(dr["GoalCategoryName"]),
                            GoalDescription = Convert.ToString(dr["GoalDescription"]), 
                            SectionId = Convert.ToInt32(dr["SectionId"]),
                            SectionName = Convert.ToString(dr["SectionName"]),
                            PerformanceCycleId = Convert.ToInt32(dr["PerformanceCycleId"]),
                            PerformanceCycleName = Convert.ToString(dr["PerformanceCycleName"]),
                            StartDate = Convert.ToString(dr["StartDate"]), 
                            DueDate = Convert.ToString(dr["DueDate"]),
                            Weight = Convert.ToInt32(dr["Weight"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByUserName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            GoalStatus = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pMSGoalViewModel;
        }

        public PMSGoalViewModel GetGoalDetail(int goalId = 0)
        {
            PMSGoalViewModel pMSGoalViewModel = new PMSGoalViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtGoal = sqlDbInterface.GetGoalDetail(goalId);
                if (dtGoal != null && dtGoal.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGoal.Rows)
                    {
                        pMSGoalViewModel = new PMSGoalViewModel
                        {
                            GoalId = Convert.ToInt32(dr["GoalId"]),
                            GoalCategoryId = Convert.ToInt32(dr["GoalCategoryId"]),
                            GoalName = Convert.ToString(dr["GoalName"]),
                            GoalDescription = Convert.ToString(dr["GoalDescription"]),
                            SectionId = Convert.ToInt32(dr["SectionId"]),
                            PerformanceCycleId = Convert.ToInt32(dr["PerformanceCycleId"]),
                            StartDate = Convert.ToString(dr["StartDate"]),
                            DueDate = Convert.ToString(dr["DueDate"]),
                            Weight = Convert.ToInt32(dr["Weight"]),
                            GoalStatus = Convert.ToBoolean(dr["Status"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByUserName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByUserName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchId= Convert.ToInt32(dr["CompanyBranchId"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pMSGoalViewModel;
        }

        public List<PMSGoalCategoryViewModel> GetPMSGoalCategoryList()
        {
            List<PMSGoalCategoryViewModel> pMSGoalCategoryViewModel = new List<PMSGoalCategoryViewModel>();
            try
            {
                List<HR_PMS_GoalCategory> pMSGoalCategoryList = dbInterface.GetPMSGoalCategoryList();
                if (pMSGoalCategoryList != null && pMSGoalCategoryList.Count > 0)
                {
                    foreach (HR_PMS_GoalCategory advance in pMSGoalCategoryList)
                    {
                        pMSGoalCategoryViewModel.Add(new PMSGoalCategoryViewModel { GoalCategoryId = advance.GoalCategoryId, GoalCategoryName = advance.GoalCategoryName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pMSGoalCategoryViewModel;
        }





        public List<PMSGoalViewModel> GetTemplateGoalList(string goalName, int sectionId, int goalCategoryId, int performanceCycleId,string fromDate, string toDate, int companyId)
        {
            List<PMSGoalViewModel> pmsgoals = new List<PMSGoalViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtgoals = sqlDbInterface.GetTemplateGoalList(goalName, sectionId, goalCategoryId, performanceCycleId,Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtgoals != null && dtgoals.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtgoals.Rows)
                    {
                        pmsgoals.Add(new PMSGoalViewModel
                        {
                            GoalId = Convert.ToInt32(dr["GoalId"]),
                            GoalName = Convert.ToString(dr["GoalName"]),
                            GoalCategoryId = Convert.ToInt32(dr["GoalCategoryId"]),
                            GoalCategoryName = Convert.ToString(dr["GoalCategoryName"]),
                            GoalDescription = Convert.ToString(dr["GoalDescription"]),
                            SectionId = Convert.ToInt32(dr["SectionId"]),
                            SectionName = Convert.ToString(dr["SectionName"]),
                            PerformanceCycleId = Convert.ToInt32(dr["PerformanceCycleId"]),
                            PerformanceCycleName = Convert.ToString(dr["PerformanceCycleName"]),
                            StartDate = Convert.ToString(dr["StartDate"]),
                            DueDate = Convert.ToString(dr["DueDate"]),
                            Weight = Convert.ToInt32(dr["Weight"]),
                            GoalStatus = Convert.ToBoolean(dr["Status"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByUserName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByUserName"]),
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
            return pmsgoals;
        }


        public List<PMSGoalViewModel> GetGoalAutoCompleteList(string searchTerm, int companyId)
        {
            List<PMSGoalViewModel> pMSGoalViewModel = new List<PMSGoalViewModel>();
            try
            {
                List<HR_PMS_Goal> goalList = dbInterface.GetGoalAutoCompleteList(searchTerm, companyId);

                if (goalList != null && goalList.Count > 0)
                {
                    foreach (HR_PMS_Goal goal in goalList)
                    {
                        pMSGoalViewModel.Add(new PMSGoalViewModel { GoalId = goal.GoalId, GoalName = goal.GoalName, GoalDescription = goal.GoalDescription, SectionId = Convert.ToInt32(goal.SectionId), GoalCategoryId = Convert.ToInt32(goal.GoalCategoryId), Weight = Convert.ToInt32(goal.Weight),
                            PerformanceCycleId = Convert.ToInt32(goal.PerformanceCycleId), FinYearId = Convert.ToInt32(goal.FinYearId), StartDate =Convert.ToDateTime(goal.StartDate).ToString("dd-MMM-yyyy"), DueDate = Convert.ToDateTime(goal.DueDate).ToString("dd-MMM-yyyy"), GoalStatus =Convert.ToBoolean( goal.Status)  });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pMSGoalViewModel;
        }



    }
}
