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
    public class SubLedgerPrintController : BaseController
    {
        
        #region Sub Ledger Without Financial Year Print
        [ValidateRequest(true, UserInterfaceHelper.SubLedgerPrint, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult SubLedgerPrint()
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
        [ValidateRequest(true, UserInterfaceHelper.SubLedgerPrint, (int)AccessMode.ViewAccess, (int)RequestMode.Ajax)]
        public ActionResult SubLedgerGenerate(int slTypeId, int glId,long slId, string fromDate, string toDate,int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            SubLedgerWOFYPrintBL slPrintBL = new SubLedgerWOFYPrintBL();
            try
            {
                responseOut = slPrintBL.GenerateSubLedger(slTypeId,glId,slId, ContextUser.CompanyId, Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate),ContextUser.UserId,Session.SessionID, companyBranchId);
            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SubLedgerWoFySingleGLReport(string fromDate,string toDate, string CompanyBranch, int companyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SubLedgerWOFYPrintBL slPrintBL = new SubLedgerWOFYPrintBL();
            CompanyBL companyBL = new CompanyBL();
            string companyName = companyBL.GetCompanyDetail(ContextUser.CompanyId).CompanyName;


            string path = Path.Combine(Server.MapPath("~/RDLC"), "SubLedgerWoFySingleGLPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("SubLedgerPrint");
            }

            DataTable dt = new DataTable();
            dt = slPrintBL.GetSubLedgerDetailDataTable(ContextUser.UserId,Session.SessionID, companyBranchId);

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);
            


            
            ReportParameter rp1 = new ReportParameter("CompanyName", companyName);
            ReportParameter rp2 = new ReportParameter("FromDate", fromDate);
            ReportParameter rp3 = new ReportParameter("ToDate", toDate);
            ReportParameter rp4 = new ReportParameter("CompanyBranch", CompanyBranch);


            lr.SetParameters(rp1);
            lr.SetParameters(rp2);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
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

        public ActionResult SubLedgerWoFyAllGLReport(string fromDate, string toDate, string CompanyBranch, int companyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SubLedgerWOFYPrintBL slPrintBL = new SubLedgerWOFYPrintBL();
            CompanyBL companyBL = new CompanyBL();
            string companyName = companyBL.GetCompanyDetail(ContextUser.CompanyId).CompanyName;


            string path = Path.Combine(Server.MapPath("~/RDLC"), "SubLedgerWoFyAllGLPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("SubLedgerPrint");
            }

            DataTable dt = new DataTable();
            dt = slPrintBL.GetSubLedgerDetailDataTable(ContextUser.UserId, Session.SessionID, companyBranchId);

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);




            ReportParameter rp1 = new ReportParameter("CompanyName", companyName);
            ReportParameter rp2 = new ReportParameter("FromDate", fromDate);
            ReportParameter rp3 = new ReportParameter("ToDate", toDate);
            ReportParameter rp4 = new ReportParameter("CompanyBranch", CompanyBranch);


            lr.SetParameters(rp1);
            lr.SetParameters(rp2);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
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


        #endregion
        #region Helper Method

        [HttpGet]
        public JsonResult GetGLAutoCompleteList(string term, int slTypeId)
        {
            GLBL glBL = new GLBL();
            List<GLViewModel> slList = new List<GLViewModel>();
            try
            {
                slList = glBL.GetGLAutoCompleteList(term, slTypeId, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(slList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSLAutoCompleteList(string term, int slTypeId)
        {
            SLBL slBL = new SLBL();
            List<SLViewModel> slList = new List<SLViewModel>();
            try
            {
                slList = slBL.GetSLAutoCompleteList(term, slTypeId, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(slList, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
