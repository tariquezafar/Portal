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
    public class InterviewTypeController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_InterviewType, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditInterviewType(int interviewtypeId = 0, int accessMode = 3)
        {

            try
            {
                if (interviewtypeId != 0)
                {
                    ViewData["interviewtypeId"] = interviewtypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["interviewtypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_InterviewType, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditInterviewType(InterviewTypeViewModel interviewtypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            InterviewTypeBL interviewtypeBL = new InterviewTypeBL();
            try
            {
                if (interviewtypeViewModel != null)
                {
                    responseOut = interviewtypeBL.AddEditInterviewType(interviewtypeViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_InterviewType, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListInterviewType()
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
        public PartialViewResult GetInterviewTypeList(string interviewtypeName = "", string interviewtypeStatus = "")
        {
            List<InterviewTypeViewModel> interviewtypes = new List<InterviewTypeViewModel>();
            InterviewTypeBL interviewtypeBL = new InterviewTypeBL();
            try
            {
                interviewtypes = interviewtypeBL.GetInterviewTypeList(interviewtypeName, interviewtypeStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(interviewtypes);
        }


        [HttpGet]
        public JsonResult GetInterviewTypeDetail(int interviewtypeId)
        {
            InterviewTypeBL interviewtypeBL = new InterviewTypeBL();
            InterviewTypeViewModel interviewtype = new InterviewTypeViewModel();
            try
            {
                interviewtype = interviewtypeBL.GetInterviewTypeDetail(interviewtypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(interviewtype, JsonRequestBehavior.AllowGet);
        }

    }
}
