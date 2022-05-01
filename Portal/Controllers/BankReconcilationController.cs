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
    public class BankReconcilationController : BaseController
    {
        //
        // GET: /BankReconcilation/

        #region Bank Reconcilation

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_BankReconcilation, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditBankReconcilation(int bankRecoID = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                ViewData["monthStartDate"] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd-MMM-yyyy");
                ViewData["monthEndDate"] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, daysInMonth).ToString("dd-MMM-yyyy");
                if (bankRecoID != 0)
                {
                    ViewData["bankRecoID"] = bankRecoID;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["bankRecoID"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_BankReconcilation, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditBankReconcilation(BankReconcilationViewModel bankReconcilation, List<BankReconcilationDetailViewModel> bankReconcilationDetails, BankReconcilationSummaryViewModel bankReconcilationSummary)
        {
            ResponseOut responseOut = new ResponseOut();
            BankReconcilationBL bankReconcilationBL = new BankReconcilationBL();
             
            try
            {
                if (bankReconcilation != null)
                {
                    bankReconcilation.CreatedBy = ContextUser.UserId;
                    bankReconcilation.CompanyId = ContextUser.CompanyId;
                    responseOut = bankReconcilationBL.AddEditBankReconcilation(bankReconcilation, bankReconcilationDetails, bankReconcilationSummary);
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

        public JsonResult GetBankReconcilationDetail(int bankRecoId)
        {
            BankReconcilationBL bankReconcilationBL = new BankReconcilationBL();
            BankReconcilationViewModel bankReconcilation = new BankReconcilationViewModel();
            try
            {
                bankReconcilation = bankReconcilationBL.GetBankReconcilationDetail(bankRecoId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(bankReconcilation, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetBankReconcilationList1( int bankBookId = 0, string fromDate = "", string toDate = "", int companyBranchId = 0,string trnType= "")
        {
            List<BankReconcilationDetailViewModel> bankStatements = new List<BankReconcilationDetailViewModel>();

            BankReconcilationBL bankReconcilationBL = new BankReconcilationBL();
             
            try
            {
                bankStatements = bankReconcilationBL.GetBankReconcilationDetailList(bankBookId, fromDate, toDate, ContextUser.CompanyId, companyBranchId,trnType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(bankStatements);
        }
        public PartialViewResult GetBankReconcilationList2(int bankBookId = 0, string fromDate = "", string toDate = "", int companyBranchId = 0, string trnType = "")
        {
            List<BankReconcilationDetailViewModel> bankStatements = new List<BankReconcilationDetailViewModel>();

            BankReconcilationBL bankReconcilationBL = new BankReconcilationBL();

            try
            {
                bankStatements = bankReconcilationBL.GetBankReconcilationDetailList(bankBookId, fromDate, toDate, ContextUser.CompanyId, companyBranchId, trnType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(bankStatements);
        }
        public PartialViewResult GetBankReconcilationList3(int bankBookId = 0, string fromDate = "", string toDate = "", int companyBranchId = 0, string trnType = "")
        {
            List<BankReconcilationDetailViewModel> bankStatements = new List<BankReconcilationDetailViewModel>();

            BankReconcilationBL bankReconcilationBL = new BankReconcilationBL();

            try
            {
                bankStatements = bankReconcilationBL.GetBankReconcilationDetailList(bankBookId, fromDate, toDate, ContextUser.CompanyId, companyBranchId, trnType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(bankStatements);
        }
        public PartialViewResult GetBankReconcilationList4(int bankBookId = 0, string fromDate = "", string toDate = "", int companyBranchId = 0, string trnType = "")
        {
            List<BankReconcilationDetailViewModel> bankStatements = new List<BankReconcilationDetailViewModel>();

            BankReconcilationBL bankReconcilationBL = new BankReconcilationBL();

            try
            {
                bankStatements = bankReconcilationBL.GetBankReconcilationDetailList(bankBookId, fromDate, toDate, ContextUser.CompanyId, companyBranchId, trnType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(bankStatements);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_BankReconcilation, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListBankReconcilation()
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


        public PartialViewResult GetBankReconcilationList(string bankRecoNo = "", int bankBookId = 0, int companyBranchId = 0, string fromDate = "", string toDate = "", string bankRecoStatus = "")
        {
            List<BankReconcilationViewModel> bankReconcilations = new List<BankReconcilationViewModel>();

            BankReconcilationBL bankReconcilationBL = new BankReconcilationBL();
            try
            {
                bankReconcilations = bankReconcilationBL.GetBankReconcilationList(bankRecoNo, bankBookId, companyBranchId, fromDate, toDate, ContextUser.CompanyId, bankRecoStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(bankReconcilations);
        }

        [HttpGet]
        public JsonResult GetBankClosingBalance(int bookId=0, string fromDate="", string ToDate="")
        {
            BankReconcilationBL bankReconcilationBL = new BankReconcilationBL();
            decimal bankClosingBalance = 0;
            try
            {
                int finYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                bankClosingBalance = bankReconcilationBL.GetBankClosingBalance(bookId,fromDate,ToDate, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(bankClosingBalance, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Report(int bankRecoID = 0, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();

            BankReconcilationBL bankReconcilationBL = new BankReconcilationBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "BankReconcilationReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable dtReconcilation = new DataTable();
            dtReconcilation = bankReconcilationBL.GetBankReconcilationDetailDataTable(bankRecoID);

            //decimal totalInvoiceAmount = 0;
            //totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
            //string totalWords = CommonHelper.changeToWords(totalInvoiceAmount.ToString());

            ReportDataSource rd = new ReportDataSource("BankReconcilationDataSet", dtReconcilation);
            ReportDataSource rdBankReconcilation = new ReportDataSource("BankReconcilationDetailListDataSet", bankReconcilationBL.GetBankReconcilationDetailListDataTable(bankRecoID));
            ReportDataSource rdBankReconcilationSummary = new ReportDataSource("BankReconcilationSummaryDataSet", bankReconcilationBL.GetBankReconcilationSummaryList(bankRecoID));
            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdBankReconcilation);
            lr.DataSources.Add(rdBankReconcilationSummary);

            // ReportParameter rp1 = new ReportParameter("AmountInWords", totalWords);
            //ReportParameter rp2 = new ReportParameter("ReportOption", reportOption);
            //lr.SetParameters(rp1);
            //lr.SetParameters(rp2);

            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo = "<DeviceInfo>" +
                       "  <OutputFormat>" + reportType + "</OutputFormat>" +
                       "  <PageWidth>9.0in</PageWidth>" +
                       "  <PageHeight>11in</PageHeight>" +
                       "  <MarginTop>0.50in</MarginTop>" +
                       "  <MarginLeft>.35in</MarginLeft>" +
                       "  <MarginRight>0.1in</MarginRight>" +
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
