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
    public class DebitNoteVoucherController : BaseController
    {
        #region Debit Note Voucher Entry

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DebitNoteVoucher, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditDebitNoteVoucher(int voucherId = 0, int accessMode = 3, string ReportLevel = "", string FromDate = "", string ToDate = "", string StartInterface = "", int GLMainGroupId = 0, int GLSubGroupId = 0, Int32 GLId = 0, Int32 SLId = 0)
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
                ViewData["startInterface"] = StartInterface ;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DebitNoteVoucher, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditDebitNoteVoucher(VoucherViewModel voucherViewModel, List<VoucherDetailViewModel> voucherEntryList,List<VoucherSupportingDocumentViewModel> debitNotevoucherDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            DebitNoteVoucherBL debitnoteVoucherBL = new DebitNoteVoucherBL();
            try
            {
                if (voucherViewModel != null)
                {
                    voucherViewModel.CreatedBy = ContextUser.UserId;
                    voucherViewModel.CompanyId = ContextUser.CompanyId;
                    voucherViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = debitnoteVoucherBL.AddEditDebitNoteVoucher(voucherViewModel, voucherEntryList, debitNotevoucherDocuments);
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
        public PartialViewResult GetDebitNoteVoucherEntryList(List<VoucherDetailViewModel> voucherEntryList, long voucherId)
        {

            DebitNoteVoucherBL debitnoteVoucherBL = new DebitNoteVoucherBL();
            try
            {
                if (voucherEntryList == null)
                {
                    voucherEntryList = debitnoteVoucherBL.GetDebitNoteVoucherEntryList(voucherId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(voucherEntryList);
        }

        [HttpGet]
        public JsonResult GetDebitNoteVoucherDetail(long voucherId)
        {
            DebitNoteVoucherBL debitnoteVoucherBL = new DebitNoteVoucherBL(); 
            VoucherViewModel voucher = new VoucherViewModel();
            try
            {
                voucher = debitnoteVoucherBL.GetDebitNoteVoucherDetail(voucherId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(voucher, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DebitNoteVoucher, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListDebitNoteVoucher()
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
        public PartialViewResult GetDebitNoteVoucherList(Int32 bookId, string voucherMode, string voucherNo, string voucherStatus, string fromDate, string toDate,int companyBranchId)
        {
            List<VoucherViewModel> vouchers = new List<VoucherViewModel>();
          
            DebitNoteVoucherBL debitnoteVoucherBL = new DebitNoteVoucherBL(); 
           
            try
            {
                vouchers = debitnoteVoucherBL.GetDebitNoteVoucherList(bookId, voucherMode, voucherNo, voucherStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(vouchers);
        }

        public ActionResult DebitNoteVoucherPrint(long voucherId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();

            DebitNoteVoucherBL debitnoteVoucherBL = new DebitNoteVoucherBL();

            string path = Path.Combine(Server.MapPath("~/RDLC"), "DebitNoteVoucherPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable dt = new DataTable();
            dt = debitnoteVoucherBL.GetDebitNoteVoucherDetailDataTable(voucherId);

            decimal totalVoucherAmount = 0;
            totalVoucherAmount = Convert.ToDecimal(dt.Rows[0]["VoucherAmount"].ToString());
            string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalVoucherAmount));

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdVoucherEntry = new ReportDataSource("DataSetVoucherEntry", debitnoteVoucherBL.GetDebitNoteVoucherEntryListDataTable(voucherId));

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
        public PartialViewResult GetPIList(string piNo = "", string vendorName = "", string refNo = "", string fromDate = "", string toDate = "", string approvalStatus = "", string displayType = "")
        {
            List<PurchaseInvoiceViewModel> pis = new List<PurchaseInvoiceViewModel>();
            PurchaseInvoiceBL piBL = new PurchaseInvoiceBL();
            try
            {
                pis = piBL.GetPIList(piNo, vendorName, refNo, fromDate, toDate, ContextUser.CompanyId, approvalStatus, displayType,"","0","","");
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pis);
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
                        var path = Path.Combine(Server.MapPath("~/Images/DebitNoteVoucherDocument"), newFileName);
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
            var path = Path.Combine(Server.MapPath("~/Images/DebitNoteVoucherDocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [HttpPost]
        public PartialViewResult GetDebitNoteVoucherSupportingDocumentList(List<VoucherSupportingDocumentViewModel> debitNotelVoucherDocuments, long voucherId)
        {          
            DebitNoteVoucherBL debitNoteVoucherBL = new DebitNoteVoucherBL();
            try
            {
                if (debitNotelVoucherDocuments == null)
                {
                    debitNotelVoucherDocuments = debitNoteVoucherBL.GetDebitNoteVoucherSupportingDocumentList(voucherId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(debitNotelVoucherDocuments);
        }
        public ActionResult GenerateDebitNoteVoucherReport(Int32 bookId, string voucherMode, string voucherNo, string voucherStatus, string fromDate, string toDate,int companyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();            
            DebitNoteVoucherBL debitNoteVoucherBL = new DebitNoteVoucherBL();

            string path = Path.Combine(Server.MapPath("~/RDLC"), "DebitNoteVoucherReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("DebitNoteVoucherReportsDataSet", debitNoteVoucherBL.GenerateDebitNoteVoucherReports(bookId, voucherMode, voucherNo, voucherStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId, companyBranchId));
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
            "  <PageWidth>14.4in</PageWidth>" +
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


        #region Helper Method
        [HttpGet]
        public JsonResult GetBookList(string bookType)
        {
            BookBL bookBL = new BookBL();
            List<BookViewModel> bookList = new List<BookViewModel>();
            try
            {
                bookList = bookBL.GetDebitNoteBookList(bookType, ContextUser.CompanyId);
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
        public JsonResult GetSLAutoCompleteList(string term, int slTypeId)
        {
            SLBL slBL = new SLBL();
            List<SLViewModel> slList = new List<SLViewModel>();
            try
            {
                slList = slBL.GetSLAutoCompleteList(term, slTypeId, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(slList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetJVGLAutoCompleteList(string term)
        {
            GLBL glBL = new GLBL();
            List<GLViewModel> glList = new List<GLViewModel>();
            try
            {
                glList = glBL.GetGLAutoCompleteListForJournalVoucher(term, ContextUser.CompanyId);
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
        #endregion

        #region Cancel Approved

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ApprovedDebitNoteVoucher, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListApprovedDebitNoteVoucher()
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
        public PartialViewResult GetApprovedDebitNoteVoucherList(Int32 bookId, string voucherMode, string voucherNo, string voucherStatus, string fromDate, string toDate,int companyBranchId)
        {
            List<VoucherViewModel> vouchers = new List<VoucherViewModel>();

            DebitNoteVoucherBL debitnoteVoucherBL = new DebitNoteVoucherBL();

            try
            {
                vouchers = debitnoteVoucherBL.GetApprovedDebitNoteVoucherList(bookId, voucherMode, voucherNo, voucherStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId,companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(vouchers);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ApprovedDebitNoteVoucher, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult CancelApprovedDebitNoteVoucher(int voucherId = 0, int accessMode = 3)
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ApprovedDebitNoteVoucher, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult CancelApprovedDebitNoteVoucher(long voucherId, string cancelReason, string voucherStatus)
        {
            ResponseOut responseOut = new ResponseOut();
            DebitNoteVoucherBL debitNoteVoucherBL = new DebitNoteVoucherBL();        
            VoucherViewModel voucherViewModel = new VoucherViewModel();
            try
            {
                if (voucherViewModel != null)
                {
                    voucherViewModel.VoucherId = voucherId;
                    voucherViewModel.CancelReason = cancelReason;
                    voucherViewModel.VoucherStatus = voucherStatus;
                    voucherViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = debitNoteVoucherBL.CancelApprovedDebitNoteVoucher(voucherViewModel);
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

    }
}
