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
    public class VoucherController : BaseController
    {
        //
        // GET: /Voucher/
        #region Bank Voucher Entry
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_BankVoucher, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditBankVoucher(int voucherId = 0, int accessMode = 3,string ReportLevel="",string FromDate="",string ToDate="",string StartInterface="",int GLMainGroupId=0,int GLSubGroupId=0,Int32 GLId=0,Int32 SLId=0)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                ViewData["reportLevel"] = ReportLevel;
                ViewData["drillFromDate"] = FromDate;
                ViewData["drillToDate"] = ToDate;
                ViewData["startInterface"] = StartInterface;
                ViewData["gLMainGroupId"] = GLMainGroupId;
                ViewData["gLSubGroupId"] = GLSubGroupId;
                ViewData["glId"] = GLId;
                ViewData["slId"] = SLId;

                if (voucherId != 0)
                {
                    ViewData["voucherId"] = voucherId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["voucherId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_BankVoucher, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditBankVoucher(VoucherViewModel voucherViewModel, List<VoucherDetailViewModel> voucherEntryList,List<VoucherSupportingDocumentViewModel> voucherDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            VoucherBL voucherBL = new VoucherBL();
            try
            {
                if (voucherViewModel != null)
                {
                    voucherViewModel.CreatedBy = ContextUser.UserId;
                    voucherViewModel.CompanyId = ContextUser.CompanyId;
                    voucherViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = voucherBL.AddEditBankVoucher(voucherViewModel, voucherEntryList, voucherDocuments);
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
        public PartialViewResult GetBankVoucherEntryList(List<VoucherDetailViewModel> voucherEntryList, long voucherId)
        {
            VoucherBL voucherBL = new VoucherBL();
            try
            {
                if (voucherEntryList == null)
                {
                    voucherEntryList = voucherBL.GetBankVoucherEntryList(voucherId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(voucherEntryList);
        }

        [HttpGet]
        public JsonResult GetBankVoucherDetail(long voucherId)
        {
            VoucherBL voucherBL = new VoucherBL();
            VoucherViewModel voucher = new VoucherViewModel();
            try
            {
                voucher = voucherBL.GetBankVoucherDetail(voucherId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(voucher, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_BankVoucher, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListBankVoucher()
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
        public PartialViewResult GetBankVoucherList(Int32 bookId, string voucherMode, string voucherNo, string voucherStatus, string fromDate, string toDate,int companyBranchId)
        {
            List<VoucherViewModel> vouchers = new List<VoucherViewModel>();

            VoucherBL voucherBL = new VoucherBL();
            try
            {
                vouchers = voucherBL.GetBankVoucherList(bookId, voucherMode, voucherNo, voucherStatus,Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(vouchers);
        }
         


        public ActionResult BankVoucherPrint(long voucherId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            VoucherBL voucherBL = new VoucherBL();

            string path = Path.Combine(Server.MapPath("~/RDLC"), "BankVoucherPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable dt = new DataTable();
            dt = voucherBL.GetBankVoucherDetailDataTable(voucherId);

            decimal totalVoucherAmount = 0;
            totalVoucherAmount = Convert.ToDecimal(dt.Rows[0]["VoucherAmount"].ToString());
            string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalVoucherAmount));

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdVoucherEntry = new ReportDataSource("DataSetVoucherEntry", voucherBL.GetBankVoucherEntryListDataTable(voucherId));

            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdVoucherEntry);


            ReportParameter rp1 = new ReportParameter("AmountInWords", totalWords);
            //ReportParameter rp2 = new ReportParameter("ReportOption", reportOption);
            lr.SetParameters(rp1);
            //lr.SetParameters(rp2);

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

        [HttpPost]
        public ActionResult SaveSupportingDocument()
        {
            ResponseOut responseOut = new ResponseOut();
            HttpFileCollectionBase files = Request.Files;
            Random rnd = new Random();
            try
            {
                //  Get all files from Request object  
                if (files != null && files.Count > 0 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    HttpPostedFileBase file = files[0];
                    string fname;
                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    if (file != null && file.ContentLength > 0)
                    {
                        string newFileName = "";
                        var fileName = Path.GetFileName(file.FileName);
                        newFileName = Convert.ToString(rnd.Next(10000, 99999)) + "_" + fileName;
                        var path = Path.Combine(Server.MapPath("~/Images/VoucherDocument"), newFileName);
                        file.SaveAs(path);
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = newFileName;
                    }
                    else
                    {
                        responseOut.status = ActionStatus.Fail;
                        responseOut.message = "";
                    }
                }
                else
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = "";
                }


            }
            catch (Exception ex)
            {
                responseOut.message = "";
                responseOut.status = ActionStatus.Fail;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }
        public FileResult DocumentDownload(string fileName)
        {
            var path = Path.Combine(Server.MapPath("~/Images/VoucherDocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [HttpPost]
        
        public PartialViewResult GetVoucherSupportingDocumentList(List<VoucherSupportingDocumentViewModel> voucherDocuments, long voucherId)
        {        
            VoucherBL voucherBL = new VoucherBL();
            try
            {
                if (voucherDocuments == null)
                {
                    voucherDocuments = voucherBL.GetVoucherSupportingDocumentList(voucherId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(voucherDocuments);
        }

        
        public ActionResult GenerateBankVoucherReport(Int32 bookId, string voucherMode, string voucherNo, string voucherStatus, string fromDate, string toDate, int companyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();           
            VoucherBL voucherBL = new VoucherBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "BankVoucherReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("BankVoucherReportsDataSet", voucherBL.GenerateBankVoucherReports(bookId, voucherMode, voucherNo, voucherStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId, companyBranchId) );           
            lr.DataSources.Add(rd);
           
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>12.7in</PageWidth>" +
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



        #region List Approved Bank Voucher
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ApprovedBankVoucher, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListApprovedBankVoucher()
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
        public PartialViewResult GetApprovedBankVoucherList(Int32 bookId, string voucherMode, string voucherNo, string voucherStatus, string fromDate, string toDate,int companyBranchId)
        {
            List<VoucherViewModel> vouchers = new List<VoucherViewModel>();

            VoucherBL voucherBL = new VoucherBL();
            try
            {
                vouchers = voucherBL.GetApprovedBankVoucherList(bookId, voucherMode, voucherNo, voucherStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(vouchers);
        }



        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ApprovedBankVoucher, (int)AccessMode.CancelAccess, (int)RequestMode.GetPost)]
        public ActionResult CancelApprovedBankVoucher(int voucherId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (voucherId != 0)
                {
                    ViewData["voucherId"] = voucherId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["voucherId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ApprovedBankVoucher, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult CancelApprovedBankVoucher(long voucherId, string cancelReason, string voucherStatus)
        {
            ResponseOut responseOut = new ResponseOut();
            VoucherBL voucherBL = new VoucherBL();
            VoucherViewModel voucherViewModel = new VoucherViewModel();
            try
            {
                if (voucherViewModel != null)
                {
                    voucherViewModel.VoucherId = voucherId;
                    voucherViewModel.CancelReason = cancelReason;
                    voucherViewModel.VoucherStatus = voucherStatus;
                    voucherViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = voucherBL.CancelApprovedBankVoucher(voucherViewModel);
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



        #endregion






        #region Helper Method
        [HttpGet]
        public JsonResult GetBookList(string bookType,int companyBranchId)
        {
            BookBL bookBL = new BookBL();
            List<BookViewModel> bookList = new List<BookViewModel>();
            try
            {
                bookList = bookBL.GetBookList(bookType,ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(bookList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSLTypeList()
        {
            SLTypeBL slTypeBL = new SLTypeBL();
            List<SLTypeViewModel> slTypeList = new List<SLTypeViewModel>();
            try
            {
                slTypeList = slTypeBL.GetSLTypeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(slTypeList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSLAutoCompleteList(string term,int slTypeId)
        {
            SLBL slBL = new SLBL();
            List<SLViewModel> slList = new List<SLViewModel>();
            try
            {
                slList = slBL.GetSLAutoCompleteList(term,slTypeId, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(slList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGLAutoCompleteList(string term,int bookId)
        {
            GLBL glBL = new GLBL();
            List<GLViewModel> glList = new List<GLViewModel>();
            try
            {
                glList = glBL.GetGLAutoCompleteListForVoucher(term,bookId, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(glList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPaymentModeList()
        {
            PaymentModeBL paymentModeBL = new PaymentModeBL();
            List<PaymentModeViewModel> paymentList = new List<PaymentModeViewModel>();
            try
            {
                paymentList = paymentModeBL.GetPaymentModeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(paymentList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCostCenterList()
        {
            CostCenterBL costCenterBL = new CostCenterBL();
            List<CostCenterViewModel> costCenterList = new List<CostCenterViewModel>();
            try
            {
                costCenterList = costCenterBL.GetCostCenterList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(costCenterList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBookBalance(long voucherId=0,int bookId=0)
        {
            BookBL bookBL = new BookBL();
            decimal bookBalance = 0;
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                bookBalance= bookBL.GetBookBalance(ContextUser.CompanyId, finYearId,voucherId,bookId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(bookBalance, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetGLBalance(int glId)
        {
            decimal glBalance = 0;
            GeneralLedgerWOFYPrintBL glPrintBL = new GeneralLedgerWOFYPrintBL();
            try
            {

                int finYearId= Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                glBalance = glPrintBL.GetGeneralLedgerBalance(glId, ContextUser.CompanyId, finYearId);


            }
            catch (Exception ex)
            {
                
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(glBalance, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSLBalance(int glId=0,long slId=0)
        {
            decimal slBalance = 0;
            SubLedgerWOFYPrintBL slPrintBL = new SubLedgerWOFYPrintBL();
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                slBalance = slPrintBL.GetSubLedgerBalance(glId,slId,ContextUser.CompanyId, finYearId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(slBalance, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult GetPIList(string piNo = "", string vendorName = "", string refNo = "", string fromDate = "", string toDate = "", string approvalStatus = "", string displayType = "",string vendorCode="", string purchaseType = "0")
        {
            List<PurchaseInvoiceViewModel> pis = new List<PurchaseInvoiceViewModel>();
            PurchaseInvoiceBL piBL = new PurchaseInvoiceBL();
            try
            {
                pis = piBL.GetPIAndPIImportMergeList(piNo, vendorName, refNo, fromDate, toDate, ContextUser.CompanyId, approvalStatus, displayType, vendorCode, purchaseType,"","");
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pis);
        }

        [HttpGet]
        public PartialViewResult GetSaleInvoiceList(string saleinvoiceNo = "", string customerName = "", string refNo = "", string fromDate = "", string toDate = "", string invoiceType = "", string displayType = "", string approvalStatus = "",string customerCode="", int companyBranchId = 0, string saleType = "0")
        {
            List<SaleInvoiceViewModel> invoices = new List<SaleInvoiceViewModel>();
            SaleInvoiceBL saleinvoiceBL = new SaleInvoiceBL();
            try
            {
                invoices = saleinvoiceBL.GetSaleInvoiceList(saleinvoiceNo, customerName, refNo, fromDate, toDate, ContextUser.CompanyId, invoiceType, displayType, approvalStatus,customerCode, companyBranchId, saleType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(invoices);
        }

        #endregion
    }
}
