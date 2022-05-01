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
    public class AdvanceTypeController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_AdvanceType, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditAdvanceType(int advancetypeId = 0, int accessMode = 3)
        {

            try
            {
                if (advancetypeId != 0)
                {
                    ViewData["advancetypeId"] = advancetypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["advancetypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_AdvanceType, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditAdvanceType(HR_AdvanceTypeViewModel advancetypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            AdvanceTypeBL advancetypeBL = new AdvanceTypeBL();
            try
            {
                if (advancetypeViewModel != null)
                {
                    responseOut = advancetypeBL.AddEditAdvanceType(advancetypeViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_AdvanceType, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListAdvanceType()
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
        public PartialViewResult GetAdvanceTypeList(string advancetypeName = "", string advancetypeStatus = "")
        {
            List<HR_AdvanceTypeViewModel> advancetypes = new List<HR_AdvanceTypeViewModel>();
            AdvanceTypeBL advancetypeBL = new AdvanceTypeBL();
            try
            {
                advancetypes = advancetypeBL.GetAdvanceTypeList(advancetypeName, advancetypeStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(advancetypes);
        }


        [HttpGet]
        public JsonResult GetAdvanceTypeDetail(int advancetypeId)
        {
            AdvanceTypeBL advancetypeBL = new AdvanceTypeBL();
            HR_AdvanceTypeViewModel advancetype = new HR_AdvanceTypeViewModel();
            try
            {
                advancetype = advancetypeBL.GetAdvanceTypeDetail(advancetypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(advancetype, JsonRequestBehavior.AllowGet);
        }

    }
}
