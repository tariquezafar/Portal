using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class PLStatementPrintController : BaseController
    {
        //
     
        #region Profit Loss Statement Print
        [ValidateRequest(true, UserInterfaceHelper.PLStatementPrint, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult PLStatementPrint()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
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

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.PLStatementPrint, (int)AccessMode.ViewAccess, (int)RequestMode.Ajax)]
        public ActionResult PLStatementGenerate(string fromDate, string toDate, int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            ProfitLossStatementPrintBL plPrintBL = new ProfitLossStatementPrintBL();
            try
            {
                    
                    int finYearId= Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = plPrintBL.GenerateProfitLossStatement(ContextUser.CompanyId,finYearId,Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate),ContextUser.UserId,Session.SessionID, companyBranchId);
            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PLStatementReport(int reportLevel,string fromDate,string toDate, string CompanyBranch, int CompanyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            ProfitLossStatementPrintBL plPrintBL = new ProfitLossStatementPrintBL();
            CompanyBL companyBL = new CompanyBL();
            string companyName = companyBL.GetCompanyDetail(ContextUser.CompanyId).CompanyName;


            string path = Path.Combine(Server.MapPath("~/RDLC"), "PLStatementPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("PLStatementPrint");
            }

            DataTable dt = new DataTable();
            dt.Rows.Clear();
            dt = plPrintBL.GetProfitLossStatementDataTable(ContextUser.UserId,Session.SessionID, CompanyBranchId, "EXPENSES");
            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);

            DataTable dt1 = new DataTable();
            dt1.Rows.Clear();
            dt1 = plPrintBL.GetProfitLossStatementDataTable(ContextUser.UserId, Session.SessionID,CompanyBranchId, "INCOME");
            ReportDataSource rd1 = new ReportDataSource("DataSet2", dt1);
            lr.DataSources.Add(rd1);



            ReportParameter rp1 = new ReportParameter("CompanyName", companyName);
            ReportParameter rp2 = new ReportParameter("FromDate", fromDate);
            ReportParameter rp3 = new ReportParameter("ToDate", toDate);
            ReportParameter rp4 = new ReportParameter("ReportLevel",Convert.ToString(reportLevel));
            ReportParameter rp5 = new ReportParameter("CompanyBranch", CompanyBranch);


            lr.SetParameters(rp1);
            lr.SetParameters(rp2);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);
            lr.SetParameters(rp5);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
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

        #region Balance Sheet Print
        [ValidateRequest(true, UserInterfaceHelper.BalanceSheetPrint, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult BalanceSheetPrint()
        {
            try
            {

                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
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

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.BalanceSheetPrint, (int)AccessMode.ViewAccess, (int)RequestMode.Ajax)]
        public ActionResult BalanceSheetGenerate(string asOnDate ,int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            ProfitLossStatementPrintBL plPrintBL = new ProfitLossStatementPrintBL();
            try
            {

                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                responseOut = plPrintBL.GenerateBalanceSheet(ContextUser.CompanyId, finYearId, Convert.ToDateTime(asOnDate),  ContextUser.UserId, Session.SessionID, companyBranchId);
            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }


        public ActionResult BalanceSheetReport(int reportLevel, string fromDate, string toDate, string CompanyBranch, int CompanyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            ProfitLossStatementPrintBL plPrintBL = new ProfitLossStatementPrintBL();
            CompanyBL companyBL = new CompanyBL();
            string companyName = companyBL.GetCompanyDetail(ContextUser.CompanyId).CompanyName;
            string companyAddress = companyBL.GetCompanyDetail(ContextUser.CompanyId).Address +" "+ companyBL.GetCompanyDetail(ContextUser.CompanyId).City+" "+ companyBL.GetCompanyDetail(ContextUser.CompanyId).State+" "+ companyBL.GetCompanyDetail(ContextUser.CompanyId).CountryName;
            int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;

            decimal netProfitLoss = 0;
            netProfitLoss = plPrintBL.GetNetProfitLoss(ContextUser.CompanyId, finYearId, Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate), ContextUser.UserId, Session.SessionID, CompanyBranchId);

            string path = Path.Combine(Server.MapPath("~/RDLC"), "BalanceSheetPrintNew.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("BalanceSheetPrintNew");
            }

            DataTable dt = new DataTable();
            dt.Clear();
            dt = plPrintBL.GetBalanceSheetDataTable(ContextUser.UserId, Session.SessionID, CompanyBranchId, "LIABILITIES");

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);
            DataTable dt1 = new DataTable();
            dt1.Clear();
            dt1 = plPrintBL.GetBalanceSheetDataTable(ContextUser.UserId, Session.SessionID, CompanyBranchId, "ASSETS");
            ReportDataSource rd1 = new ReportDataSource("DataSet2", dt1);
            lr.DataSources.Add(rd1);

            ReportParameter rp1 = new ReportParameter("CompanyName", companyName);
            ReportParameter rp2 = new ReportParameter("FromDate", fromDate);
            ReportParameter rp3 = new ReportParameter("ToDate", toDate);
            ReportParameter rp4 = new ReportParameter("ReportLevel", Convert.ToString(reportLevel));
            ReportParameter rp5 = new ReportParameter("NetProfitLoss", Convert.ToString(netProfitLoss));
            ReportParameter rp6 = new ReportParameter("companyAddress", Convert.ToString(companyAddress));
            ReportParameter rp7 = new ReportParameter("CompanyBranch", CompanyBranch);

            lr.SetParameters(rp1);
            lr.SetParameters(rp2);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);
            lr.SetParameters(rp5);
            lr.SetParameters(rp6);
            lr.SetParameters(rp7);
            string mimeType;
            string encoding;
            string fileNameExtension;
            
            string deviceInfo = "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
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
