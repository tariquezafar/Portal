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
    public class AttendanceController : BaseController
    {
        #region Mark Attendance
        [ValidateRequest(true, UserInterfaceHelper.MarkAttendance, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult MarkAttendance(int employeeAttendanceId = 0, int accessMode = 1, int essEmployeeId = 0, string essEmployeeName = "")
        {
            try
            {
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                if (employeeAttendanceId != 0)
                {
                    ViewData["employeeAttendanceId"] = employeeAttendanceId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["essEmployeeId"] = essEmployeeId;
                    ViewData["essEmployeeName"] = essEmployeeName;
                }
                else
                {
                    ViewData["employeeAttendanceId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.MarkAttendance, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeMarkAttendance(EmployeeMarkAttendanceViewModel employeeMarkAttendanceViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeMarkAttendanceBL employeeMarkAttendanceBL = new EmployeeMarkAttendanceBL();
            try
            {
                if (employeeMarkAttendanceViewModel != null)
                {
                    employeeMarkAttendanceViewModel.CompanyId = ContextUser.CompanyId;
                    employeeMarkAttendanceViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = employeeMarkAttendanceBL.AddEditEmployeeMarkAttendance(employeeMarkAttendanceViewModel); 
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

        [ValidateRequest(true, UserInterfaceHelper.MarkAttendance, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeMarkAttendance(int essEmployeeId = 0, string essEmployeeName = "")
        {
            try
            {
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
        public PartialViewResult GetEmployeeMarkAttendanceList(int employeeId = 0, string fromDate = "", string toDate = "", int essEmployeeId = 0, string essEmployeeName = "")
        {
            List<EmployeeMarkAttendanceViewModel> employeeMarkAttendance = new List<EmployeeMarkAttendanceViewModel>();
            EmployeeMarkAttendanceBL employeeMarkAttendanceBL = new EmployeeMarkAttendanceBL();
            try
            {
                ViewData["essEmployeeId"] = essEmployeeId;
                ViewData["essEmployeeName"] = essEmployeeName;
                employeeMarkAttendance = employeeMarkAttendanceBL.GetEmployeeMarkAttendanceList(essEmployeeId, fromDate, toDate, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeMarkAttendance);
        }

        [HttpPost]
        public JsonResult GetEmployeeInOutDetails(string attendanceDate = "", int employeeId = 0)
        {
            List<EmployeeMarkAttendanceViewModel> employeeMarkAttendance = new List<EmployeeMarkAttendanceViewModel>();
            EmployeeMarkAttendanceBL employeeMarkAttendanceBL = new EmployeeMarkAttendanceBL();
            try
            {
                employeeMarkAttendance = employeeMarkAttendanceBL.GetEmployeeInOutDetails(attendanceDate, employeeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employeeMarkAttendance, JsonRequestBehavior.AllowGet);
        }
       

        #endregion
       


    }
}
