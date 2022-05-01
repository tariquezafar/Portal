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
    public class SeparationApplicationController : BaseController
    {
        #region Separation Application
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SeparationApplication, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSeparationApplication(int applicationId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (applicationId != 0)
                {
                    ViewData["applicationId"] = applicationId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["applicationId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SeparationApplication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditSeparationApplication(SeparationApplicationViewModel separationApplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SeparationApplicationBL separationapplicationBL = new SeparationApplicationBL();
            try
            {
                if (separationApplicationViewModel != null)
                {
                    separationApplicationViewModel.CompanyId = ContextUser.CompanyId;
                    separationApplicationViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = separationapplicationBL.AddEditSeparationApplication(separationApplicationViewModel); 

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SeparationApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSeparationApplication()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

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
        public PartialViewResult GetSeparationApplicationList(string applicationNo = "", int employeeId = 0, string separationcategoryId = "",  string applicationStatus = "0", string fromDate = "", string toDate = "",int companyBranchId=0)
        {
            List<SeparationApplicationViewModel> separationApplications = new List<SeparationApplicationViewModel>();
            SeparationApplicationBL separationApplicationBL = new SeparationApplicationBL();
            try
            {
                separationApplications = separationApplicationBL.GetSeparationApplicationList(applicationNo, employeeId, separationcategoryId, applicationStatus, fromDate, toDate, ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(separationApplications);
        }

        [HttpGet]
        public JsonResult GetSeparationApplicationDetail(long applicationId)
        {
            SeparationApplicationBL separationApplicationBL = new SeparationApplicationBL();
            SeparationApplicationViewModel separationApplications = new SeparationApplicationViewModel();
            try
            {
                separationApplications = separationApplicationBL.GetSeparationApplicationDetail(applicationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(separationApplications, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSeparationCategoryForSeparationApplicationList()
        {
            SeparationCategoryBL separationcategoryBL = new SeparationCategoryBL();
            List<SeparationCategoryViewModel> separationcategory = new List<SeparationCategoryViewModel>(); 
            try
            {
                separationcategory = separationcategoryBL.GetSeparationCategoryForSeparationApplicationList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(separationcategory, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Separation Approval
        [ValidateRequest(true, UserInterfaceHelper.ApproveSeparationApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSeparationApplicationApproval()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
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
        public PartialViewResult GetSeparationApplicationApprovalList(string applicationNo = "", int employeeId = 0, string separationcategoryId = "", string applicationStatus = "0", string fromDate = "", string toDate = "",int companyBranchId=0)
        {
            List<SeparationApplicationViewModel> separationApplications = new List<SeparationApplicationViewModel>();
            SeparationApplicationBL separationApplicationBL = new SeparationApplicationBL();
            try
            {
                separationApplications = separationApplicationBL.GetSeparationApplicationApprovalList(applicationNo, employeeId, separationcategoryId, applicationStatus, fromDate, toDate, ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(separationApplications);
        }
        [ValidateRequest(true, UserInterfaceHelper.ApproveSeparationApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ApproveSeparationApplication(int applicationId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (applicationId != 0)
                {
                    ViewData["applicationId"] = applicationId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["applicationId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.ApproveSeparationApplication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult ApproveSeparationApplication(SeparationApplicationViewModel separationApplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SeparationApplicationBL separationapplicationBL = new SeparationApplicationBL();
            try
            {
                if (separationApplicationViewModel != null)
                {
                    separationApplicationViewModel.CompanyId = ContextUser.CompanyId;
                    separationApplicationViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = separationapplicationBL.ApproveRejectSeparationApplication(separationApplicationViewModel);

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


        #endregion

    }
}
