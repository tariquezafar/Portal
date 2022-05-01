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
    public class ReligionController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Religion_HR, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditReligion(int religionId = 0, int accessMode = 3)
        {

            try
            {
                if (religionId != 0)
                {
                    ViewData["religionId"] = religionId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["religionId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Religion_HR, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditReligion(HR_ReligionViewModel religionViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ReligionBL religionBL = new ReligionBL();
            try
            {
                if (religionViewModel != null)
                {
                    responseOut = religionBL.AddEditReligion(religionViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Religion_HR, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListReligion()
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
        public PartialViewResult GetReligionList(string religionName = "", string religionStatus = "")
        {
            List<HR_ReligionViewModel> hR_ReligionViewModel = new List<HR_ReligionViewModel>();

            ReligionBL religionBL = new ReligionBL();
            try
            {
                hR_ReligionViewModel = religionBL.GetReligionList(religionName, religionStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(hR_ReligionViewModel);
        }     
        [HttpGet]
        public JsonResult GetReligionDetail(int religionId)
        {
            ReligionBL religionBL = new ReligionBL();
         
            HR_ReligionViewModel hR_ReligionViewModel = new HR_ReligionViewModel();
            try
            {
                hR_ReligionViewModel = religionBL.GetReligionDetail(religionId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(hR_ReligionViewModel, JsonRequestBehavior.AllowGet);
        }

    }
}
