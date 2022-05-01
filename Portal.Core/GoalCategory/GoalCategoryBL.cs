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
    public class GoalCategoryBL
    {
        HRMSDBInterface dbInterface;
        public GoalCategoryBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditGoalCategory(PMSGoalCategoryViewModel pMSGoalCategoryViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_PMS_GoalCategory pMSGoalCategory = new HR_PMS_GoalCategory
                {
                    GoalCategoryId = pMSGoalCategoryViewModel.GoalCategoryId,
                    GoalCategoryName = pMSGoalCategoryViewModel.GoalCategoryName,     
                    Weight=pMSGoalCategoryViewModel.Weight,
                    Status = pMSGoalCategoryViewModel.GoalCategory_Status
                };
                responseOut = dbInterface.AddEditGoalCategory(pMSGoalCategory);
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

        public List<PMSGoalCategoryViewModel> GetGoalCategoryList(string goalCategoryName = "", string Status = "")
        {
            List<PMSGoalCategoryViewModel> pMSGoalCategoryViewModel = new List<PMSGoalCategoryViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dteducation = sqlDbInterface.GetGoalCategoryList(goalCategoryName, Status);
                if (dteducation != null && dteducation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dteducation.Rows)
                    {
                        pMSGoalCategoryViewModel.Add(new PMSGoalCategoryViewModel
                        {
                            GoalCategoryId = Convert.ToInt32(dr["GoalCategoryId"]),
                            GoalCategoryName = Convert.ToString(dr["GoalCategoryName"]),                         
                            GoalCategory_Status = Convert.ToBoolean(dr["Status"])
                        });
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

        public PMSGoalCategoryViewModel GetGoalCategoryDetail(int goalCategoryId = 0)
        {
            PMSGoalCategoryViewModel pMSGoalCategoryViewModel = new PMSGoalCategoryViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGoalCategory = sqlDbInterface.GetGoalCategoryDetail(goalCategoryId);
                if (dtGoalCategory != null && dtGoalCategory.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGoalCategory.Rows)
                    {
                        pMSGoalCategoryViewModel = new PMSGoalCategoryViewModel
                        {
                            GoalCategoryId = Convert.ToInt32(dr["GoalCategoryId"]),
                            GoalCategoryName = Convert.ToString(dr["GoalCategoryName"]),
                            Weight = Convert.ToInt32(dr["Weight"]),
                            GoalCategory_Status = Convert.ToBoolean(dr["Status"])
                        };
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


    }
}
