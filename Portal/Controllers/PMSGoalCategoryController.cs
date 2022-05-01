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
    public class PMSGoalCategoryController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_GoalCategory_HR, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditGoalCategory(int goalCategoryId = 0, int accessMode = 3)
        {

            try
            {
                if (goalCategoryId != 0)
                {
                    ViewData["goalCategoryId"] = goalCategoryId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["goalCategoryId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_GoalCategory_HR, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditGoalCategory(PMSGoalCategoryViewModel pMSGoalCategoryViewModel)
        {
            ResponseOut responseOut = new ResponseOut();                    
            GoalCategoryBL goalCategoryBL = new GoalCategoryBL();
            try
            {
                if (pMSGoalCategoryViewModel != null)
                {
                    responseOut = goalCategoryBL.AddEditGoalCategory(pMSGoalCategoryViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_GoalCategory_HR, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListGoalCategoryType()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult GetGoalCategoryList(string goalCategoryName = "", string goalCategoryStatus = "")
        {
            List<PMSGoalCategoryViewModel> pMSGoalCategoryViewModel = new List<PMSGoalCategoryViewModel>();
            GoalCategoryBL goalCategoryBL = new GoalCategoryBL();           
            try
            {
                pMSGoalCategoryViewModel = goalCategoryBL.GetGoalCategoryList(goalCategoryName, goalCategoryStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pMSGoalCategoryViewModel);
        }     
        [HttpGet]
        public JsonResult GetGoalCategoryDetail(int goalCategoryId)
        {


            GoalCategoryBL goalCategoryBL = new GoalCategoryBL();
            PMSGoalCategoryViewModel pMSGoalCategoryViewModel = new PMSGoalCategoryViewModel();
            try
            {
                pMSGoalCategoryViewModel = goalCategoryBL.GetGoalCategoryDetail(goalCategoryId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(pMSGoalCategoryViewModel, JsonRequestBehavior.AllowGet);
        }

    }
}
