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
    public class ExitInterviewController : BaseController
    {
        #region Exit Interview
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ExitInterview, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditExitInterview(int exitinterviewId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (exitinterviewId != 0)
                {
                    ViewData["exitinterviewId"] = exitinterviewId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["exitinterviewId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ExitInterview, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditExitInterview(ExitInterviewViewModel exitinterviewViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ExitInterviewBL exittnterviewBL = new ExitInterviewBL();
            try
            {
                if (exitinterviewViewModel != null)
                {
                    exitinterviewViewModel.CompanyId = ContextUser.CompanyId;
                    exitinterviewViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = exittnterviewBL.AddEditExitInterview(exitinterviewViewModel); 

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ExitInterview, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListExitInterview()
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
        public PartialViewResult GetExitInterviewList(string exitinterviewNo = "", int employeeId = 0, int applicationId = 0, string interviewStatus = "", int interviewbyuserId = 0, string fromDate = "", string toDate = "")
        {
            List<ExitInterviewViewModel> exitinterviews = new List<ExitInterviewViewModel>();
            ExitInterviewBL exitInterviewBL = new ExitInterviewBL();
            try
            {
                exitinterviews = exitInterviewBL.GetExitInterviewList(exitinterviewNo, employeeId, applicationId, interviewStatus, interviewbyuserId, fromDate, toDate, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(exitinterviews);
        }

        [HttpGet]
        public JsonResult GetExitInterviewDetail(long exitinterviewId)
        {
            ExitInterviewBL exitInterviewBL = new ExitInterviewBL();
            ExitInterviewViewModel exitinterviews = new ExitInterviewViewModel();
            try
            {
                exitinterviews = exitInterviewBL.GetExitInterviewDetail(exitinterviewId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(exitinterviews, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSeparationApplicationForExitInterviewList()
        {
           SeparationApplicationBL separationApplicationBL = new SeparationApplicationBL();
           List<SeparationApplicationViewModel> separationApplications = new List<SeparationApplicationViewModel>(); 
            try
            {
                separationApplications = separationApplicationBL.GetSeparationApplicationForExitInterviewList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(separationApplications, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}
