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
    public class PurchaseInvoiceController :BaseController
    {
        //
        // GET: /PurchaseInvoice/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PI, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPI(int invoiceId = 0, int accessMode = 3)
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
                    ViewData["InvoiceId"] = invoiceId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["InvoiceId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PI, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPI(PurchaseInvoiceViewModel piViewModel, List<PurchaseInvoiceProductDetailViewModel> piProducts, List<PurchaseInvoiceTaxDetailViewModel> piTaxes, List<PurchaseInvoiceTermsDetailViewModel> piTerms, List<PISupportingDocumentViewModel> piDocuments, List<PurchaseInvoiceChasisDetailViewModel> piChasisLists)
        {
            ResponseOut responseOut = new ResponseOut();
           
            PurchaseInvoiceBL purchaseInvoiceBL = new PurchaseInvoiceBL();
            try
            {
                if (piViewModel != null)
                {
                    piViewModel.CreatedBy = ContextUser.UserId;
                    piViewModel.CompanyId = ContextUser.CompanyId;
                    piViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = purchaseInvoiceBL.AddEditPI(piViewModel, piProducts, piTaxes, piTerms, piDocuments, piChasisLists);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PI, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPI(string listStatus = "false")
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
        public PartialViewResult GetPIList(string piNo = "", string vendorName = "", string refNo = "", string fromDate = "", string toDate = "",string approvalStatus="", string displayType="", string vendorCode = "",string purchaseType= "",string CreatedByUserName="",string poNo="",string companyBranch="")
        {
            List<PurchaseInvoiceViewModel> pis = new List<PurchaseInvoiceViewModel>();
            PurchaseInvoiceBL piBL = new PurchaseInvoiceBL();
            try
            {
                pis = piBL.GetPIList(piNo, vendorName, refNo, fromDate, toDate, ContextUser.CompanyId, approvalStatus, displayType, vendorCode, purchaseType, CreatedByUserName, poNo, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pis);
        }

        [HttpPost]
        public PartialViewResult GetPIProductList(List<PurchaseInvoiceProductDetailViewModel> piProducts, long invoiceId)
        {
            PurchaseInvoiceBL piBL = new PurchaseInvoiceBL();
            try
            {
                if (piProducts == null)
                {
                    piProducts = piBL.GetPIProductList(invoiceId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(piProducts);
        }

        [HttpPost]
        public PartialViewResult GetPIMRNProductList(List<PurchaseInvoiceProductDetailViewModel> piProducts, long invoiceId)
        {
            PurchaseInvoiceBL piBL = new PurchaseInvoiceBL();
            try
            {
                if (piProducts == null)
                {
                    piProducts = piBL.GetPIMRNProductList(invoiceId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(piProducts);
        }

        [HttpPost]
        public PartialViewResult GetPITaxList(List<PurchaseInvoiceTaxDetailViewModel> piTaxes, long invoiceId)
        {

            PurchaseInvoiceBL piBL = new PurchaseInvoiceBL();
            try
            {
                if (piTaxes == null)
                {
                    piTaxes = piBL.GetPITaxList(invoiceId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(piTaxes);
        }
        [HttpPost]
        public PartialViewResult GetPITermList(List<PurchaseInvoiceTermsDetailViewModel> piTerms, long invoiceId)
        {
            PurchaseInvoiceBL piBL = new PurchaseInvoiceBL();
            try
            {
                if (piTerms == null)
                {
                    piTerms = piBL.GetPITermList(invoiceId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(piTerms);
        }

        [HttpGet]
        public JsonResult GetVendorAutoCompleteList(string term)
        {
            CustomerBL customerBL = new CustomerBL();
            VendorBL vendorBL = new VendorBL();
            List<VendorViewModel> vendorList = new List<VendorViewModel>();
            try
            {
                vendorList = vendorBL.GetVendorAutoCompleteList(term, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(vendorList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTaxAutoCompleteList(string term)
        {
            TaxBL taxBL = new TaxBL();

            List<TaxViewModel> taxList = new List<TaxViewModel>();
            try
            {
                taxList = taxBL.GetTaxAutoCompleteList(term, "SALE", ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(taxList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetTermTemplateList()
        {
            TermTemplateBL termBL = new TermTemplateBL();
            List<TermTemplateViewModel> termList = new List<TermTemplateViewModel>();
            try
            {
                termList = termBL.GetTermTemplateList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(termList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTermTemplateDetailList(int termTemplateId)
        {
            TermTemplateBL termBL = new TermTemplateBL();
            List<TermTemplateDetailViewModel> termList = new List<TermTemplateDetailViewModel>();
            try
            {
                termList = termBL.GetTermTemplateDetailList(termTemplateId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(termList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPIDetail(long invoiceId)
        {
            PurchaseInvoiceBL piBL = new PurchaseInvoiceBL();
            PurchaseInvoiceViewModel pi = new PurchaseInvoiceViewModel(); 
            try
            {
                pi = piBL.GetPIDetail(invoiceId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(pi, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetPurchaseOrderList(string poNo = "", string vendorName = "", string refNo = "", string fromDate = "", string toDate = "", string approvalStatus = "", string displayType = "",string CreatedByUserName= "", int companyBranch = 0, string poType = "0")
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

        public ActionResult Report(long piId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();

            PurchaseInvoiceBL purchaseInvoiceBL = new PurchaseInvoiceBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "PIPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable dt = new DataTable();
            dt = purchaseInvoiceBL.GetPIDetailDataTable(piId);

            decimal totalInvoiceAmount = 0;
            string approvalstatus = "";
            totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
            string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));
            approvalstatus = dt.Rows[0]["ApprovalStatus"].ToString();

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdProduct = new ReportDataSource("DataSetPIProducts", purchaseInvoiceBL.GetPIProductListDataTable(piId));
            ReportDataSource rdTax = new ReportDataSource("DataSetPITax", purchaseInvoiceBL.GetPITaxDataTable(piId));
            ReportDataSource rdTerms = new ReportDataSource("DataSetPITerms", purchaseInvoiceBL.GetPITermListDataTable(piId));


            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            lr.DataSources.Add(rdTax);
            lr.DataSources.Add(rdTerms);

            ReportParameter rp1 = new ReportParameter("AmountInWords", totalWords);
            //ReportParameter rp2 = new ReportParameter("ReportOption", reportOption);
            ReportParameter rp3 = new ReportParameter("approvalstatus", approvalstatus);
            lr.SetParameters(rp1);
            //lr.SetParameters(rp2);
            lr.SetParameters(rp3);

            string mimeType;
            string encoding;
            string fileNameExtension;



              string deviceInfo = "<DeviceInfo>" +
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

        [HttpPost]
        public PartialViewResult GetPISupportingDocumentList(List<PISupportingDocumentViewModel> piDocuments, long piId)
        {

            PurchaseInvoiceBL purchaseInvoiceBL = new PurchaseInvoiceBL();
            try
            {
                if (piDocuments == null)
                {
                    piDocuments = purchaseInvoiceBL.GetPISupportingDocumentList(piId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(piDocuments);
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
                        var path = Path.Combine(Server.MapPath("~/Images/PODocument"), newFileName);
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
            var path = Path.Combine(Server.MapPath("~/Images/PODocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        } 

        [HttpGet]
        public ActionResult PurchaseInvoiceMail(long piId, string reportType = "PDF")
        {
            ResponseOut responseOut = new ResponseOut();
            LocalReport lr = new LocalReport();
            PurchaseInvoiceBL purchaseInvoiceBL = new PurchaseInvoiceBL();
            EmailTemplateBL emailTemplateBL = new EmailTemplateBL();
            try
            {
                string path = Path.Combine(Server.MapPath("~/RDLC"), "PIPrint.rdlc");
                if (System.IO.File.Exists(path))
                {
                    lr.ReportPath = path;
                }
                else
                {
                    responseOut.message = "Report Path not Found!!!";
                    responseOut.status = ActionStatus.Fail;
                    return Json(responseOut, JsonRequestBehavior.AllowGet);
                }
                DataTable emaildt = new DataTable();
                emaildt = emailTemplateBL.GetEmailTemplateDetailByEmailType((int)MailSendingMode.PurchaseInvoice);

                DataTable dt = new DataTable();
                dt = purchaseInvoiceBL.GetPIDetailDataTable(piId);

                if (dt.Rows.Count > 0)
                {
                    StringBuilder mailBody = new StringBuilder(" ");
                    SendMail sendMail = new SendMail();
                    mailBody.Append("<html><head></head><body>");
                    //mailBody.Append("<img src='" + Convert.ToString(ConfigurationManager.AppSettings["Logo_Path"]) + "' alt='ICS Logo' />");
                    //mailBody.Append("<hr/><br/>");
                    mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Dear " + dt.Rows[0]["ContactPersonName"].ToString() + " </p><br/>");
                    //  mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Please find attached Purchase Invoice for your reference</p><br/>");
                    //  mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Regards,</p><br/>");
                    // mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Sale Team</p><br/>");
                    mailBody.Append(emaildt.Rows[0][4].ToString());
                    mailBody.Append("</body></html>");

                    decimal totalInvoiceAmount = 0;
                    totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
                    string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));

                    ReportDataSource rd = new ReportDataSource("DataSet1", dt);
                    ReportDataSource rdProduct = new ReportDataSource("DataSetPIProducts", purchaseInvoiceBL.GetPIProductListDataTable(piId));
                    ReportDataSource rdTax = new ReportDataSource("DataSetPITax", purchaseInvoiceBL.GetPITaxDataTable(piId));
                    ReportDataSource rdTerms = new ReportDataSource("DataSetPITerms", purchaseInvoiceBL.GetPITermListDataTable(piId));

                    lr.DataSources.Add(rd);
                    lr.DataSources.Add(rdProduct);
                    lr.DataSources.Add(rdTax);
                    lr.DataSources.Add(rdTerms);

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

                    UserEmailSettingBL userEmailSettingBL = new UserEmailSettingBL();
                    DataTable userEmailSetting = userEmailSettingBL.GetUserEmailSettingDetailDataTable(ContextUser.UserId);
                    bool sendMailStatus = false;
                    if (userEmailSetting.Rows.Count > 0)
                    {
                        sendMailStatus = sendMail.SendEmail(userEmailSetting.Rows[0]["SmtpUser"].ToString(), dt.Rows[0]["Email"].ToString(), "Purchase Invoice", mailBody.ToString(), renderedBytes, "PurchaseInvoice.pdf", userEmailSetting.Rows[0]["SmtpPass"].ToString(), userEmailSetting.Rows[0]["SmtpDisplayName"].ToString(), userEmailSetting.Rows[0]["SmtpServer"].ToString(), Convert.ToInt32(userEmailSetting.Rows[0]["SmtpPort"]), Convert.ToBoolean(userEmailSetting.Rows[0]["EnableSsl"]));
                    }
                    else
                    {
                        sendMailStatus = sendMail.SendEmail("", dt.Rows[0]["Email"].ToString(), "Purchase Invoice", mailBody.ToString(), renderedBytes, "PurchaseInvoice.pdf");
                    }

                    if (sendMailStatus)
                    {
                        responseOut.message = "Mail Sent Successfully";
                        responseOut.status = ActionStatus.Success;


                    }
                    else
                    {
                        responseOut.message = "Problem in Sending Mail!!!";
                        responseOut.status = ActionStatus.Fail;

                    }
                }
                else
                {
                    responseOut.message = "Purchase Invoice Detail not found!!!";
                    responseOut.status = ActionStatus.Fail;

                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PI, (int)AccessMode.CancelAccess, (int)RequestMode.GetPost)] 
        public ActionResult CancelPI(int invoiceId = 0, int accessMode = 4)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (invoiceId != 0)
                {
                    ViewData["invoiceId"] = invoiceId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["invoiceId"] = 0;
                    ViewData["accessMode"] =0;

                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
         
        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PI, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult CancelPI(long invoiceId, string invoiceNo, string cancelReason)
        {
            ResponseOut responseOut = new ResponseOut(); 
            PurchaseInvoiceBL purchaseInvoiceBL = new PurchaseInvoiceBL();
            PurchaseInvoiceViewModel piViewModel = new PurchaseInvoiceViewModel();
            try
            {
                if (piViewModel != null)
                {
                    piViewModel.InvoiceId = invoiceId;
                    piViewModel.InvoiceNo = invoiceNo;
                    piViewModel.CancelReason = cancelReason;
                    piViewModel.CreatedBy = ContextUser.UserId;
                    piViewModel.CompanyId = ContextUser.CompanyId;
                    piViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = purchaseInvoiceBL.CancelPI(piViewModel);
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

        [HttpGet]
        public PartialViewResult GetPorductPurchaseList(long productId,long companyBranchId)
        {
            List<PurchaseInvoiceViewModel> pis = new List<PurchaseInvoiceViewModel>();
            PurchaseInvoiceBL purchaseInvoiceBL = new PurchaseInvoiceBL();           
            try
            {
                pis = purchaseInvoiceBL.GetPIPrpductPurchaseList(productId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pis);
        }

        //For PI Partil Against PO By Dheeraj Kumar
        [HttpPost]
        public PartialViewResult GetPOProductList(List<POProductViewModel> poProducts, long poId)
        {
            POBL poBL = new POBL();
            try
            {
                if (poProducts == null)
                {
                    poProducts = poBL.GetPIPOProductList(poId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(poProducts);
        }


        [HttpGet]
        public PartialViewResult GetSIList(string invoiceNo = "", string customerName = "", string refNo = "", string fromDate = "", string toDate = "",string displayType = "", string approvalStatus = "", int companyBranch = 0, string CreatedByUserName = "")
        {
            List<SaleInvoiceViewModel> saleinvoices = new List<SaleInvoiceViewModel>();
            SaleInvoiceBL saleinvoiceBL = new SaleInvoiceBL();
            try
            {
                saleinvoices = saleinvoiceBL.GetSIList(invoiceNo, customerName, refNo, fromDate, toDate, ContextUser.CompanyId, displayType,  approvalStatus, companyBranch, CreatedByUserName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(saleinvoices);
        }

        [HttpPost]
        public PartialViewResult GetSIChaisList(List<SaleInvoiceProductSerialDetailViewModel> siChasis, long invoiceId = 0,string mode="")
        {
            PurchaseInvoiceBL piBL = new PurchaseInvoiceBL();
            try
            {
                if (siChasis == null)
                {
                    siChasis = piBL.GetSIChaisList(invoiceId, mode);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(siChasis);
        }


        [HttpPost]
        public PartialViewResult GetSaleInvoiceProductList(List<SaleInvoiceProductViewModel> saleinvoiceProducts, long saleinvoiceId)
        {
            SaleInvoiceBL siBL = new SaleInvoiceBL();
            try
            {
                if (saleinvoiceProducts == null)
                {
                    saleinvoiceProducts = siBL.GetSaleInvoiceProductList(saleinvoiceId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(saleinvoiceProducts);
        }
    }
}
