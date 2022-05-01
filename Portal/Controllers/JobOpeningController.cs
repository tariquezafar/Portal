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
    public class JobOpeningController : BaseController
    {
        //
        // GET: /JobOpening/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_JobOpening, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditJobOpening(int jobopeningId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                if (jobopeningId != 0)
                {
                    ViewData["jobopeningId"] = jobopeningId;

                    ViewData["accessMode"] = accessMode;



                }
                else
                {
                    ViewData["jobopeningId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_JobOpening, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditJobOpening(JobOpeningViewModel jobOpeningViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            JobOpeningBL jobOpeningBL = new JobOpeningBL();
            try
            {
            

                if (jobOpeningViewModel!=null)
                {
                    jobOpeningViewModel.CreatedBy = ContextUser.UserId;
                    jobOpeningViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = jobOpeningBL.AddEditJobOpening(jobOpeningViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_JobOpening, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListJobOpening()
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
        public PartialViewResult GetJobOpeningList(string jobOpeningNo="", string requisitionNo="", string jobPortalRefNo="", string jobTitle = "", string fromDate="", string toDate="", int companyId=0,string jobStatus="",string companyBranch="")
        {
            List<JobOpeningViewModel> jobOpenings= new List<JobOpeningViewModel>();
            JobOpeningBL jobOpeningBL = new JobOpeningBL();
            try
            {
                jobOpenings = jobOpeningBL.GetJobOpeningList(jobOpeningNo,requisitionNo, jobPortalRefNo, jobTitle, fromDate, toDate, ContextUser.CompanyId,jobStatus, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(jobOpenings);
        }

        [HttpGet]
        public PartialViewResult GetResourceRequisitionList(string requisitionNo = "", int positionLevelId = 0, string priorityLevel = "", int positionTypeId = 0, int departmentId = 0, string approvalStatus = "Approved", string fromDate = "",string toDate="",int companyBranchId=0)
        {
            List<ResourceRequisitionViewModel> resourceRequisitions = new List<ResourceRequisitionViewModel>();
            ResourceRequisitionBL rrBL = new ResourceRequisitionBL();
            try
            {
                resourceRequisitions = rrBL.GetResourceRequisitionList(requisitionNo, positionLevelId, priorityLevel, positionTypeId, departmentId, approvalStatus, fromDate, toDate,ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(resourceRequisitions);
        }

        [HttpGet]
        public JsonResult GetJobOpeningDetail(long jobOpeningId)
        {
            JobOpeningViewModel jobOpening = new JobOpeningViewModel();
            JobOpeningBL jobOpeningBL = new JobOpeningBL();
            try
            {
                jobOpening = jobOpeningBL.GetJobOpeningDetail(jobOpeningId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(jobOpening, JsonRequestBehavior.AllowGet);
        }
    }
}
