using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;



namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class PMSGoalController : BaseController
    {
        
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Goal, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditGoal(int goalId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                if (goalId != 0)
                {
                    ViewData["goalId"] = goalId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["goalId"] = 0;
                    ViewData["accessMode"] = 0;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Goal, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditGoal(PMSGoalViewModel pMSGoalViewModel)
        {
            ResponseOut responseOut = new ResponseOut();                    
            GoalBL goalBL = new GoalBL();
            
            try
            {
               
                if (pMSGoalViewModel != null)
                {
                    pMSGoalViewModel.CreatedBy = ContextUser.UserId;
                    pMSGoalViewModel.CompanyId = ContextUser.CompanyId;
                    pMSGoalViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = goalBL.AddEditGoal(pMSGoalViewModel);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                }

            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Goal, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListGoal()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult GetGoalList(string goalName = "", int sectionId = 0, int goalCategoryId = 0, int performanceCycleId = 0, string goalStatus = "", string fromDate = "", string toDate = "",int companyBranchId=0)
        {
            List<PMSGoalViewModel> pMSGoalViewModel = new List<PMSGoalViewModel>();
            GoalBL goalBL = new GoalBL();           
            try
            {
                pMSGoalViewModel = goalBL.GetGoalList(goalName, sectionId, goalCategoryId, performanceCycleId, goalStatus, fromDate, toDate, ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pMSGoalViewModel);
        }     
        [HttpGet]
        public JsonResult GetGoalDetail(int goalId)
        {

            GoalBL goalBL = new GoalBL();
            PMSGoalViewModel pMSGoalViewModel = new PMSGoalViewModel();
            try
            {
                pMSGoalViewModel = goalBL.GetGoalDetail(goalId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(pMSGoalViewModel, JsonRequestBehavior.AllowGet);
        } 

        [HttpGet]
        public JsonResult GetPMSGoalCategoryList()
        {
            GoalCategoryBL goalCategoryBL = new GoalCategoryBL();
            List<PMSGoalCategoryViewModel> pMSGoalCategoryViewModel = new List<PMSGoalCategoryViewModel>();

            try
            {
                pMSGoalCategoryViewModel = goalCategoryBL.GetPMSGoalCategoryList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(pMSGoalCategoryViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPMSSectionList()
        {
            PMS_SectionBL pMS_SectionBL = new PMS_SectionBL();       
            List<PMS_SectionViewModel> pMSSectionViewModel = new List<PMS_SectionViewModel>();

            try
            {
                pMSSectionViewModel = pMS_SectionBL.GetPMSSectionList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(pMSSectionViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPMSPerformanceCycleList()
        {
            PMS_PerformanceCycleBL pMS_PerformanceCycleBL = new PMS_PerformanceCycleBL();
           
            List<PMS_PerformanceCycleViewModel> pMSPerformanceCycleViewModel = new List<PMS_PerformanceCycleViewModel>();

            try
            {
                pMSPerformanceCycleViewModel = pMS_PerformanceCycleBL.GetPMSPerformanceCycleList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(pMSPerformanceCycleViewModel, JsonRequestBehavior.AllowGet);
        }

      

    }
}
