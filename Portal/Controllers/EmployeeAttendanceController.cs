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
    public class EmployeeAttendanceController : BaseController
    {
        #region Employee Attendance
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeAttandance, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeAttendance(int employeeAttendanceId = 0, int accessMode = 3, int EmployeeId = 0, string EmployeeName = "")
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                if (EmployeeId != 0)
                {
                    ViewData["employeeAttendanceId"] = employeeAttendanceId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["employeeId"] = EmployeeId;
                    ViewData["employeeName"] = EmployeeName;
                }
                else
                {
                    ViewData["employeeAttendanceId"] = 0;
                    ViewData["accessMode"] = 3;
                    ViewData["employeeId"] = EmployeeId;
                    ViewData["employeeName"] = EmployeeName;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeAttandance, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeAttendance(int employeeId, string attendanceDate, string presentAbsent, string inTime, string outTime, string attendanceStatus,int companyBranch)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeAttendanceBL employeeAttendanceBL = new EmployeeAttendanceBL();
            try
            {
                if (attendanceDate !="")
                {
                    responseOut = employeeAttendanceBL.AddEditEmployeeAttendance(ContextUser.CompanyId, employeeId, attendanceDate, presentAbsent, inTime, outTime, attendanceStatus, ContextUser.UserId, companyBranch);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeAttandance, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeAttendance(int employeeId = 0, string employeeName = "")
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                ViewData["attendanceDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["essEmployeeId"] = employeeId;
                ViewData["essEmployeeName"] = employeeName;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpGet]
        public PartialViewResult GetEmployeeAttendanceList(int employeeId = 0, string attendanceDate = "", int departmentId = 0, int designationId = 0,string companyBranch = "")
        {
            List<HR_EmployeeAttendanceViewModel> employeeAttendance = new List<HR_EmployeeAttendanceViewModel>();
            EmployeeAttendanceBL employeeAttendanceBL = new EmployeeAttendanceBL();
            try
            {
                employeeAttendance = employeeAttendanceBL.GetEmployeeMarkAttendanceList(employeeId, attendanceDate, departmentId, designationId, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeAttendance);
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

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeAttandance, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult UpdateEmployeeAttendanceByEmployer(List<HR_EmployeeAttendanceViewModel> employeeAttendanceList)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeAttendanceBL employeeAttendanceBL = new EmployeeAttendanceBL();
            try
            {
                if (employeeAttendanceList != null && employeeAttendanceList.Count > 0)
                {
                    responseOut = employeeAttendanceBL.UpdateEmployeeAttendanceByEmployer(employeeAttendanceList, ContextUser.CompanyId, ContextUser.UserId);
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



     
        

        //Employee Attendance Form Section
        [ValidateRequest(true, UserInterfaceHelper.ApproveAttendance, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult EmployeeAttendanceForm(int employeeId = 0, string employeeName = "")
        {
            try
            {
                ViewData["attendanceDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["essEmployeeId"] = employeeId;
                ViewData["essEmployeeName"] = employeeName;
                ViewData["loggedInUserRoleId"] = ContextUser.RoleId;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpGet]
        public PartialViewResult GetEmployeeAttendanceFormList(int employeeId = 0, string attendanceDate = "", string employeeType = "", int departmentId = 0, int designationId = 0)
        {
            List<HR_EmployeeAttendanceViewModel> employeeAttendance = new List<HR_EmployeeAttendanceViewModel>();
            EmployeeAttendanceBL employeeAttendanceBL = new EmployeeAttendanceBL();
            try
            {
                employeeAttendance = employeeAttendanceBL.GetEmployeeAttendanceFormList(employeeId, attendanceDate, employeeType, departmentId, designationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeAttendance);
        }

        [HttpGet]
        public PartialViewResult GetTempEmployeeAttendanceFormList(int employeeId = 0, string attendanceDate = "", string employeeType = "", int departmentId = 0, int designationId = 0)
        {
            List<HR_EmployeeAttendanceViewModel> employeeAttendance = new List<HR_EmployeeAttendanceViewModel>();
            EmployeeAttendanceBL employeeAttendanceBL = new EmployeeAttendanceBL();
            try
            {
                employeeAttendance = employeeAttendanceBL.GetTempEmployeeAttendanceFormList(employeeId, attendanceDate, employeeType, departmentId, designationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeAttendance);
        }


        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.ApproveAttendance, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditEmployeeAttendanceFormByEmployer(List<HR_EmployeeAttendanceViewModel> employeeAttendanceList)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeAttendanceBL employeeAttendanceBL = new EmployeeAttendanceBL();
            try
            {
                if (employeeAttendanceList != null && employeeAttendanceList.Count > 0)
                {
                    responseOut = employeeAttendanceBL.AddEditEmployeeAttendanceFormByEmployer(employeeAttendanceList, ContextUser.CompanyId, ContextUser.UserId);
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

        //[ValidateRequest(true, UserInterfaceHelper.Add_Edit_Customer, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult PrintEmployeeAttendanceReport(int employeeId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;

                if (employeeId != 0)
                {
                    ViewData["employeeId"] = employeeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["employeeId"] = 0;
                    ViewData["accessMode"] = 0;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        public ActionResult EmployeeAttendanceReport(int month = 0, int year = 0, string reportType = "PDF")
        {
            string currentDate = DateTime.Today.ToString("dd-MMM-yyyy");
            LocalReport lr = new LocalReport();
            lr.EnableHyperlinks = true;
            EmployeeAttendanceBL employeeAttendanceBL = new EmployeeAttendanceBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "EmployeeAttendanceReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("PrintEmployeeAttendanceReport");
            }
            string[] monthName = { "","January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        
            DataTable dt = new DataTable();
            dt = employeeAttendanceBL.GetEmployeeAttendanceReport(month, year);

            ReportDataSource rd = new ReportDataSource("EmployeeAttendanceReportDataSet", dt);
            lr.DataSources.Add(rd);
            ReportParameter rp3 = new ReportParameter("CurrentDate", currentDate);
            ReportParameter rp4 = new ReportParameter("monthName", monthName[month].ToUpper());
            ReportParameter rp5 = new ReportParameter("year", year.ToString());
            //ReportParameter rp4 = new ReportParameter("todate", toDate);
                lr.SetParameters(rp3);
                lr.SetParameters(rp4);
                lr.SetParameters(rp5);

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>39.3in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>.2in</MarginLeft>" +
            "  <MarginRight>.2in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }
        #endregion



    }
}
