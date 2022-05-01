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
using System.Data;
using System.IO;
using System.Text;


namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class PurchaseInvoiceImportController : BaseController
    {
        //
        // GET: /PurchaseInvoiceImport/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PurchaseInvoiceImport, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPurchaseInvoiceImport(int invoiceId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["RoleId"] = ContextUser.RoleId;

                if (invoiceId != 0)
                {

                    ViewData["invoiceId"] = invoiceId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["invoiceId"] = 0;
                    ViewData["accessMode"] = 0;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PurchaseInvoiceImport, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPurchaseInvoiceImport(PurchaseInvoiceImportViewModel piViewModel, List<PurchaseInvoiceImportProductDetailViewModel> piProducts,List<PurchaseInvoiceImportTermsDetailViewModel> piTerms)
        {
            ResponseOut responseOut = new ResponseOut();

            PurchaseInvoiceImportBL purchaseInvoiceImportBL = new PurchaseInvoiceImportBL();
            try
            {
                if (piViewModel != null)
                {
                    piViewModel.CreatedBy = ContextUser.UserId;
                    piViewModel.CompanyId = ContextUser.CompanyId;
                    piViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = purchaseInvoiceImportBL.AddEditPurchaseInvoiceImport(piViewModel, piProducts, piTerms);
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
        public PartialViewResult GetPurchaseInvoiceImportProductList(List<PurchaseInvoiceImportProductDetailViewModel> piProducts, long invoiceId)
        {
            PurchaseInvoiceImportBL piBL = new PurchaseInvoiceImportBL();
            try
            {
                if (piProducts == null)
                {
                    piProducts = piBL.GetPurchaseInvoiceImportProductList(invoiceId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(piProducts);
        }

        [HttpPost]
        public PartialViewResult GetPurchaseInvoiceImportTermList(List<PurchaseInvoiceImportTermsDetailViewModel> piTerms, long invoiceId)
        {
            PurchaseInvoiceImportBL piBL = new PurchaseInvoiceImportBL();
            try
            {
                if (piTerms == null)
                {
                    piTerms = piBL.GetPIImportTermList(invoiceId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(piTerms);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PurchaseInvoiceImport, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPurchaseInvoiceImport()
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
        public JsonResult GetPurchaseInvoiceImportDetail(long invoiceId)
        {
            PurchaseInvoiceImportBL piBL = new PurchaseInvoiceImportBL();
            PurchaseInvoiceImportViewModel pi = new PurchaseInvoiceImportViewModel();
            try
            {
                pi = piBL.GetPurchaseInvoiceImportDetail(invoiceId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(pi, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetPurchaseInvoiceImportList(string invoiceNo = "", string vendorName = "", string fromDate = "", string toDate = "", int companyId = 0, string invoiceStatus = "", string displayType = "", string CreatedByUserName = "", string companyBranch = "")
        {
            List<PurchaseInvoiceImportViewModel> pis = new List<PurchaseInvoiceImportViewModel>();
            PurchaseInvoiceImportBL piImportBL = new PurchaseInvoiceImportBL();
            try
            {
                pis = piImportBL.GetPurchaseInvoiceImportList(invoiceNo, vendorName,  fromDate, toDate, ContextUser.CompanyId, invoiceStatus, displayType, CreatedByUserName, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pis);
        }

        public ActionResult Report(long piId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PurchaseInvoiceImportBL purchaseInvoiceImportBL = new PurchaseInvoiceImportBL();
            PurchaseInvoiceBL purchaseInvoiceBL = new PurchaseInvoiceBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "PurchaseInvoiceImportPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable dt = new DataTable();
            dt = purchaseInvoiceImportBL.GetPurchaseInvoiceImportDataTable(piId);

            decimal totalInvoiceAmount = 0;
           
            totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
            string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));
            //approvalstatus = dt.Rows[0]["ApprovalStatus"].ToString();

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdProduct = new ReportDataSource("DataSetPIImportProducts", purchaseInvoiceImportBL.GetPurchaseInvoiceImportProductDataTable(piId));
           


            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
           
            ReportParameter rp1 = new ReportParameter("AmountInWords", totalWords);
            //ReportParameter rp2 = new ReportParameter("ReportOption", reportOption);
            //ReportParameter rp3 = new ReportParameter("approvalstatus", approvalstatus);
            lr.SetParameters(rp1);
            //lr.SetParameters(rp2);
            //lr.SetParameters(rp3);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
                      "  <OutputFormat>" + reportType + "</OutputFormat>" +
                      "  <PageWidth>8.5in</PageWidth>" +
                      "  <PageHeight>11in</PageHeight>" +
                      "  <MarginTop>0.10in</MarginTop>" +
                      "  <MarginLeft>.35in</MarginLeft>" +
                      "  <MarginRight>.1in</MarginRight>" +
                      "  <MarginBottom>0.1in</MarginBottom>" +
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
        public PartialViewResult GetPurchaseOrderList(string poNo = "", string vendorName = "", string refNo = "", string fromDate = "", string toDate = "", string approvalStatus = "", string displayType = "", string CreatedByUserName = "", int companyBranch = 0, string poType = "0")
        {
            List<POViewModel> poList = new List<POViewModel>();
            POBL poBL = new POBL();
            try
            {
                poList = poBL.GetPOList(poNo, vendorName, refNo, fromDate, toDate, approvalStatus, ContextUser.CompanyId, displayType, CreatedByUserName, companyBranch, poType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(poList);
        }
        //For PI Partil Against PO By Dheeraj Kumar
        [HttpPost]
        public PartialViewResult GetPOProductList(List<PurchaseInvoiceImportProductDetailViewModel> poProducts, long poId)
        {
           
            PurchaseInvoiceImportBL purchaseInvoiceImportBL = new PurchaseInvoiceImportBL();
            try
            {
                if (poProducts == null)
                {
                    poProducts = purchaseInvoiceImportBL.GetPIPOProductList(poId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(poProducts);
        }
    }
}
