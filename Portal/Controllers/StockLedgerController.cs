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
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class StockLedgerController : BaseController
    {
        //
        // GET: /Product/
        //
        // GET: /User/
        
        [ValidateRequest(true, UserInterfaceHelper.Print_Stock_Ledger, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult PrintStockLedger(int productId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;

                if (productId != 0)
                {
                    ViewData["productId"] = productId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["productId"] = 0;
                    ViewData["accessMode"] = 0;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        public ActionResult Report(int productTypeId=0,string assemblyType="",int productMainGroupId=0, int productSubGroupId=0,long productId=0,int customerBranchId=0,string fromDate="", string toDate="", string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            StockLedgerBL stockLedgerBL = new StockLedgerBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "StockLedger.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("PrintStockLedger");
            }

            DataTable dt = new DataTable();
            dt = stockLedgerBL.GetStockLedgerDataTable(productTypeId, assemblyType, productMainGroupId, productSubGroupId, productId, customerBranchId,Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate), ContextUser.CompanyId);

            
            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);
            
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
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
        public ActionResult SummaryReport(int productTypeId=0, string assemblyType="", int productMainGroupId=0, int productSubGroupId=0, long productId=0, int customerBranchId=0, string fromDate="", string toDate="", string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            StockLedgerBL stockLedgerBL = new StockLedgerBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "StockSummary.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("PrintStockLedger");
            }

            DataTable dt = new DataTable();
            dt = stockLedgerBL.GetStockSummaryDataTable(productTypeId, assemblyType, productMainGroupId, productSubGroupId, productId, customerBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId);


            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
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
        [ValidateRequest(true, UserInterfaceHelper.Stock_Ledger_DrilDown, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult StockLedgerDrilDown(int productId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;

                if (productId != 0)
                {
                    ViewData["productId"] = productId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["productId"] = 0;
                    ViewData["accessMode"] = 0;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult GetStockLedgerList(int productTypeId=0, string assemblyType="", int productMainGroupId=0, int productSubGroupId=0, long productId=0, int customerBranchId=0, string fromDate="", string toDate="")
        {
            List<StockLedgerViewModel> stockLedgerViewModel = new List<StockLedgerViewModel>();
            StockLedgerBL stockLedgerBL = new StockLedgerBL();
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                stockLedgerViewModel = stockLedgerBL.GetStockLedgerList(productTypeId, assemblyType, productMainGroupId, productSubGroupId, productId, customerBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(stockLedgerViewModel);
        }
        [HttpGet]
        public PartialViewResult GetStockLedgerDrilDownList(long productId=0,string fromDate="", string toDate="",int customerBranchId=0)
        {
            FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

            ViewData["fromDate"] = finYear.StartDate;
            ViewData["toDate"] = finYear.EndDate;
            List<StockLedgerViewModel> stockLedgerViewModel = new List<StockLedgerViewModel>();        
            StockLedgerBL stockLedgerBL = new StockLedgerBL();
            try
            {

                stockLedgerViewModel = stockLedgerBL.GetStockLedgerDrilDownList(productId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),ContextUser.CompanyId, customerBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(stockLedgerViewModel);
        }

        public ActionResult GenerateStockSummaryReport(int productTypeId, string assemblyType, int productMainGroupId, int productSubGroupId, long productId, int customerBranchId, string fromDate, string toDate, string reportType = "PDF")
        {
            string branchName;
            LocalReport lr = new LocalReport();
            StockLedgerBL stockLedgerBL = new StockLedgerBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "StockSummaryReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("PrintStockLedger");
            }

            DataTable dt = new DataTable();
            dt = stockLedgerBL.GetStockSummaryReports(productTypeId, assemblyType, productMainGroupId, productSubGroupId, productId, customerBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId);

            if(customerBranchId==0)
            {
                branchName = "All Branch";
            }
            else
            {
                branchName = stockLedgerBL.GetBranchName(customerBranchId);
            }
            ReportDataSource rd = new ReportDataSource("StockSummaryReportsDataSet", dt);
            lr.DataSources.Add(rd);
            ReportParameter rp3 = new ReportParameter("fromdate", fromDate);
            ReportParameter rp4 = new ReportParameter("todate", toDate);
            ReportParameter rp5 = new ReportParameter("CompanybranchName", branchName);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);
            lr.SetParameters(rp5);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>14.5in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
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
        public ActionResult GenerateStockLedgerReport(long productId, string fromDate, string toDate, string reportType = "PDF")
        {          
            LocalReport lr = new LocalReport();
            StockLedgerBL stockLedgerBL = new StockLedgerBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "StockLedgerReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("PrintStockLedger");
            }

            DataTable dt = new DataTable();
            dt = stockLedgerBL.GetStockLedgerReports(productId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId);

           
            ReportDataSource rd = new ReportDataSource("StockLedgerReportDataSet", dt);
            lr.DataSources.Add(rd);          
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>12.0in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
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
        public ActionResult StockLedgerReport(int productTypeId = 0, string assemblyType = "", int productMainGroupId = 0, int productSubGroupId = 0, long productId = 0, int customerBranchId = 0, string fromDate = "", string toDate = "",string CompanyBranch="", string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            StockLedgerBL stockLedgerBL = new StockLedgerBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "StockLedgerCompanyWise.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("PrintStockLedger");
            }
            string CompanyBranchPrint = "";
            if(CompanyBranch == "-Select Company Branch-")
            {
                CompanyBranchPrint = "All Company Branch";
            }
            else
            {
                CompanyBranchPrint = CompanyBranch;
            }
            DataTable dt = new DataTable();
            dt = stockLedgerBL.GetStockLedgerDataTable(productTypeId, assemblyType, productMainGroupId, productSubGroupId, productId, customerBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId);


            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);
            ReportParameter rp1 = new ReportParameter("CompanyBranch", CompanyBranchPrint);
            lr.SetParameters(rp1);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
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
    }
}
