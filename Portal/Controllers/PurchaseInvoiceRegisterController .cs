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


namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class PurchaseInvoiceRegisterController : BaseController
    {
       

        [ValidateRequest(true, UserInterfaceHelper.Add_PurchaseInvoiceRegister, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPurchaseInvoiceRegister()
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
        public PartialViewResult GetPurchaseInvoiceRegisterList( string vendorId = "", int stateId=0, string fromDate = "", string toDate = "", int createdBy = 0, string sortBy = "", string sortOrder = "",string companyBranch="")
        {         
            List<PurchaseInvoiceViewModel> purchaseInvoiceViewModel = new List<PurchaseInvoiceViewModel>();           
            PurchaseInvoiceRegisterBL purchaseInvoiceRegisterBL = new PurchaseInvoiceRegisterBL();
            try
            {
                purchaseInvoiceViewModel = purchaseInvoiceRegisterBL.GetPurchaseInvoiceRegisterList( vendorId, stateId, fromDate, toDate, ContextUser.CompanyId, createdBy, sortBy, sortOrder, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(purchaseInvoiceViewModel);
        }
        public ActionResult GeneratePIRegisterReports(string vendorId = "", int stateId = 0, string fromDate = "", string toDate = "", int createdBy = 0, string sortBy = "", string sortOrder = "", string companyBranch = "" , string companyBranchName = "",string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PurchaseInvoiceRegisterBL purchaseInvoiceRegisterBL = new PurchaseInvoiceRegisterBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "PIRegisterReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ListPurchaseInvoiceRegister");
            }
            ReportDataSource rd = new ReportDataSource("PIRegisterDataSet", purchaseInvoiceRegisterBL.GeneratePIRegisterReports(vendorId, stateId, fromDate, toDate, ContextUser.CompanyId, createdBy, sortBy, sortOrder, companyBranch));
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
            "  <PageWidth>41.6in</PageWidth>" +
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
