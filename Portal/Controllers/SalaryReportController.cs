using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.IO;
using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices;
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class SalaryReportController : BaseController
    {
        //
        // GET: /Customer/
        #region Salary Summary Report
        [ValidateRequest(true, UserInterfaceHelper.SalarySummaryReport, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult SalarySummaryReport(int salarySummaryReportId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (salarySummaryReportId != 0)
                {
                    ViewData["salarySummaryReportId"] = salarySummaryReportId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["salarySummaryReportId"] = 0;
                    ViewData["accessMode"] = 3;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        public ActionResult GenerateSalarySummaryReport(long payrollProcessingPeriodId, int companyBranchId, int departmentId, int designationId, string employeeType, string employeeCodes, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "SalarySummaryReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("SalarySummaryReport");
            }
            ReportDataSource rd = new ReportDataSource("SalarySummaryReportDataSet", payrollProcessBL.GetSalarySummaryReport(payrollProcessingPeriodId, companyBranchId, departmentId, designationId, employeeType, employeeCodes));
            lr.DataSources.Add(rd);
          
          
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>66.9in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>.1in</MarginLeft>" +
            "  <MarginRight>.1in</MarginRight>" +
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
        public ActionResult GenerateSalarySlipReport(long payrollProcessingPeriodId, int companyBranchId, int departmentId, int designationId, string employeeType, string employeeCodes, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "SalarySlipReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("SalarySummaryReport");
            }
            ReportDataSource rd = new ReportDataSource("SalarySummaryReportDataSet", payrollProcessBL.GetSalarySummaryReport(payrollProcessingPeriodId, companyBranchId, departmentId, designationId, employeeType, employeeCodes));
            lr.DataSources.Add(rd);


            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>.1in</MarginLeft>" +
            "  <MarginRight>.1in</MarginRight>" +
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

        #region Salary JV Generation
        [ValidateRequest(true, UserInterfaceHelper.SalaryJV, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult SalaryJV(int salaryJVId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (salaryJVId != 0)
                {
                    ViewData["salaryJVId"] = salaryJVId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["salaryJVId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.SalaryJV, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult SalaryJV(long payrollProcessingPeriodId, int companyBranchId, int departmentId, int designationId, string employeeType,  string employeeCodes)
        {
            ResponseOut responseOut = new ResponseOut();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            try
            {
                if (payrollProcessingPeriodId != 0)
                {
                    responseOut = payrollProcessBL.GenerateSalaryJV(payrollProcessingPeriodId, companyBranchId, departmentId, designationId, employeeType, ContextUser.UserId, employeeCodes);
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


        public ActionResult GenerateSalaryJVReport(string payrollPeriod, long payrollProcessingPeriodId, int companyBranchId, int departmentId, int designationId, string employeeType, string employeeCodes, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "SalaryJVReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("SalaryJV");
            }
            ReportDataSource rd = new ReportDataSource("SalaryJVReportDataSet", payrollProcessBL.GetSalaryJVReport(payrollProcessingPeriodId, companyBranchId, departmentId, designationId, employeeType, employeeCodes));
            lr.DataSources.Add(rd);
            ReportParameter rp1 = new ReportParameter("PayrollPeriod", payrollPeriod);
            
            lr.SetParameters(rp1);
            

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>.1in</MarginLeft>" +
            "  <MarginRight>.1in</MarginRight>" +
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

        #region Helper Method
        [HttpGet]
        public JsonResult GetProcessedMonthList()
        {
            PayrollProcessPeriodBL payrollProcessPeriodBL = new PayrollProcessPeriodBL();
            List<PayrollProcessPeriodViewModel> payrollMonthList = new List<PayrollProcessPeriodViewModel>();

            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                payrollMonthList = payrollProcessPeriodBL.GetPayrollProcessedMonthList(ContextUser.CompanyId,finYear.FinYearId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(payrollMonthList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPayrollMonthStartAndEndDate(int monthId)
        {
            PayrollProcessPeriodBL payrollProcessPeriodBL = new PayrollProcessPeriodBL();
            PayrollProcessPeriodViewModel payrollProcessDate = new PayrollProcessPeriodViewModel();

            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                payrollProcessDate = payrollProcessPeriodBL.GetPayrollMonthStartEndDate(monthId, finYear.FinYearId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(payrollProcessDate, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
