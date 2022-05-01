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
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class PayRollProcessReportController : BaseController
    {
        #region Payroll Process Report
        //
        // GET: /PayrollProcessPeriod/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PayrollProcessReport, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult PayRollProcessReport()
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PayrollProcessReport, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult PayRollESIProcessReport()
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }


        [HttpGet]
        public PartialViewResult GetPayrollProcessReportList(int monthId = 0, int year=0)
        {
            List<PayrollProcessReportPFViewModel> payrollPeriods = new List<PayrollProcessReportPFViewModel>();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            try
            {
                payrollPeriods = payrollProcessBL.GetPayrollProcessReportList(monthId, year);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(payrollPeriods);
        }

        public ActionResult PayRollProcessReports(int monthId = 0, int year = 0, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "PayRollReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("PayRollReportDataSet", payrollProcessBL.PayRollProcessReports(monthId, year));
            lr.DataSources.Add(rd);

          

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11.5in</PageWidth>" +
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

        public string CreateFile(int monthId=0,int year=0)
        {
            //todo: add some data from your database into that string:
           
            DataTable dt = new DataTable();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            dt = payrollProcessBL.GetPayrollProcessReportListDataTable(monthId, year);

            var txt = "";

            //  Commented Beleow code By Dheeraj date 14-Jun-2018
            //  For Remove ECR TXT File HEADER PART

            /////////////////////////////////////////

            //foreach (DataColumn column in dt.Columns)
            //{
            //    //Add the Header row for Text file.
            //    txt += column.ColumnName + "\t\t";
            //}

            ////Add new line.
            //txt += "\r\n";

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Data rows.
                    //txt += row[column.ColumnName].ToString() + "\t\t";
                    txt += row[column.ColumnName].ToString()+ "#~#";
                }

                //Add new line.
                txt += "\r\n";
            }

            //Download the Text file.
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ECR.txt");
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(txt);
            Response.Flush();
            Response.End();
          

            return "ECR Download Succesfully";

        }

        [HttpGet]
        public PartialViewResult GetPayrollProcessReportESIList(int monthId = 0, int year = 0)
        {
            List<PayrollProcessReportESIViewModel> payrollPeriods = new List<PayrollProcessReportESIViewModel>();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            try
            {
                payrollPeriods = payrollProcessBL.GetPayrollProcessReportESIList(monthId, year);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(payrollPeriods);
        }


        [HttpGet]
        public PartialViewResult GetPayrollProcessReportESIListREPORT(int monthId = 0, int year = 0)
        {
            List<PayrollProcessReportESIViewModel> payrollPeriods = new List<PayrollProcessReportESIViewModel>();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            try
            {
                payrollPeriods = payrollProcessBL.GetPayrollProcessReportESIList(monthId, year);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(payrollPeriods);
        }

        public ActionResult PayRollProcessESIReports(int monthId = 0, int year = 0, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "ESIReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("ESIDataSet", payrollProcessBL.PayRollProcessESIReports(monthId, year));
            lr.DataSources.Add(rd);



            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>8.8in</PageWidth>" +
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

        public string GenerateESITXT(int monthId = 0, int year = 0)
        {
            //todo: add some data from your database into that string:

            DataTable dt = new DataTable();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            dt = payrollProcessBL.GetPayrollProcessReportListDataTableESI(monthId, year);

            var txt = "";

            //  Commented Beleow code By Dheeraj date 14-Jun-2018
            //  For Remove ECR TXT File HEADER PART

            /////////////////////////////////////////

            //foreach (DataColumn column in dt.Columns)
            //{
            //    //Add the Header row for Text file.
            //    txt += column.ColumnName + "\t\t";
            //}

            ////Add new line.
            //txt += "\r\n";

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Data rows.
                    //txt += row[column.ColumnName].ToString() + "\t\t";
                    txt += row[column.ColumnName].ToString() + " ";
                }

                //Add new line.
                txt += "\r\n";
            }

            //Download the Text file.
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ECR.txt");
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(txt);
            Response.Flush();
            Response.End();


            return "ECR Download Succesfully";

        }

        public ActionResult AddEditESI(List<PayrollProcessReportESIViewModel> eSIViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
           
            try
            {

                  responseOut = payrollProcessBL.AddEditESI(eSIViewModel);

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
