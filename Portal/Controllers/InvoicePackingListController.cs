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
using System.Text;
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class InvoicePackingListController : BaseController
    {
        #region Invoice Packing List

        //
        // GET: /DeliveryChallan/

        [ValidateRequest(true, UserInterfaceHelper.AddEditInvoicePackingList, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditInvoicePackingList(int invoicePackingListId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (invoicePackingListId != 0)
                {
                    ViewData["invoicePackingListId"] = invoicePackingListId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["invoicePackingListId"] = 0;
                    ViewData["accessMode"] = 0;
                    
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.AddEditInvoicePackingList, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditInvoicePackingList(InvoicePackingListViewModel invoicePackingListViewModel, List<InvoicePackingListProductDetailViewModel> invoicePackingListProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            InvoicePackingListBL invoicePackingListBL = new InvoicePackingListBL();
            try
            {
                if (invoicePackingListViewModel != null)
                {
                    invoicePackingListViewModel.CreatedBy = ContextUser.UserId;
                    invoicePackingListViewModel.CompanyId = ContextUser.CompanyId;
                    invoicePackingListViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = invoicePackingListBL.AddEditInvoicePackingList(invoicePackingListViewModel, invoicePackingListProducts);
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

        [ValidateRequest(true, UserInterfaceHelper.AddEditInvoicePackingList, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListInvoicePackingList(string totalPackingList="false")
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["TotalPackingList"] = totalPackingList;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult GetInvoicePackingList(string invoicePackingListNo = "", string invoiceNo = "", Int32 packingListType = 0, string fromDate = "", string toDate = "", string approvalStatus = "",string CreatedByUserName="",int companyBranchId=0)
        {
            List<InvoicePackingListViewModel> invoicePackingLists = new List<InvoicePackingListViewModel>();
            InvoicePackingListBL invoicePackingListBL = new InvoicePackingListBL();
            try
            {
                invoicePackingLists = invoicePackingListBL.GetInvoicePackingList(invoicePackingListNo, invoiceNo, packingListType, fromDate, toDate, approvalStatus, ContextUser.CompanyId, CreatedByUserName, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(invoicePackingLists);
        }

        [HttpGet]
        public JsonResult GetInvoicePackingListDetail(long invoicePackingListId)
        {
            InvoicePackingListBL invoicePackingListBL = new InvoicePackingListBL();
            InvoicePackingListViewModel invoicePackingList = new InvoicePackingListViewModel();
            try
            {
                invoicePackingList = invoicePackingListBL.GetInvoicePackingListDetail(invoicePackingListId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(invoicePackingList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetInvoicePackingListProductList(List<InvoicePackingListProductDetailViewModel> invoicePackingListProducts, long invoicePackingListId)
        {
            InvoicePackingListBL invoicePackingListBL = new InvoicePackingListBL();
            try
            {
                if (invoicePackingListProducts == null)
                {
                    invoicePackingListProducts = invoicePackingListBL.GetInvoicePackingListProductList(invoicePackingListId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(invoicePackingListProducts);
        }

        [HttpPost]
        public PartialViewResult GetPackingListTypeProductList(long invoiceId,long packingListTypeId)
        {
            List<InvoicePackingListProductDetailViewModel> invoicePackingListProducts = new List<InvoicePackingListProductDetailViewModel>();
            InvoicePackingListBL invoicePackingListBL = new InvoicePackingListBL();
            try
            {
                invoicePackingListProducts = invoicePackingListBL.GetPackingListTypeProductList(invoiceId, packingListTypeId);
                
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(invoicePackingListProducts);
        }

        public ActionResult Report(long invoicePackingListId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            InvoicePackingListBL invoicePackingListBL = new InvoicePackingListBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "InvoicePackingListReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = invoicePackingListBL.GetInvoicePackingListDetailDataTable(invoicePackingListId);
            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdProduct = new ReportDataSource("DataSet2", invoicePackingListBL.GetInvoicePackingListProductListDataTable(invoicePackingListId));
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

        [HttpGet]
        public PartialViewResult GetPLSaleInvoiceList(string saleinvoiceNo = "", string customerName = "", string refNo = "", string fromDate = "", string toDate = "", string invoiceType = "", string displayType = "", string approvalStatus = "",int companyBranchId=0)
        {
            List<SaleInvoiceViewModel> invoices = new List<SaleInvoiceViewModel>();           
            InvoicePackingListBL invoicePackingListBL = new InvoicePackingListBL();
            try
            {
                invoices = invoicePackingListBL.GetPLSaleInvoiceList(saleinvoiceNo, customerName, refNo, fromDate, toDate, ContextUser.CompanyId, invoiceType, displayType, approvalStatus,"",companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(invoices);
        }

        public ActionResult CheckListReport(long invoiceID,int packingListTypeid,long invoicepackingListId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            InvoicePackingListBL invoicePackingListBL = new InvoicePackingListBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "CheckListReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            DataTable dt = new DataTable();
            dt = invoicePackingListBL.GetChasisSerialNoList(invoiceID, packingListTypeid);
            ReportDataSource rd = new ReportDataSource("CheckListDataSet", dt);
            ReportDataSource rdPackingList = new ReportDataSource("CheckListProductDataSet", invoicePackingListBL.GetPackingListProduct(invoicepackingListId));
            ReportDataSource rdPackingListTypeName = new ReportDataSource("PackingListTypeDataSet", invoicePackingListBL.GetPackingListName(invoicepackingListId));
            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdPackingList);
            lr.DataSources.Add(rdPackingListTypeName);
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>11.6in</PageWidth>" +
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
        #endregion
    }
}
