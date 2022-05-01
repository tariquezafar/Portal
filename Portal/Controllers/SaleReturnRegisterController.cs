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
    public class SaleReturnRegisterController : BaseController
    {
        
        [ValidateRequest(true, UserInterfaceHelper.Add_SaleReturnRegister, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSaleReturnRegister()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
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
        public PartialViewResult GetSaleReturnRegisterList(string saleReturnNo = "", string customerName = "", string dispatchrefNo = "", string fromDate = "", string toDate = "", string approvalStatus = "", string sortBy = "", string sortOrder = "",int companyBranchId=0)
        {
            List<SaleReturnViewModel> saleReturnViewModel = new List<SaleReturnViewModel>();
            SaleReturnRegisterBL saleReturnRegisterBL = new SaleReturnRegisterBL();
            try
            {
                saleReturnViewModel = saleReturnRegisterBL.GetSaleReturnRegisterList(saleReturnNo, customerName, dispatchrefNo, fromDate, toDate, approvalStatus, ContextUser.CompanyId,sortBy,sortOrder, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(saleReturnViewModel);
        }

        public ActionResult GenerateSaleReturnRegisterReports(string saleReturnNo = "", string customerName = "", string dispatchrefNo = "", string fromDate = "", string toDate = "", string approvalStatus = "", string sortBy = "", string sortOrder = "",int companyBranchId=0, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();      
            SaleReturnRegisterBL saleReturnRegisterBL = new SaleReturnRegisterBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "SaleReturnRegisterReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("SaleReturnRegisterDataSet", saleReturnRegisterBL.GenerateSaleReturnRegisterReports(saleReturnNo, customerName, dispatchrefNo, fromDate, toDate, approvalStatus, ContextUser.CompanyId, sortBy, sortOrder, companyBranchId));
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
            "  <PageWidth>22.1in</PageWidth>" +
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
