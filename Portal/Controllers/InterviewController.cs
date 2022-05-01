using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices;
using System.IO;
using System.Data;
using System.Text;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class InterviewController : BaseController
    {
        //
        // GET: /Interview/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Interview, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditInterview(int interviewId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                if (interviewId != 0)
                {
                    ViewData["interviewId"] = interviewId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["interviewId"] = 0;
                    ViewData["accessMode"] = 3;
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }


        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Interview, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditInterview(InterviewViewModel interviewViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            InterviewBL interviewBL = new InterviewBL();
            try
            {


                if (interviewViewModel != null)
                {
                    interviewViewModel.CreatedBy = ContextUser.UserId;
                    interviewViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = interviewBL.AddEditInterview(interviewViewModel);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                    responseOut.trnId = 0;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetInterviewDetail(long interviewId)
        {
            InterviewBL interviewBL = new InterviewBL();
            InterviewViewModel interview = new InterviewViewModel();
            try
            {
                interview = interviewBL.GetInterviewDetail(interviewId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(interview, JsonRequestBehavior.AllowGet);
        }


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Interview, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListInterview()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpGet]
        public PartialViewResult GetInterviewList(string interviewNo="", string applicantNo="", string interviewFinalStatus="", int companyId=0, string fromDate="", string toDate="",string companyBranch="")
        {
            List<InterviewViewModel> interviews = new List<InterviewViewModel>();
           
            InterviewBL interviewBL = new InterviewBL();
            try
            {
                interviews = interviewBL.GetInterviewList(interviewNo, applicantNo, interviewFinalStatus, ContextUser.CompanyId, fromDate, toDate, companyBranch);
               
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(interviews);
        }

        [HttpGet]
        public PartialViewResult GetApplicantList(string applicantNo, string projectNo, string firstName, string lastName, Int32 resumeSource, Int32 designation, string fromDate, string toDate, string applicantStatus = "Final")
        {
            List<ApplicantViewModel> applicants = new List<ApplicantViewModel>();
            ApplicantBL applicantBL = new ApplicantBL();
            try
            {
                applicants = applicantBL.GetApplicantList(applicantNo, projectNo, firstName, lastName, resumeSource, designation, fromDate, toDate, ContextUser.CompanyId, applicantStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(applicants);
        }

    }
}
