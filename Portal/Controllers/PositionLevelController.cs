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
    public class PositionLevelController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PositionLevel, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPositionLevel(int positionlevelId = 0, int accessMode = 3)
        {

            try
            {
                if (positionlevelId != 0)
                {
                    ViewData["positionlevelId"] = positionlevelId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["positionlevelId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PositionLevel, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPositionLevel(PositionLevelViewModel positionlevelViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            PositionLevelBL positionlevelBL = new PositionLevelBL();
            try
            {
                if (positionlevelViewModel != null)
                {
                    responseOut = positionlevelBL.AddEditPositionLevel(positionlevelViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PositionLevel, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPositionLevel()
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
        public PartialViewResult GetPositionLevelList(string positionlevelName = "", string positionlevelCode = "", string positionlevelStatus = "")
        {
            List<PositionLevelViewModel> positionlevels = new List<PositionLevelViewModel>();
            PositionLevelBL positionlevelBL = new PositionLevelBL();
            try
            {
                positionlevels = positionlevelBL.GetPositionLevelList(positionlevelName, positionlevelCode, positionlevelStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(positionlevels);
        }


        [HttpGet]
        public JsonResult GetPositionLevelDetail(int positionlevelId)
        {
            PositionLevelBL positionlevelBL = new PositionLevelBL();
            PositionLevelViewModel positionlevel = new PositionLevelViewModel();
            try
            {
                positionlevel = positionlevelBL.GetPositionLevelDetail(positionlevelId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(positionlevel, JsonRequestBehavior.AllowGet);
        }

    }
}
