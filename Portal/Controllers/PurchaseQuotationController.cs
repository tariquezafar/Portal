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
using System.Text;
using System.Data;
namespace Portal.Controllers
{
    public class PurchaseQuotationController : BaseController
    {
        //
        // GET: /PurchaseQuotation/
        #region PurchaseQuotation
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PurchaseQuotation, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPurchaseQuotation(int quotationId = 0, int accessMode = 3, int vendorId = 0, string vendorCode = "", string vendorName = "")
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                if (quotationId != 0)
                {
                    ViewData["quotationId"] = quotationId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["quotationId"] = 0;
                    ViewData["accessMode"] = 0;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["VendorId"] = vendorId;
                    ViewData["VendorCode"] = vendorCode;
                    ViewData["VendorName"] = vendorName;

                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PurchaseQuotation, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPurchaseQuotation(PurchaseQuotationViewModel quotationViewModel, List<PurchaseQuotationProductViewModel> quotationProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            PurchaseQuotationBL purchaseQuotationBL = new PurchaseQuotationBL();
            
            try
            {
                if (quotationViewModel != null)
                {
                    quotationViewModel.CreatedBy = ContextUser.UserId;
                    quotationViewModel.CompanyId = ContextUser.CompanyId;
                    quotationViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = purchaseQuotationBL.AddEditPurchaseQuotation(quotationViewModel, quotationProducts);

                }

                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                    responseOut.trnId = 0;
                }

            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetPurchaseQuotationProductList(List<PurchaseQuotationProductViewModel> quotationProducts, long quotationId)
        {
            PurchaseQuotationBL purchaseQuotationBL = new PurchaseQuotationBL();
            try
            {
                if (quotationProducts == null)
                {
                    quotationProducts = purchaseQuotationBL.GetPurchaseQuotationProductList(quotationId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(quotationProducts);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PurchaseQuotation, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPurchaseQuotation(string listStatus = "false")
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["listStatus"] = listStatus;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetPurchaseQuotationDetail(long quotationId)
        {
            PurchaseQuotationBL quotationBL = new PurchaseQuotationBL();
           PurchaseQuotationViewModel quotation = new PurchaseQuotationViewModel();
            try
            {
                quotation = quotationBL.GetPurchaseQuotationDetail(quotationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(quotation, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetPurchaseQuotationList(string quotationNo = "", string vendorName = "", string refNo = "", string fromDate = "", string toDate = "", string displayType = "", string approvalStatus = "",string companyBranch="")
        {
            List<PurchaseQuotationViewModel> quotations = new List<PurchaseQuotationViewModel>();
            PurchaseQuotationBL purchaseQuotationBL = new PurchaseQuotationBL();
            companyBranch = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId.ToString() : "0";
            try
            {
                quotations = purchaseQuotationBL.GetPurchaseQuotationList(quotationNo, vendorName, refNo, fromDate, toDate, ContextUser.CompanyId, displayType, approvalStatus, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(quotations);
        }

        public ActionResult Report(long indentId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PurchaseQuotationBL purchaseQuotationBL = new PurchaseQuotationBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "GetPurchaseQuotationComparison.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = purchaseQuotationBL.GetPurchaseQuotationComparisonList(indentId);
            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
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


        [HttpGet]
        public PartialViewResult GetPurchaseIndentList(string indentNo = "", string indentType = "", string customerName = "", int companyBranchId = 0, string fromDate = "", string toDate = "", string displayType = "", string approvalStatus = "0")
        {
            List<PurchaseIndentViewModel> indents = new List<PurchaseIndentViewModel>();
            PurchaseQuotationBL purchaseQuotationBL = new PurchaseQuotationBL();

            try
            {
                indents = purchaseQuotationBL.GetPurchaseQuotationIndentList(indentNo, indentType, customerName, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId, displayType, approvalStatus, ContextUser.CreatedBy);
            }
            catch (Exception ex)
                {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(indents);
        }


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_QuotationComparison, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult QuotationComparison()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;


            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        public ActionResult PurchaseQuotationReport(long quotationId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PurchaseQuotationBL purchaseQuotationBL = new PurchaseQuotationBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "PurchaseQuotationReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = purchaseQuotationBL.GetPurchaseQuotationDataTable(quotationId);
            ReportDataSource rd = new ReportDataSource("PurchaseQuotationDetailDataSet", dt);
            ReportDataSource rdProduct = new ReportDataSource("PurchaseQuotationProductsDataSet", purchaseQuotationBL.GetPurchaseQuotationProductListDataTable(quotationId));
            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>8.5in</PageWidth>" +
                        "  <PageHeight>9in</PageHeight>" +
                        "  <MarginTop>0.50in</MarginTop>" +
                        "  <MarginLeft>.15in</MarginLeft>" +
                        "  <MarginRight>.05in</MarginRight>" +
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

        [HttpPost]
        public PartialViewResult GetPurchaseIndentProductList(List<PurchaseIndentProductDetailViewModel> purchaseIndentProducts, long purchaseIndentId)
        {
            PurchaseIndentBL purchaseIndentBL = new PurchaseIndentBL();
            try
            {
                if (purchaseIndentProducts == null)
                {
                    purchaseIndentProducts = purchaseIndentBL.GetPurchaseIndentProductList(purchaseIndentId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(purchaseIndentProducts);
        }

        #endregion
    }
}
