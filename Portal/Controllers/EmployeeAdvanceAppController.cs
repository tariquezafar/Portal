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
    public class EmployeeAdvanceAppController : BaseController
    {
        #region Employee Advance Application
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeAdvanceApplication, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeAdvanceApp(int applicationId = 0, int accessMode = 3, int essEmployeeId = 0, string essEmployeeName = "")
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (applicationId != 0)
                {
                    ViewData["applicationId"] = applicationId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["essEmployeeId"] = essEmployeeId;
                    ViewData["essEmployeeName"] = essEmployeeName;
                }
                else
                {
                    ViewData["applicationId"] = 0;
                    ViewData["accessMode"] = 3;
                    ViewData["essEmployeeId"] = essEmployeeId;
                    ViewData["essEmployeeName"] = essEmployeeName;

                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeAdvanceApplication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditEmployeeAdvanceApp(HR_EmployeeAdvanceApplicationViewModel employeeadvanceApplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeAdvanceAppBL employeeadvanceAppBL= new EmployeeAdvanceAppBL();
            try
            {
                if (employeeadvanceApplicationViewModel != null)
                {
                    employeeadvanceApplicationViewModel.CompanyId = ContextUser.CompanyId;
                    employeeadvanceApplicationViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = employeeadvanceAppBL.AddEditEmployeeAdvanceApp(employeeadvanceApplicationViewModel); 

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeAdvanceApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeAdvanceApp(int essEmployeeId = 0, string essEmployeeName = "")
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["essEmployeeId"] = essEmployeeId;
                ViewData["essEmployeeName"] = essEmployeeName;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpGet]
        public PartialViewResult GetEmployeeAdvanceAppList(string applicationNo = "", int employeeId = 0, string advanceTypeId = "",  string advanceStatus = "0", string fromDate = "", string toDate = "", int essEmployeeId = 0, string essEmployeeName = "",string companyBranch="")
        {
            List<HR_EmployeeAdvanceApplicationViewModel> employeeadvaneApplications = new List<HR_EmployeeAdvanceApplicationViewModel>();
            EmployeeAdvanceAppBL employeeadvanceAppBL = new EmployeeAdvanceAppBL();
            try
            {
                ViewData["essEmployeeId"] = essEmployeeId;
                ViewData["essEmployeeName"] = essEmployeeName;
                employeeadvaneApplications = employeeadvanceAppBL.GetEmployeeAdvanceAppList(applicationNo, essEmployeeId, advanceTypeId,advanceStatus, fromDate, toDate, ContextUser.CompanyId, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeadvaneApplications);
        }

        [HttpGet]
        public JsonResult GetEmployeeAdvanceAppDetail(long applicationId)
        {
            EmployeeAdvanceAppBL employeeadvanceAppBL = new EmployeeAdvanceAppBL();
            HR_EmployeeAdvanceApplicationViewModel employeeadvanceapplication = new HR_EmployeeAdvanceApplicationViewModel();
            try
            {
                employeeadvanceapplication = employeeadvanceAppBL.GetEmployeeAdvanceAppDetail(applicationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employeeadvanceapplication, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAdvanceTypeForEmpolyeeAdvanceAppList()
        {
            AdvanceTypeBL advancetypeBL = new AdvanceTypeBL();
            List<HR_AdvanceTypeViewModel> advanceapplication = new List<HR_AdvanceTypeViewModel>();
            try
            {
                advanceapplication = advancetypeBL.GetAdvanceTypeForEmpolyeeAdvanceAppList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(advanceapplication, JsonRequestBehavior.AllowGet);
        } 

        #endregion
        #region Employee Advance Application Approval
        [ValidateRequest(true, UserInterfaceHelper.ApproveEmployeeAdvanceApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeAdvanceAppApproval()
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
        public PartialViewResult GetEmployeeAdvanceAppApprovalList(string applicationNo = "", int employeeId = 0, string advanceTypeId = "", string advanceStatus = "0", string fromDate = "", string toDate = "")
        {
            List<HR_EmployeeAdvanceApplicationViewModel> employeeadvaneApplications = new List<HR_EmployeeAdvanceApplicationViewModel>();
            EmployeeAdvanceAppBL employeeadvanceAppBL = new EmployeeAdvanceAppBL();
            try
            {
                employeeadvaneApplications = employeeadvanceAppBL.GetEmployeeAdvanceAppApprovalList(applicationNo, employeeId, advanceTypeId, advanceStatus, fromDate, toDate, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeadvaneApplications);
        }
        [ValidateRequest(true, UserInterfaceHelper.ApproveEmployeeAdvanceApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ApproveEmployeeAdvanceApp(int applicationId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

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
        [ValidateRequest(true, UserInterfaceHelper.ApproveEmployeeAdvanceApplication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult ApproveEmployeeAdvanceApp(HR_EmployeeAdvanceApplicationViewModel employeeadvanceApplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeAdvanceAppBL employeeadvanceAppBL = new EmployeeAdvanceAppBL();
            try
            {
                if (employeeadvanceApplicationViewModel != null)
                {
                    employeeadvanceApplicationViewModel.CompanyId = ContextUser.CompanyId;
                    employeeadvanceApplicationViewModel.ApproveBy = ContextUser.UserId;
                    responseOut = employeeadvanceAppBL.ApproveRejectEmployeeAdvanceApp(employeeadvanceApplicationViewModel);

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
