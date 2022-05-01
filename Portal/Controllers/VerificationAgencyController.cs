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
    public class VerificationAgencyController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_VerificationAgency, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditVerificationAgency(int verificationagencyId = 0, int accessMode = 3)
        {

            try
            {
                if (verificationagencyId != 0)
                {
                    ViewData["verificationagencyId"] = verificationagencyId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["verificationagencyId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_VerificationAgency, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditVerificationAgency(HR_VerificationAgencyViewModel verificationagencyViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            VerificationAgencyBL verificationagencyBL = new VerificationAgencyBL();
            try
            {
                if (verificationagencyViewModel != null)
                {
                    responseOut = verificationagencyBL.AddEditVerificationAgency(verificationagencyViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_VerificationAgency, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListVerificationAgency()
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
        public PartialViewResult GetVerificationAgencyList(string verificationagencyName = "", string verificationagencyStatus = "")
        {
            List<HR_VerificationAgencyViewModel> verificationagencys = new List<HR_VerificationAgencyViewModel>();
            VerificationAgencyBL verificationagencyBL = new VerificationAgencyBL();
            try
            {
                verificationagencys = verificationagencyBL.GetVerificationAgencyList(verificationagencyName, verificationagencyStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(verificationagencys);
        }


        [HttpGet]
        public JsonResult GetVerificationAgencyDetail(int verificationagencyId)
        {
            VerificationAgencyBL verificationagencyBL = new VerificationAgencyBL();
            HR_VerificationAgencyViewModel verificationagency = new HR_VerificationAgencyViewModel();
            try
            {
                verificationagency = verificationagencyBL.GetVerificationAgencyDetail(verificationagencyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(verificationagency, JsonRequestBehavior.AllowGet);
        }

    }
}
