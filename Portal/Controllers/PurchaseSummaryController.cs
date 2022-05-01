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
    public class PurchaseSummaryController : BaseController
    {
        #region SaleInvoiceSummary
        [ValidateRequest(true, UserInterfaceHelper.Add_PurchaseSummary, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPurchaseSummary()
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
        public PartialViewResult GetPurchaseSummaryList(int vendorId, int userId, int stateId,  string fromDate, string toDate,string companyBranch)
        {
            List<PurchaseSummaryRegisterViewModel> purchaseInvoices = new List<PurchaseSummaryRegisterViewModel>();
            PurchaseInvoiceRegisterBL purchaseInvoiceRegisterBL = new PurchaseInvoiceRegisterBL();
            try
            {

                purchaseInvoices = purchaseInvoiceRegisterBL.GetPurchaseSummaryRegister(vendorId,userId, stateId ,ContextUser.CompanyId ,Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate), companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(purchaseInvoices);
        }
        public ActionResult PurchaseSummaryReport(int vendorId, int userId, int stateId, string fromDate, string toDate,string companyBranch, string companyBranchName = "", string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PurchaseInvoiceRegisterBL purchaseInvoiceRegisterBL = new PurchaseInvoiceRegisterBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "PurchaseSummaryReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ListPurchaseSummary");
            }
            ReportDataSource rd = new ReportDataSource("PurchaseSummaryRegisterDataSet", purchaseInvoiceRegisterBL.GeneratePurchaseSummaryReports(vendorId, userId, stateId, ContextUser.CompanyId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),companyBranch));
            lr.DataSources.Add(rd);
            ReportParameter rp3 = new ReportParameter("fromdate", fromDate);
            ReportParameter rp4 = new ReportParameter("todate", toDate);
            string companyBN = "";
            if (companyBranchName == "-Select Company Branch-")
            {
                companyBN = "All Company Branch";
            }
            else
            {
                companyBN = companyBranchName;
            }
            ReportParameter rp5 = new ReportParameter("CompanyBranch", companyBN);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);
            lr.SetParameters(rp5);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>10.5in</PageWidth>" +
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
    }
}
