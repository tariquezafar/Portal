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
    public class ProductPurchasePIController : BaseController
    {               
        [ValidateRequest(true, UserInterfaceHelper.List_Product_Purchase, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListProductPurchasePI(int productId = 0, int accessMode = 3)
        {

            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
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

        public PartialViewResult GetProductPurchasePIList(long productId, string vendorName = "", string invoiceFromDate = "", string invoiceToDate = "", string companyBranch = "")
        {
            List<ProductPurchasePIViewModel> productPurchaseViewModel = new List<ProductPurchasePIViewModel>();          
            ProductPurchasePIBL productPurchasePIBL = new ProductPurchasePIBL();
            try
            {
                productPurchaseViewModel = productPurchasePIBL.GetProductPurchasePIList(productId, vendorName, invoiceFromDate, invoiceToDate, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(productPurchaseViewModel);
        }

        public ActionResult PIProductReport(long productId = 0,string companyBranch="", string companyBranchName = "", string reportType = "PDF")
        {
            string productName;
            LocalReport lr = new LocalReport();          
            ProductPurchasePIBL productPurchasePIBL = new ProductPurchasePIBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "PIProductReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ListProductPurchasePI");
            }

            DataTable dt = new DataTable();
            dt = productPurchasePIBL.PIProductReport(productId, companyBranch);
            productName = productPurchasePIBL.GetProductName(productId); 
                      
            ReportDataSource rd = new ReportDataSource("PIProductDataSet", dt);
            ReportParameter rp1 = new ReportParameter("ProductName", productName);
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

            lr.DataSources.Add(rd);
            lr.SetParameters(rp1);
            lr.SetParameters(rp5);
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>24.0in</PageWidth>" +
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
