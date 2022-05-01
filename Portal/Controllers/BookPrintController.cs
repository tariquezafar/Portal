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
    public class BookPrintController : BaseController
    {
        //
        // GET: /User/
        #region Bank Book Print
        [ValidateRequest(true, UserInterfaceHelper.BankBookPrint, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult BankBookPrint()
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

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.BankBookPrint, (int)AccessMode.ViewAccess, (int)RequestMode.Ajax)]
        public ActionResult BankBookGenerate(int bookId, string fromDate, string toDate,int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            BankBookPrintBL bankBookBL = new BankBookPrintBL();
            try
            {
                if (bookId != 0)
                {
                    
                    int finYearId= Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = bankBookBL.GenerateBankBook(bookId,ContextUser.CompanyId, finYearId,Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate),ContextUser.UserId,Session.SessionID, companyBranchId);
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


        public ActionResult BankBookReport(string bookName,string fromDate,string toDate,string CompanyBranch,int CompanyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            BankBookPrintBL bankBookPrintBL = new BankBookPrintBL();
            CompanyBL companyBL = new CompanyBL();
            string companyName = companyBL.GetCompanyDetail(ContextUser.CompanyId).CompanyName;


            string path = Path.Combine(Server.MapPath("~/RDLC"), "BankBookPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("BankBookPrint");
            }

            DataTable dt = new DataTable();
            dt = bankBookPrintBL.GetBankBookDetailDataTable(ContextUser.UserId,Session.SessionID, CompanyBranchId);

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);


            
            ReportParameter rp1 = new ReportParameter("BookName", bookName);
            ReportParameter rp2 = new ReportParameter("CompanyName", companyName);
            ReportParameter rp3 = new ReportParameter("FromDate", fromDate);
            ReportParameter rp4 = new ReportParameter("ToDate", toDate);
            ReportParameter rp5 = new ReportParameter("CompanyBranch", CompanyBranch);

            lr.SetParameters(rp1);
            lr.SetParameters(rp2);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);
            lr.SetParameters(rp5);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
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



        #endregion

        #region Journal Book Print
        [ValidateRequest(true, UserInterfaceHelper.JournalBookPrint, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult JournalBookPrint()
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

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.JournalBookPrint, (int)AccessMode.ViewAccess, (int)RequestMode.Ajax)]
        public ActionResult JournalBookGenerate(int bookId, string fromDate, string toDate,int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            JournalBookPrintBL journalBookBL = new JournalBookPrintBL();
            try
            {
                if (bookId != 0)
                {

                    int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = journalBookBL.GenerateJournalBook(bookId, ContextUser.CompanyId, finYearId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.UserId, Session.SessionID, companyBranchId);
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


        public ActionResult JournalBookReport(string bookName, string fromDate, string toDate, string CompanyBranch, int CompanyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            JournalBookPrintBL journalBookPrintBL = new JournalBookPrintBL();
            CompanyBL companyBL = new CompanyBL();
            string companyName = companyBL.GetCompanyDetail(ContextUser.CompanyId).CompanyName;


            string path = Path.Combine(Server.MapPath("~/RDLC"), "JournalBookPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("JournalBookPrint");
            }

            DataTable dt = new DataTable();
            dt = journalBookPrintBL.GetJournalBookDetailDataTable(ContextUser.UserId, Session.SessionID, CompanyBranchId);

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);



            ReportParameter rp1 = new ReportParameter("BookName", bookName);
            ReportParameter rp2 = new ReportParameter("CompanyName", companyName);
            ReportParameter rp3 = new ReportParameter("FromDate", fromDate);
            ReportParameter rp4 = new ReportParameter("ToDate", toDate);
            ReportParameter rp5 = new ReportParameter("CompanyBranch", CompanyBranch);

            lr.SetParameters(rp1);
            lr.SetParameters(rp2);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);
            lr.SetParameters(rp5);
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
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



        #endregion

        #region Cash Book Print
        [ValidateRequest(true, UserInterfaceHelper.CashBookPrint, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult CashBookPrint()
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

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.CashBookPrint, (int)AccessMode.ViewAccess, (int)RequestMode.Ajax)]
        public ActionResult CashBookGenerate(int bookId, string fromDate, string toDate, int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            CashBookPrintBL cashBookPrintBL = new CashBookPrintBL();
            try
            {
                if (bookId != 0)
                {

                    int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = cashBookPrintBL.GenerateCashBook(bookId, ContextUser.CompanyId, finYearId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.UserId, Session.SessionID, companyBranchId);
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


        public ActionResult CashBookReport(string bookName, string fromDate, string toDate, string CompanyBranch, int CompanyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            
            CashBookPrintBL cashBookPrintBL = new CashBookPrintBL();
            CompanyBL companyBL = new CompanyBL();
            string companyName = companyBL.GetCompanyDetail(ContextUser.CompanyId).CompanyName;


            string path = Path.Combine(Server.MapPath("~/RDLC"), "CashBookPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("CashBookPrint");
            }

            DataTable dt = new DataTable();
            dt = cashBookPrintBL.GetCashBookDetailDataTable(ContextUser.UserId, Session.SessionID, CompanyBranchId);

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);



            ReportParameter rp1 = new ReportParameter("BookName", bookName);
            ReportParameter rp2 = new ReportParameter("CompanyName", companyName);
            ReportParameter rp3 = new ReportParameter("FromDate", fromDate);
            ReportParameter rp4 = new ReportParameter("ToDate", toDate);
            ReportParameter rp5 = new ReportParameter("CompanyBranch", CompanyBranch);

            lr.SetParameters(rp1);
            lr.SetParameters(rp2);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);
            lr.SetParameters(rp5);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
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



        #endregion

        #region Sale Book Print
        [ValidateRequest(true, UserInterfaceHelper.SaleBookPrint, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult SaleBookPrint()
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

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.SaleBookPrint, (int)AccessMode.ViewAccess, (int)RequestMode.Ajax)]
        public ActionResult SaleBookGenerate(int bookId, string fromDate, string toDate,int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            
            SaleBookPrintBL saleBookPrintBL = new SaleBookPrintBL();
            try
            {
                if (bookId != 0)
                {

                    int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = saleBookPrintBL.GenerateSaleBook(bookId, ContextUser.CompanyId, finYearId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.UserId, Session.SessionID, companyBranchId);
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


        public ActionResult SaleBookReport(string bookName, string fromDate, string toDate, string CompanyBranch, int CompanyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            
            SaleBookPrintBL saleBookPrintBL = new SaleBookPrintBL();
            CompanyBL companyBL = new CompanyBL();
            string companyName = companyBL.GetCompanyDetail(ContextUser.CompanyId).CompanyName;


            string path = Path.Combine(Server.MapPath("~/RDLC"), "SaleBookPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("SaleBookPrint");
            }

            DataTable dt = new DataTable();
            dt = saleBookPrintBL.GetSaleBookDetailDataTable(ContextUser.UserId, Session.SessionID, CompanyBranchId);

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);



            ReportParameter rp1 = new ReportParameter("BookName", bookName);
            ReportParameter rp2 = new ReportParameter("CompanyName", companyName);
            ReportParameter rp3 = new ReportParameter("FromDate", fromDate);
            ReportParameter rp4 = new ReportParameter("ToDate", toDate);
            ReportParameter rp5 = new ReportParameter("CompanyBranch", CompanyBranch);

            lr.SetParameters(rp1);
            lr.SetParameters(rp2);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);
            lr.SetParameters(rp5);
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
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
        #endregion

        #region Purchase Book Print
        [ValidateRequest(true, UserInterfaceHelper.PurchaseBookPrint, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult PurchaseBookPrint()
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

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.PurchaseBookPrint, (int)AccessMode.ViewAccess, (int)RequestMode.Ajax)]
        public ActionResult PurchaseBookGenerate(int bookId, string fromDate, string toDate,int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();

            PurchaseBookPrintBL purchaseBookPrintBL = new PurchaseBookPrintBL();
            try
            {
                if (bookId != 0)
                {

                    int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = purchaseBookPrintBL.GeneratePurchaseBook(bookId, ContextUser.CompanyId, finYearId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.UserId, Session.SessionID, companyBranchId);
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


        public ActionResult PurchaseBookReport(string bookName, string fromDate, string toDate, string CompanyBranch, int companyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();

            PurchaseBookPrintBL purchaseBookPrintBL = new PurchaseBookPrintBL();
            CompanyBL companyBL = new CompanyBL();
            string companyName = companyBL.GetCompanyDetail(ContextUser.CompanyId).CompanyName;


            string path = Path.Combine(Server.MapPath("~/RDLC"), "PurchaseBookPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("PurchaseBookPrint");
            }

            DataTable dt = new DataTable();
            dt = purchaseBookPrintBL.GetPurchaseBookDetailDataTable(ContextUser.UserId, Session.SessionID, companyBranchId);

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);



            ReportParameter rp1 = new ReportParameter("BookName", bookName);
            ReportParameter rp2 = new ReportParameter("CompanyName", companyName);
            ReportParameter rp3 = new ReportParameter("FromDate", fromDate);
            ReportParameter rp4 = new ReportParameter("ToDate", toDate);
            ReportParameter rp5 = new ReportParameter("CompanyBranch", CompanyBranch);

            lr.SetParameters(rp1);
            lr.SetParameters(rp2);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);
            lr.SetParameters(rp5);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
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
        #endregion

        #region Credit Note Book Print
        [ValidateRequest(true, UserInterfaceHelper.CreditNoteBookPrint, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult CreditNoteBookPrint()
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

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.CreditNoteBookPrint, (int)AccessMode.ViewAccess, (int)RequestMode.Ajax)]
        public ActionResult CreditNoteBookGenerate(int bookId, string fromDate, string toDate,int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            CreditNoteBookPrintBL creditNoteBookPrintBL = new CreditNoteBookPrintBL();
            try
            {
                if (bookId != 0)
                {

                    int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = creditNoteBookPrintBL.GenerateCreditNoteBook(bookId, ContextUser.CompanyId, finYearId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.UserId, Session.SessionID, companyBranchId);
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


        public ActionResult CreditNoteBookReport(string bookName, string fromDate, string toDate, string CompanyBranch, int companyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();

            CreditNoteBookPrintBL creditNoteBookPrintBL = new CreditNoteBookPrintBL();
            CompanyBL companyBL = new CompanyBL();
            string companyName = companyBL.GetCompanyDetail(ContextUser.CompanyId).CompanyName;


            string path = Path.Combine(Server.MapPath("~/RDLC"), "CreditNoteBookPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("CreditNoteBookPrint");
            }

            DataTable dt = new DataTable();
            dt = creditNoteBookPrintBL.GetCreditNoteBookDetailDataTable(ContextUser.UserId, Session.SessionID, companyBranchId);

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);



            ReportParameter rp1 = new ReportParameter("BookName", bookName);
            ReportParameter rp2 = new ReportParameter("CompanyName", companyName);
            ReportParameter rp3 = new ReportParameter("FromDate", fromDate);
            ReportParameter rp4 = new ReportParameter("ToDate", toDate);
            ReportParameter rp5 = new ReportParameter("CompanyBranch", CompanyBranch);

            lr.SetParameters(rp1);
            lr.SetParameters(rp2);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);
            lr.SetParameters(rp5);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
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



        #endregion

        #region Debit Note Book Print
        [ValidateRequest(true, UserInterfaceHelper.DebitNoteBookPrint, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult DebitNoteBookPrint()
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

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.DebitNoteBookPrint, (int)AccessMode.ViewAccess, (int)RequestMode.Ajax)]
        public ActionResult DebitNoteBookGenerate(int bookId, string fromDate, string toDate, int companyBranchId)
        {
            ResponseOut responseOut = new ResponseOut();
            DebitNoteBookPrintBL debitNoteBookPrintBL = new DebitNoteBookPrintBL();
            try
            {
                if (bookId != 0)
                {

                    int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = debitNoteBookPrintBL.GenerateDebitNoteBook(bookId, ContextUser.CompanyId, finYearId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.UserId, Session.SessionID, companyBranchId);
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


        public ActionResult DebitNoteBookReport(string bookName, string fromDate, string toDate, string CompanyBranch, int companyBranchId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            DebitNoteBookPrintBL debitNoteBookPrintBL = new DebitNoteBookPrintBL();
            CompanyBL companyBL = new CompanyBL();
            string companyName = companyBL.GetCompanyDetail(ContextUser.CompanyId).CompanyName;


            string path = Path.Combine(Server.MapPath("~/RDLC"), "DebitNoteBookPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("DebitNoteBookPrint");
            }

            DataTable dt = new DataTable();
            dt = debitNoteBookPrintBL.GetDebitNoteBookDetailDataTable(ContextUser.UserId, Session.SessionID, companyBranchId);

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            lr.DataSources.Add(rd);



            ReportParameter rp1 = new ReportParameter("BookName", bookName);
            ReportParameter rp2 = new ReportParameter("CompanyName", companyName);
            ReportParameter rp3 = new ReportParameter("FromDate", fromDate);
            ReportParameter rp4 = new ReportParameter("ToDate", toDate);
            ReportParameter rp5 = new ReportParameter("CompanyBranch", CompanyBranch);

            lr.SetParameters(rp1);
            lr.SetParameters(rp2);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);
            lr.SetParameters(rp5);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
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



        #endregion
    }
}
