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
    public class SINRegisterController : BaseController
    { 

        [ValidateRequest(true, UserInterfaceHelper.Add_SINRegister, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSINRegister()
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

        [HttpGet]
        public PartialViewResult GetSINRegisterList(string sinNo = "",string requisitionNo="" , string jobNo = "",int companyBranchId=0, int fromLocation = 0, int toLocation = 0, string fromDate = "", string toDate = "", string employee = "", string sortBy = "", string sortOrder = "")
        {
            List<SINViewModel> sins = new List<SINViewModel>();
            SINRegisterBL sinregisterBL = new SINRegisterBL();
            try
            {
                sins = sinregisterBL.GetSINRegisterList(sinNo, requisitionNo, jobNo,companyBranchId, fromLocation, toLocation, fromDate, toDate,ContextUser.CompanyId, employee, sortBy, sortOrder);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(sins);
        }

       
        public ActionResult GetSINRegisterReport(string sinNo = "", string requisitionNo = "", string jobNo = "", int companyBranchId = 0, int fromLocation = 0, int toLocation = 0, string fromDate = "", string toDate = "", string sortBy = "", string sortOrder = "", string reportType = "PDF")
        {           
          
            LocalReport lr = new LocalReport();
            SINRegisterBL sinregisterBL = new SINRegisterBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "SINRegisterReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("SINRegisterReportDataSet", sinregisterBL.GetSINRegisterReport(sinNo, requisitionNo, jobNo, companyBranchId, fromLocation, toLocation, fromDate, toDate, ContextUser.CompanyId,"", sortBy, sortOrder));
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
            "  <PageWidth>17.0in</PageWidth>" +
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

    }
}
