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
    public class ApplicantController : BaseController
    {
        #region Job Applicant
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Applicant, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditApplicant(int applicantId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (applicantId != 0)
                {
                    ViewData["applicantId"] = applicantId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["applicantId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Applicant, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditApplicant(ApplicantViewModel applicant, List<ApplicantEducationViewModel> educations, List<ApplicantPrevEmployerViewModel> employers, List<ApplicantProjectViewModel> projects, ApplicantExtraActivityViewModel extraActivity)
        {
            ResponseOut responseOut = new ResponseOut();
            ApplicantBL applicantBL = new ApplicantBL();
            try
            {
                if (applicant != null)
                {
                    applicant.CompanyId = ContextUser.CompanyId;
                    applicant.CreatedBy = ContextUser.UserId;
                    responseOut = applicantBL.AddEditApplicant(applicant,educations,employers,projects, extraActivity);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Applicant, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListApplicant()
        {
            try
            {
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

        [HttpPost]
        public PartialViewResult GetApplicantEducationList(List<ApplicantEducationViewModel> educations, long applicantId)
        {
            ApplicantBL applicantBL = new ApplicantBL();
            try
            {
                if (educations == null)
                {
                    educations = applicantBL.GetApplicantEducationList(applicantId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(educations);
        }

        [HttpPost]
        public PartialViewResult GetApplicantPrevEmployerList(List<ApplicantPrevEmployerViewModel> employers, long applicantId)
        {
            ApplicantBL applicantBL = new ApplicantBL();
            try
            {
                if (employers == null)
                {
                    employers = applicantBL.GetApplicantPrevEmployerList(applicantId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employers);
        }

        [HttpPost]
        public PartialViewResult GetApplicantProjectList(List<ApplicantProjectViewModel> projects, long applicantId)
        {
            ApplicantBL applicantBL = new ApplicantBL();
            try
            {
                if (projects == null)
                {
                    projects = applicantBL.GetApplicantProjectList(applicantId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(projects);
        }
        [HttpGet]
        public PartialViewResult GetJobOpeningList(string jobOpeningNo = "", string requisitionNo = "", string jobPortalRefNo = "", string jobTitle = "", string fromDate = "", string toDate = "", int companyId = 0, string jobStatus = "")
        {
            List<JobOpeningViewModel> jobOpenings = new List<JobOpeningViewModel>();
            JobOpeningBL jobOpeningBL = new JobOpeningBL();
            try
            {
                jobOpenings = jobOpeningBL.GetJobOpeningList(jobOpeningNo, requisitionNo, jobPortalRefNo, jobTitle, fromDate, toDate, ContextUser.CompanyId, jobStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(jobOpenings);
        }

        [HttpGet]
        public JsonResult GetApplicantDetail(long applicantId)
        {
            ApplicantBL applicantBL = new ApplicantBL();
            ApplicantViewModel applicant = new ApplicantViewModel();
            try
            {
                applicant = applicantBL.GetApplicantDetail(applicantId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(applicant, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetApplicantExtraActivityDetail(long applicantId)
        {
            ApplicantBL applicantBL = new ApplicantBL();
            ApplicantExtraActivityViewModel applicantActivity = new ApplicantExtraActivityViewModel();
            try
            {
                applicantActivity = applicantBL.GetApplicantExtraActivityDetail(applicantId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(applicantActivity, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Shortlist Job Applicant
        [ValidateRequest(true, UserInterfaceHelper.ShortlistApplicant, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListShortlistApplicant()
        {
            try
            {
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
        public PartialViewResult GetShortlistApplicantList(string applicantNo, string projectNo, string firstName, string lastName, Int32 resumeSource, Int32 designation, string fromDate, string toDate, string applicantShortlistStatus = "Shortlist")
        {
            List<ApplicantViewModel> applicants = new List<ApplicantViewModel>();
            ApplicantBL applicantBL = new ApplicantBL();
            try
            {
                applicants = applicantBL.GetShortlistApplicantList(applicantNo, projectNo, firstName, lastName, resumeSource, designation, fromDate, toDate, ContextUser.CompanyId, applicantShortlistStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(applicants);
        }

        [ValidateRequest(true, UserInterfaceHelper.ShortlistApplicant, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ShortlistApplicant(int applicantId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (applicantId != 0)
                {
                    ViewData["applicantId"] = applicantId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["applicantId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.ShortlistApplicant, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult ShortlistApplicant(ApplicantViewModel applicant, ApplicantVerificationViewModel verification)
        {
            ResponseOut responseOut = new ResponseOut();
            ApplicantBL applicantBL = new ApplicantBL();
            try
            {
                if (applicant != null)
                {
                    applicant.CompanyId = ContextUser.CompanyId;
                    applicant.CreatedBy = ContextUser.UserId;
                    responseOut = applicantBL.ShortlistApplicant(applicant, verification);
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
        public JsonResult GetApplicantVerificationDetail(long applicantId)
        {
            ApplicantBL applicantBL = new ApplicantBL();
            ApplicantVerificationViewModel applicantVerification = new ApplicantVerificationViewModel();
            try
            {
                applicantVerification = applicantBL.GetApplicantVerificationDetail(applicantId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(applicantVerification, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Helper Method
        [HttpGet]
        public JsonResult GetResumeSourceList()
        {
            ResumeSourceBL resumeSourceBL = new ResumeSourceBL();
            List<HR_ResumeSourceViewModel> resumeSourceList = new List<HR_ResumeSourceViewModel>();
            try
            {
                resumeSourceList = resumeSourceBL.GetResumeSourceList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(resumeSourceList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllDesignationList()
        {
            DesignationBL designationBL = new DesignationBL();
            List<DesignationViewModel> designations = new List<DesignationViewModel>();
            try
            {

                designations = designationBL.GetAllDesignationList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(designations, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetEducationList()
        {
            EducationBL educationBL = new EducationBL();
            List<HR_EducationViewModel> educationList = new List<HR_EducationViewModel>();
            try
            {
                educationList = educationBL.GetEducationList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(educationList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetVerificationAgencyList()
        {
            VerificationAgencyBL agencyBL = new VerificationAgencyBL();
            List<HR_VerificationAgencyViewModel> agencyList = new List<HR_VerificationAgencyViewModel>();
            try
            {
                agencyList = agencyBL.GetVerificationAgencyList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(agencyList, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
