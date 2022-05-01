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
    public class ResumeSourceController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ResumeSource, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditResumeSource(int resumesourceId = 0, int accessMode = 3)
        {

            try
            {
                if (resumesourceId != 0)
                {
                    ViewData["resumesourceId"] = resumesourceId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["resumesourceId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ResumeSource, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditResumeSource(HR_ResumeSourceViewModel resumesourceViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ResumeSourceBL resumesourceBL = new ResumeSourceBL();
            try
            {
                if (resumesourceViewModel != null)
                {
                    responseOut = resumesourceBL.AddEditResumeSource(resumesourceViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ResumeSource, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListResumeSource()
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
        public PartialViewResult GetResumeSourceList(string resumesourceName = "", string resumesourceStatus = "")
        {
            List<HR_ResumeSourceViewModel> resumesources = new List<HR_ResumeSourceViewModel>();
            ResumeSourceBL resumesourceBL = new ResumeSourceBL();
            try
            {
                resumesources = resumesourceBL.GetResumeSourceList(resumesourceName, resumesourceStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(resumesources);
        }


        [HttpGet]
        public JsonResult GetResumeSourceDetail(int resumesourceId)
        {
            ResumeSourceBL resumesourceBL = new ResumeSourceBL();
            HR_ResumeSourceViewModel resumesource = new HR_ResumeSourceViewModel();
            try
            {
                resumesource = resumesourceBL.GetResumeSourceDetail(resumesourceId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(resumesource, JsonRequestBehavior.AllowGet);
        }

    }
}
