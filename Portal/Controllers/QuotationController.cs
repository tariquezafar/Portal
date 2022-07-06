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
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class QuotationController : BaseController
    {
        #region Quotation

        //
        // GET: /Quotation/
      
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Quotation, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditQuotation(int quotationId = 0, int accessMode = 3,int customerId=0,string customerCode="",string customerName="")
        {
            try
            {     
                         
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

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
                    ViewData["customerId"] = customerId;
                    ViewData["customerCode"] = customerCode;                    
                    ViewData["customerName"] = customerName;
                   

                } 

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
         
        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Quotation, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditQuotation(QuotationViewModel quotationViewModel, List<QuotationProductViewModel> quotationProducts, List<QuotationTaxViewModel> quotationTaxes, List<QuotationTermViewModel> quotationTerms, List<QuotationSupportingDocumentViewModel> quotationDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            QuotationBL quotationBL = new QuotationBL();
            try
            {
                if (quotationViewModel != null)
                {
                    quotationViewModel.CreatedBy = ContextUser.UserId;
                    quotationViewModel.CompanyId = ContextUser.CompanyId;
                    quotationViewModel.FinYearId= Session[SessionKey.CurrentFinYear]!=null?((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId:DateTime.Now.Year;
                    responseOut = quotationBL.AddEditQuotation(quotationViewModel, quotationProducts, quotationTaxes,quotationTerms, quotationDocuments);
                    
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Quotation, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListQuotation()
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
        public PartialViewResult GetQuotationList(string quotationNo = "",string customerName = "", string refNo="", string fromDate="", string toDate="",string displayType="" ,string approvalStatus="", int companyBranchId = 0,int LocationId=0)
        {
            List<QuotationViewModel> quotations = new List<QuotationViewModel>();
            QuotationBL quotationBL = new QuotationBL();
            try
            {
                quotations = quotationBL.GetQuotationList(quotationNo, customerName, refNo, fromDate, toDate, ContextUser.CompanyId, displayType, approvalStatus, companyBranchId,Convert.ToInt32(Session[ SessionKey.CustomerId]),LocationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(quotations);
        }

        [HttpGet]
        public JsonResult GetQuotationDetail(long quotationId)
        {
            QuotationBL quotationBL = new QuotationBL();
            QuotationViewModel quotation = new QuotationViewModel();
            try
            {
                quotation = quotationBL.GetQuotationDetail(quotationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(quotation, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetQuotationProductList(List<QuotationProductViewModel> quotationProducts, long quotationId)
        {
            QuotationBL quotationBL = new QuotationBL();
            try
            {
                if (quotationProducts == null)
                {
                    quotationProducts = quotationBL.GetQuotationProductList(quotationId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(quotationProducts);
        }
       
        [HttpPost]
        public PartialViewResult GetQuotationTaxList(List<QuotationTaxViewModel> quotationTaxes, long quotationId)
        {
            QuotationBL quotationBL = new QuotationBL();
            try
            {
                if (quotationTaxes == null)
                {
                    quotationTaxes = quotationBL.GetQuotationTaxList(quotationId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(quotationTaxes);
        }
        [HttpPost]
        public PartialViewResult GetQuotationTermList(List<QuotationTermViewModel> quotationTerms, long quotationId)
        {
            QuotationBL quotationBL = new QuotationBL();
            try
            {
                if (quotationTerms == null)
                {
                    quotationTerms = quotationBL.GetQuotationTermList(quotationId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(quotationTerms);
        }


        [HttpGet]
        public JsonResult GetCustomerAutoCompleteList(string term)
        {
            CustomerBL customerBL = new CustomerBL();

            List<CustomerViewModel> customerList = new List<CustomerViewModel>();
            try
            {
                customerList = customerBL.GetCustomerAutoCompleteList(term,ContextUser.CompanyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTaxAutoCompleteList(string term)
        {
            TaxBL taxBL = new TaxBL();

            List<TaxViewModel> taxList = new List<TaxViewModel>();
            try
            {
                taxList = taxBL.GetTaxAutoCompleteList(term,"SALE", ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(taxList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
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

       public ActionResult Report(long quotationId,string reportType= "PDF")
        {
            LocalReport lr = new LocalReport();
            QuotationBL quotationBL = new QuotationBL();

            DataTable dt = new DataTable();
            dt = quotationBL.GetQuotationDetailDataTable(quotationId);
            string BranchType = dt.Rows[0]["BranchType"].ToString();

            string path = "";
            decimal totalInvoiceAmount = 0;


            totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
            string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));

            if (BranchType == "Showroom")
            {
                path = Path.Combine(Server.MapPath("~/RDLC"), "QuotationPrint.rdlc");
            }
            else
            {
                path = Path.Combine(Server.MapPath("~/RDLC"), "QuotationWithoutShoroomPrint.rdlc");
            }


               
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdProduct = new ReportDataSource("DataSetProduct", quotationBL.GetQuotationProductListDataTable(quotationId));
            ReportDataSource rdTerms = new ReportDataSource("DataSetTerms", quotationBL.GetQuotationTermListDataTable(quotationId));
            ReportDataSource rdTax = new ReportDataSource("DataSetTax", quotationBL.GetQuotationTaxListDataTable(quotationId));
            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            lr.DataSources.Add(rdTerms);
            lr.DataSources.Add(rdTax);

            ReportParameter rp1 = new ReportParameter("AmountInWords", totalWords);
            lr.SetParameters(rp1);
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
        public ActionResult QuotationMail(long quotationId, string reportType = "PDF")
        {
            ResponseOut responseOut = new ResponseOut();
            LocalReport lr = new LocalReport();
            QuotationBL quotationBL = new QuotationBL();
            EmailTemplateBL emailTemplateBL = new EmailTemplateBL();
            try
            {  
                string path = Path.Combine(Server.MapPath("~/RDLC"), "QuotationPrint.rdlc");
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
                emaildt = emailTemplateBL.GetEmailTemplateDetailByEmailType((int)MailSendingMode.Quotation);

                DataTable dt = new DataTable();
                dt = quotationBL.GetQuotationDetailDataTable(quotationId);

                if (dt.Rows.Count > 0)
                {
                    StringBuilder mailBody = new StringBuilder(" ");
                    SendMail sendMail = new SendMail();
                    mailBody.Append("<html><head></head><body>");
                    //mailBody.Append("<img src='" + Convert.ToString(ConfigurationManager.AppSettings["Logo_Path"]) + "' alt='ICS Logo' />");
                    //mailBody.Append("<hr/><br/>");
                   mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Dear " + dt.Rows[0]["ContactPersonName"].ToString() + " </p><br/>");
                    /*mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Please find attached Quotation for your reference</p><br/>");
                   mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Regards,</p><br/>");
                   mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Sale Team</p><br/>");
                   
                    mailBody.Append(emaildt.Rows[0][4].ToString());*/
                    mailBody.Append("</body></html>");

                    ReportDataSource rd = new ReportDataSource("DataSet1", quotationBL.GetQuotationDetailDataTable(quotationId));
                    ReportDataSource rdProduct = new ReportDataSource("DataSetProduct", quotationBL.GetQuotationProductListDataTable(quotationId));
                    ReportDataSource rdTerms = new ReportDataSource("DataSetTerms", quotationBL.GetQuotationTermListDataTable(quotationId));
                    ReportDataSource rdTax = new ReportDataSource("DataSetTax", quotationBL.GetQuotationTaxListDataTable(quotationId));
                    lr.DataSources.Add(rd);
                    lr.DataSources.Add(rdProduct);
                    lr.DataSources.Add(rdTerms);
                    lr.DataSources.Add(rdTax);
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    

                    string deviceInfo =

                      "<DeviceInfo>" +
                        "<OutputFormat>" + reportType + "</OutputFormat>" +
                        "<PageWidth>8.5in</PageWidth>" +
                        "<PageHeight>11in</PageHeight>" +
                        "<MarginTop>0.50in</MarginTop>" +
                        "<MarginLeft>.1in</MarginLeft>" +
                        "<MarginRight>.1in</MarginRight>" +
                        "<MarginBottom>0.5in</MarginBottom>" +
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
                    // bool sendMailStatus= sendMail.SendEmail("", dt.Rows[0]["Email"].ToString(), "Quotation", mailBody.ToString(), renderedBytes, "Quotation.pdf");

                    if (userEmailSetting.Rows.Count > 0)
                    {
                        sendMailStatus = sendMail.SendEmail(userEmailSetting.Rows[0]["SmtpUser"].ToString(), dt.Rows[0]["Email"].ToString(), emaildt.Rows[0]["EmailTemplateSubject"].ToString(), mailBody.ToString(), renderedBytes, "Quotation.pdf", userEmailSetting.Rows[0]["SmtpPass"].ToString(), userEmailSetting.Rows[0]["SmtpDisplayName"].ToString(), userEmailSetting.Rows[0]["SmtpServer"].ToString(), Convert.ToInt32(userEmailSetting.Rows[0]["SmtpPort"]), Convert.ToBoolean(userEmailSetting.Rows[0]["EnableSsl"]));
                    }
                    else
                    {
                        sendMailStatus = sendMail.SendEmail("", dt.Rows[0]["Email"].ToString(), emaildt.Rows[0]["EmailTemplateSubject"].ToString(), mailBody.ToString(), renderedBytes, "Quotation.pdf");
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
                    responseOut.message = "Quotation Detail not found!!!";
                    responseOut.status = ActionStatus.Fail;
                    
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
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
                        var path = Path.Combine(Server.MapPath("~/Images/QuotationDocument"), newFileName);
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
            var path = Path.Combine(Server.MapPath("~/Images/QuotationDocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public PartialViewResult GetQuotationSupportingDocumentList(List<QuotationSupportingDocumentViewModel> quotationDocuments, long quotationId)
        {


         
            QuotationBL quotationBL = new QuotationBL();
            try
            {
                if (quotationDocuments == null)
                {
                    quotationDocuments = quotationBL.GetQuotationSupportingDocumentList(quotationId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(quotationDocuments);
        }

        [HttpGet]
        public JsonResult GetProductTaxPercentage(long productId=0)
        {
            ProductBL productBL = new ProductBL();
            ProductViewModel productViewModel = new ProductViewModel();           
            try
            {
                productViewModel = productBL.GetProductTaxPercentage(productId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productViewModel, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Revised Quotation


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Quotation, (int)AccessMode.ReviseAccess, (int)RequestMode.GetPost)]
        public ActionResult AddRevisedQuotation(int quotationId = 0, int accessMode = 5)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (quotationId != 0)
                {
                    ViewData["quotationId"] = quotationId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["quotationId"] = 0;
                    ViewData["accessMode"] = 3;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Quotation, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddRevisedQuotation(QuotationViewModel quotationViewModel, List<QuotationProductViewModel> quotationProducts, List<QuotationTaxViewModel> quotationTaxes, List<QuotationTermViewModel> quotationTerms, List<QuotationSupportingDocumentViewModel> revisedQuotationDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            QuotationBL quotationBL = new QuotationBL();
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                if (quotationViewModel != null)
                {
                    quotationViewModel.CreatedBy = ContextUser.UserId;
                    quotationViewModel.CompanyId = ContextUser.CompanyId;
                    quotationViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = quotationBL.AddRevisedQuotation(quotationViewModel, quotationProducts, quotationTaxes, quotationTerms, revisedQuotationDocuments);

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
        public JsonResult GetCurrencyList()
        {
            CurrencyBL currencyBL = new CurrencyBL();
            List<CurrencyViewModel> currencyList = new List<CurrencyViewModel>();
            try
            {
                currencyList = currencyBL.GetCurrencyList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(currencyList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SaveRevisedSupportingDocument()
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
                        var path = Path.Combine(Server.MapPath("~/Images/RevisedQuotationDocument"), newFileName);
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
        public FileResult RevisedDocumentDownload(string fileName)
        {
            var path = Path.Combine(Server.MapPath("~/Images/RevisedQuotationDocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        #endregion



    }
}
