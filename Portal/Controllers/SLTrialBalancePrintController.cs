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
    public class SLTrialBalancePrintController : BaseController
    {
        
        #region SL Trial Print
        [ValidateRequest(true, UserInterfaceHelper.SLTrialBalancePrint, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult SLTrialBalancePrint()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["fromDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.SLTrialBalancePrint, (int)AccessMode.ViewAccess, (int)RequestMode.Ajax)]
        public ActionResult SLTrialBalanceGenerate(int slTypeId,int glId,string asOnDate,int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            SLTrialBalancePrintBL trialPrintBL = new SLTrialBalancePrintBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                responseOut = trialPrintBL.GenerateSLTrialBalance(slTypeId,glId, ContextUser.CompanyId, finYearId, Convert.ToDateTime(asOnDate),ContextUser.UserId,Session.SessionID, companyBranchId);
            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SLTrialBalanceReport(string asOnDate,string CompanyBranch, int CompanyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SLTrialBalancePrintBL trialPrintBL = new SLTrialBalancePrintBL();
            CompanyBL companyBL = new CompanyBL();
            string companyName = companyBL.GetCompanyDetail(ContextUser.CompanyId).CompanyName;


            string path = Path.Combine(Server.MapPath("~/RDLC"), "SLTrialBalancePrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("SLTrialBalancePrint");
            }

            DataTable dt = new DataTable();
            dt = trialPrintBL.GetSLTrialBalanceDataTable(ContextUser.UserId,Session.SessionID, CompanyBranchId);

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);
            


            
            ReportParameter rp1 = new ReportParameter("CompanyName", companyName);
            ReportParameter rp2 = new ReportParameter("AsOnDate", asOnDate);
            ReportParameter rp3 = new ReportParameter("CompanyBranch", CompanyBranch);
            lr.SetParameters(rp1);
            lr.SetParameters(rp2);
            lr.SetParameters(rp3);


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
