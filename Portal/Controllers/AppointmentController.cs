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
    public class AppointmentController : BaseController
    {
        #region Appointment

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Appointment, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditAppointment(int appointmentId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;

                if (appointmentId != 0)
                {

                    ViewData["appointmentId"] = appointmentId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["appointLetterId"] = 0;
                    ViewData["accessMode"] = 0;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Appointment, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditAppointment(AppointmentViewModel appointmentViewModel, AppointmentCTCViewModel appointmentCTCViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            AppointmentBL appointmentBL = new AppointmentBL();
            try
            {
                if (appointmentViewModel != null)
                {
                    appointmentViewModel.CompanyId = ContextUser.CompanyId;
                    appointmentViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = appointmentBL.AddEditAppointment(appointmentViewModel, appointmentCTCViewModel);
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
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAppointmentDetail(long appointLetterId)
        {
            
            AppointmentViewModel appointment = new AppointmentViewModel();
            AppointmentBL appointmentBL = new AppointmentBL();
            try
            {
                appointment = appointmentBL.GetAppointmentDetail(appointLetterId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(appointment, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAppointmentCTCDetail(long appointLetterId)
        {

            AppointmentCTCViewModel appointmentCTC = new AppointmentCTCViewModel();
            AppointmentBL appointmentBL = new AppointmentBL();
            try
            {
                appointmentCTC = appointmentBL.GetAppointmentCTCDetail(appointLetterId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(appointmentCTC, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetApplicantDetailForAppoint(long interviewId)
        {

            AppointmentViewModel appointment = new AppointmentViewModel();
            AppointmentBL appointmentBL = new AppointmentBL();
            try
            {
                appointment = appointmentBL.GetApplicantDetailForAppoint(interviewId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(appointment, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetApplicantCTCDetailForAppoint(long interviewId)
        {

            AppointmentCTCViewModel appointmentCTC = new AppointmentCTCViewModel();
            AppointmentBL appointmentBL = new AppointmentBL();
            try
            {
                appointmentCTC = appointmentBL.GetApplicantCTCDetailForAppoint(interviewId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(appointmentCTC, JsonRequestBehavior.AllowGet);
        }


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Appointment, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListAppointment()
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
        public PartialViewResult GetAppointmentList(string appointLetterNo="", string interviewNo="", string applicantName="", string appointStatus="", int companyId=0, string fromDate="", string toDate="",string companyBranch="")
        {
            List<AppointmentViewModel> appointments = new List<AppointmentViewModel>();
            AppointmentBL appointmentBL = new AppointmentBL();
           
            try
            {
                appointments = appointmentBL.GetAppointmentList(appointLetterNo, interviewNo, applicantName, appointStatus, ContextUser.CompanyId, fromDate, toDate, companyBranch);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(appointments);
        }

        [HttpGet]
        public PartialViewResult GetInterviewList(string interviewNo = "", string applicantNo = "", string interviewFinalStatus = "Final", int companyId = 0, string fromDate = "", string toDate = "",string companyBranch="")
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

        #endregion

    }
}
