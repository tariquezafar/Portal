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
    public class EmployeeLeaveApplicationController : BaseController
    {
        #region Employee Leave Application
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeLeaveApplication, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeLeaveApplication(int applicationId = 0, int accessMode = 3, int essEmployeeId = 0, string essEmployeeName = "")
        {
            try
            {
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (applicationId != 0)
                {
                    ViewData["essEmployeeId"] = essEmployeeId;
                    ViewData["essEmployeeName"] = essEmployeeName;

                    ViewData["applicationId"] = applicationId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["essEmployeeId"] = essEmployeeId;
                    ViewData["essEmployeeName"] = essEmployeeName;

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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeLeaveApplication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditEmployeeLeaveApplication(EmployeeLeaveApplicationViewModel employeeleaveApplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeLeaveApplicationBL employeeleaveAppBL= new EmployeeLeaveApplicationBL();
            try
            {
                if (employeeleaveApplicationViewModel != null)
                {
                    employeeleaveApplicationViewModel.CompanyId = ContextUser.CompanyId; 
                    responseOut = employeeleaveAppBL.AddEditEmployeeLeaveApplication(employeeleaveApplicationViewModel); 

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeLeaveApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeLeaveApplication(int essEmployeeId = 0, string essEmployeeName = "")
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

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
        public PartialViewResult GetEmployeeLeaveApplicationList(string applicationNo = "", int employeeId = 0, string leaveTypeId = "", string leaveStatus = "0", string fromDate = "", string toDate = "", int essEmployeeId = 0, string essEmployeeName = "",int companyBranchId=0)
        {
            List<EmployeeLeaveApplicationViewModel> employeeleaveApplications = new List<EmployeeLeaveApplicationViewModel>();
            EmployeeLeaveApplicationBL employeeleaveAppBL = new EmployeeLeaveApplicationBL();
            try
            {
                ViewData["essEmployeeId"] = essEmployeeId;
                ViewData["essEmployeeName"] = essEmployeeName;
                employeeleaveApplications = employeeleaveAppBL.GetEmployeeLeaveApplicationList(applicationNo, essEmployeeId, leaveTypeId, leaveStatus,fromDate, toDate, ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeleaveApplications);
        }

        

        [HttpGet]
        public JsonResult GetEmployeeLeaveApplicationDetail(long applicationId)
        {
            EmployeeLeaveApplicationBL employeeleaveAppBL = new EmployeeLeaveApplicationBL();
            EmployeeLeaveApplicationViewModel employeeleaveapplication = new EmployeeLeaveApplicationViewModel();
            try
            {
                employeeleaveapplication = employeeleaveAppBL.GetEmployeeLeaveApplicationDetail(applicationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employeeleaveapplication, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetLeaveTypeForEmpolyeeLeaveAppList()
        {
            LeaveTypeBL leavetypeBL = new LeaveTypeBL();
            List<HR_LeaveTypeViewModel> leaveapplication = new List<HR_LeaveTypeViewModel>();
            try
            {
                leaveapplication = leavetypeBL.GetLeaveTypeForEmpolyeeLeaveAppList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(leaveapplication, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetEmployeeLeaveBalanceList(int employeeId = 0)
        {
            List<EmployeeLeaveDetailViewmodel> employeeLeaveDetails = new List<EmployeeLeaveDetailViewmodel>();
            EmployeeLeaveDetailBL employeeLeaveDetailBL = new EmployeeLeaveDetailBL();
            try
            {

                employeeLeaveDetails = employeeLeaveDetailBL.GetEmployeeLeaveBalanceList(employeeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeLeaveDetails);
        }

        #endregion
        #region Employee Leave Application Approval
        [ValidateRequest(true, UserInterfaceHelper.ApproveEmployeeLeaveApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeLeaveApplicationApproval()
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
        public PartialViewResult GetEmployeeLeaveApplicationApprovalList(string applicationNo = "", int employeeId = 0, string leaveTypeId = "", string leaveStatus = "0", string fromDate = "", string toDate = "",int companyBranchId=0)
        {
            List<EmployeeLeaveApplicationViewModel> employeeleaveApplications = new List<EmployeeLeaveApplicationViewModel>();
            EmployeeLeaveApplicationBL employeeleaveAppBL = new EmployeeLeaveApplicationBL();
            try
            {
                employeeleaveApplications = employeeleaveAppBL.GetEmployeeLeaveApplicationApprovalList(applicationNo, employeeId, leaveTypeId, leaveStatus, fromDate, toDate, ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeleaveApplications);
        }
        [ValidateRequest(true, UserInterfaceHelper.ApproveEmployeeLeaveApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ApproveEmployeeLeaveApplication(int applicationId = 0, int accessMode = 3)
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
        [ValidateRequest(true, UserInterfaceHelper.ApproveEmployeeLeaveApplication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult ApproveEmployeeLeaveApplication(EmployeeLeaveApplicationViewModel employeeleaveApplicationViewModel, EmployeeLeaveDetailViewmodel employeeLeaveDetailViewmodel, string fromDate, string toDate)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeLeaveApplicationBL employeeleaveAppBL = new EmployeeLeaveApplicationBL();
            try
            {
                if (employeeleaveApplicationViewModel != null)
                {
                    employeeleaveApplicationViewModel.CompanyId = ContextUser.CompanyId;
                    employeeleaveApplicationViewModel.ApproveBy = ContextUser.UserId;

                    
                    responseOut = employeeleaveAppBL.ApproveRejectEmployeeLeaveApplication(employeeleaveApplicationViewModel);

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
