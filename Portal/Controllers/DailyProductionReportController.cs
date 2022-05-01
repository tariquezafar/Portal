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
    public class DailyProductionReportController : BaseController
    {
         #region Daily Production Report

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DailyProductionReport, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListDailyProductionReport()
        {
            try
            {
                DateTime dt = DateTime.Now;
                DateTime firstdayofmonth = dt.AddDays(-(dt.Day - 1));
                DateTime lastdayofmonth = firstdayofmonth.AddMonths(1).AddDays(-1);

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = firstdayofmonth.ToString("dd-MMM-yyyy");
                ViewData["toDate"] = lastdayofmonth.ToString("dd-MMM-yyyy"); 
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }


        public ActionResult DailyProductionReport(string productName = "", string processType = "", string fromDate = "", string toDate = "",string reportType = "PDF")
        {

            DailyProductionReportBL dailyProductionBL = new DailyProductionReportBL();
            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/RDLC"), "DailyProductionReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ListChasisNoDetail");
            }

            DataTable dt = new DataTable();

            dt = dailyProductionBL.GetDailyProductionSummaryList(productName, processType, fromDate, toDate, ContextUser.CompanyId);

            

            ReportDataSource rd = new ReportDataSource("DailyProductionReport", dt);
            lr.DataSources.Add(rd);

            ReportParameter rp3 = new ReportParameter("fromdate", fromDate);
            ReportParameter rp4 = new ReportParameter("todate", toDate);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>10.5in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>.5in</MarginLeft>" +
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

        public ActionResult DailyProductionDetailReport(string productName = "", string processType = "", string fromDate = "", string toDate = "",string chassisno="", string reportType = "PDF")
        {

            DailyProductionReportBL dailyProductionBL = new DailyProductionReportBL();
            LocalReport lr = new LocalReport();

            string path = Path.Combine(Server.MapPath("~/RDLC"), "DailyProductionDetailReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ListChasisNoDetail");
            }

            DataTable dt = new DataTable();

            dt = dailyProductionBL.GetDailyProductionDetailedList(productName, processType, fromDate, toDate, ContextUser.CompanyId, chassisno);



            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);

            ReportParameter rp3 = new ReportParameter("fromdate", fromDate);
            ReportParameter rp4 = new ReportParameter("todate", toDate);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>10.5in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>.5in</MarginLeft>" +
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
