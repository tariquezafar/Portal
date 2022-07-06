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
    public class SOController : BaseController
    {
        #region SO

        //
        // GET: /SO/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SO, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSO(int soId = 0, int accessMode = 0, int customerId = 0, string customerCode = "", string customerName = "")
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["RoleId"] = ContextUser.RoleId;
                if (soId != 0)
                {
                    ViewData["soId"] = soId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["soId"] = 0;
                    ViewData["accessMode"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SO, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditSO(SOViewModel soViewModel, List<SOProductViewModel> soProducts, List<SOTaxViewModel> soTaxes, List<SOTermViewModel> soTerms, List<SOSupportingDocumentViewModel> soDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            SOBL soBL = new SOBL();
            try
            {
                if (soViewModel != null)
                {
                    soViewModel.CreatedBy = ContextUser.UserId;
                    soViewModel.CompanyId = ContextUser.CompanyId;
                    soViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = soBL.AddEditSO(soViewModel, soProducts, soTaxes, soTerms, soDocuments);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SO, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSO(string listStatus = "false")
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
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
        public PartialViewResult GetSOList(string soNo = "", string customerName = "", string refNo = "", string fromDate = "", string toDate = "",string approvalStatus="",string CreatedByUserName="",int companyBranchId=0, string dashboardList = "", int LocationId=0)
        {
            List<SOViewModel> sos = new List<SOViewModel>();
            SOBL soBL = new SOBL();
            try
            {
                sos = soBL.GetSOList(soNo, customerName, refNo, fromDate, toDate, ContextUser.CompanyId, approvalStatus,"",CreatedByUserName,companyBranchId, dashboardList, Convert.ToInt32(Session[SessionKey.CustomerId]),LocationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(sos);
        }

        [HttpGet]
        public JsonResult GetSODetail(long soId)
        {
            SOBL soBL = new SOBL();
            SOViewModel so = new SOViewModel();
            try
            {
                so = soBL.GetSODetail(soId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(so, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult GetSOProductList(List<SOProductViewModel> soProducts, long soId)
        {
            SOBL soBL = new SOBL();
            try
            {
                if (soProducts == null)
                {
                    soProducts = soBL.GetSOProductList(soId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(soProducts);
        }
        [HttpPost]
        public PartialViewResult GetSOTaxList(List<SOTaxViewModel> soTaxes, long soId)
        {
            SOBL soBL = new SOBL();
            try
            {
                if (soTaxes == null)
                {
                    soTaxes = soBL.GetSOTaxList(soId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(soTaxes);
        }
        [HttpPost]
        public PartialViewResult GetSOTermList(List<SOTermViewModel> soTerms, long soId)
        {
            SOBL soBL = new SOBL();
            try
            {
                if (soTerms == null)
                {
                    soTerms = soBL.GetSOTermList(soId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(soTerms);
        }


        [HttpGet]
        public JsonResult GetCustomerAutoCompleteList(string term)
        {
            CustomerBL customerBL = new CustomerBL();

            List<CustomerViewModel> customerList = new List<CustomerViewModel>();
            try
            {
                customerList = customerBL.GetCustomerAutoCompleteList(term, ContextUser.CompanyId);

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
                taxList = taxBL.GetTaxAutoCompleteList(term, "SALE", ContextUser.CompanyId);
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

        [HttpGet]
        public PartialViewResult GetQuotationList(string quotationNo = "", string customerName = "", string refNo = "", string fromDate = "", string toDate = "", string displayType = "",string approvalStatus="",int companyBranchId=0)
        {
            List<QuotationViewModel> quotations = new List<QuotationViewModel>();
            QuotationBL quotationBL = new QuotationBL();
            try
            {
                quotations = quotationBL.GetQuotationList(quotationNo, customerName, refNo, fromDate, toDate, ContextUser.CompanyId, displayType,approvalStatus, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(quotations);
        }


        [HttpGet]
        public JsonResult GetCustomerBranchList(int customerId=0)
        {
            CustomerBL customerBL = new CustomerBL();
            List<CustomerBranchViewModel> customerBranchList = new List<CustomerBranchViewModel>();
            try
            {
                customerBranchList = customerBL.GetCustomerBranches(customerId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(customerBranchList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCustomerBranchDetail(long customerBranchId)
        {
            CustomerBL customerBL = new CustomerBL();
            CustomerBranchViewModel customerBranchList = new CustomerBranchViewModel();
            try
            {
                customerBranchList = customerBL.GetCustomerBranchDetail(customerBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(customerBranchList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetBookAutoCompleteList(string term,string bookType, int companyBranchId)
        {
            BookBL bookBL = new BookBL();

            List<BookViewModel> bookList = new List<BookViewModel>();
            try
            {
                bookList = bookBL.GetBookAutoCompleteList(term,bookType, ContextUser.CompanyId, companyBranchId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(bookList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Report(long soId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SOBL soBL = new SOBL();
          
            
            DataTable dt = soBL.GetSODetailDataTable(soId);
            DataTable dtProducts = soBL.GetSOProductListDataTable(soId);

            decimal totalInvoiceAmount = 0;
            string approvalstatus = "";
            int CGSTCount = 0;
            int IGSTCount = 0;

            totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
            string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));
            approvalstatus = dt.Rows[0]["ApprovalStatus"].ToString();

            string BranchType = dt.Rows[0]["BranchType"].ToString();
            string path = "";



            foreach (DataRow dr in dtProducts.Rows)
            {
                if (Convert.ToDecimal(dr["CGST_Amount"]) > 0)
                {
                    CGSTCount++;
                    break;
                }
                if (Convert.ToDecimal(dr["IGST_Amount"]) > 0)
                {
                    IGSTCount++;
                    break;
                }
            }

            if (BranchType == "Showroom")
            {
                if (CGSTCount > 0)
                {
                    path = Path.Combine(Server.MapPath("~/RDLC"), "SOBranchTypeWithoutIgstPrint.rdlc");
                }
                else
                {
                    path = Path.Combine(Server.MapPath("~/RDLC"), "SOBranchTypeWithIgstPrint.rdlc");
                }
            }
            else
            {
                if (CGSTCount > 0)
                {
                    
                    path = Path.Combine(Server.MapPath("~/RDLC"), "SOWithoutIgstPrint.rdlc");

                }
                else
                {
                    path = Path.Combine(Server.MapPath("~/RDLC"), "SOWithIgstPrint.rdlc");
                }

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
            ReportDataSource rdProduct = new ReportDataSource("DataSetProduct", dtProducts);
            ReportDataSource rdTax = new ReportDataSource("DataSetTax", soBL.GetSOTaxList(soId));
            ReportDataSource rdTerms = new ReportDataSource("DataSetTerms", soBL.GetSOTermList(soId));


            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            lr.DataSources.Add(rdTax);
            lr.DataSources.Add(rdTerms);

            ReportParameter rp1 = new ReportParameter("AmountInWords", totalWords);
            ReportParameter rp3 = new ReportParameter("approvalstatus", approvalstatus);
            lr.SetParameters(rp1);
            lr.SetParameters(rp3);
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

        public ActionResult SOMail(long soId, string reportType = "PDF")
        {
            ResponseOut responseOut = new ResponseOut();
            LocalReport lr = new LocalReport();
            SOBL soBL = new SOBL();
            EmailTemplateBL emailTemplateBL = new EmailTemplateBL();
            try
            { 
                string path = Path.Combine(Server.MapPath("~/RDLC"), "SOPrint.rdlc");
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
                emaildt = emailTemplateBL.GetEmailTemplateDetailByEmailType((int)MailSendingMode.SO);

                DataTable dt = new DataTable();
                dt = soBL.GetSODetailDataTable(soId);

                if (dt.Rows.Count > 0)
                {
                    StringBuilder mailBody = new StringBuilder(" ");
                    SendMail sendMail = new SendMail();
                    mailBody.Append("<html><head></head><body>");
                    //mailBody.Append("<img src='" + Convert.ToString(ConfigurationManager.AppSettings["Logo_Path"]) + "' alt='ICS Logo' />");
                    //mailBody.Append("<hr/><br/>");
                    mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Dear " + dt.Rows[0]["ContactPerson"].ToString() + " </p><br/>");
                    //mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Please find attached Proforma Invoice for your reference</p><br/>");
                    //mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Regards,</p><br/>");
                    mailBody.Append(emaildt.Rows[0][4].ToString());
                    mailBody.Append("</body></html>");

                    decimal totalInvoiceAmount = 0;
                    totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
                    string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));



                    ReportDataSource rd = new ReportDataSource("DataSet1", dt);
                    ReportDataSource rdProduct = new ReportDataSource("DataSetProduct", soBL.GetSOProductListDataTable(soId));
                    ReportDataSource rdTax = new ReportDataSource("DataSetTax", soBL.GetSOTaxList(soId));
                    ReportDataSource rdTerms = new ReportDataSource("DataSetTerms", soBL.GetSOTermList(soId));

                    lr.DataSources.Add(rd);
                    lr.DataSources.Add(rdProduct);
                    lr.DataSources.Add(rdTax);
                    lr.DataSources.Add(rdTerms);

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

                    UserEmailSettingBL userEmailSettingBL = new UserEmailSettingBL();
                    DataTable userEmailSetting = userEmailSettingBL.GetUserEmailSettingDetailDataTable(ContextUser.UserId);
                    bool sendMailStatus = false;

                    // bool sendMailStatus = sendMail.SendEmail("", dt.Rows[0]["Email"].ToString(), "Proforma Invoice", mailBody.ToString(), renderedBytes, "PI.pdf");

                    if (userEmailSetting.Rows.Count > 0)
                    {
                        sendMailStatus = sendMail.SendEmail(userEmailSetting.Rows[0]["SmtpUser"].ToString(), dt.Rows[0]["Email"].ToString(), "Sale Order", mailBody.ToString(), renderedBytes, "SO.pdf", userEmailSetting.Rows[0]["SmtpPass"].ToString(), userEmailSetting.Rows[0]["SmtpDisplayName"].ToString(), userEmailSetting.Rows[0]["SmtpServer"].ToString(), Convert.ToInt32(userEmailSetting.Rows[0]["SmtpPort"]), Convert.ToBoolean(userEmailSetting.Rows[0]["EnableSsl"]));
                    }
                    else
                    {
                        sendMailStatus = sendMail.SendEmail("", dt.Rows[0]["Email"].ToString(), "Sale Order", mailBody.ToString(), renderedBytes, "SO.pdf");
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
                    responseOut.message = "SO Detail not found!!!";
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
                        var path = Path.Combine(Server.MapPath("~/Images/SODocument"), newFileName);
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
            var path = Path.Combine(Server.MapPath("~/Images/SODocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public PartialViewResult GetSOSupportingDocumentList(List<SOSupportingDocumentViewModel> soDocuments, long soId)
        {
 
            SOBL sOBL = new SOBL();
            try
            {
                if (soDocuments == null)
                {
                    soDocuments = sOBL.GetSOSupportingDocumentList(soId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(soDocuments);
        }

        [HttpGet]
        public JsonResult GetCompanyBranchDetails(int companyBranchID)
        {
            CompanyBL companyBL = new CompanyBL();
            CompanyBranchViewModel companyBranchList = new CompanyBranchViewModel();
            try
            {
                companyBranchList = companyBL.GetCompanyBranchDetails(companyBranchID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(companyBranchList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetSOQuotationProductList(List<QuotationProductViewModel> quotationProducts, long quotationId)
        {
            SOBL soBL = new SOBL();
            try
            {
                if (quotationProducts == null)
                {
                    quotationProducts = soBL.GetSOQuotationProductList(quotationId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(quotationProducts);
        }
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SO, (int)AccessMode.CancelAccess, (int)RequestMode.GetPost)]
        public ActionResult CancelSO(int soId = 0, int accessMode = 4)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (soId != 0)
                {
                    ViewData["soId"] = soId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["soId"] = 0;
                    ViewData["accessMode"] = 3;

                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }


        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SO, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult CancelSO(long soId, string sONo, string cancelReason)
        {
            ResponseOut responseOut = new ResponseOut();
            SOBL sOBL = new SOBL();
            SOViewModel sOViewModel = new SOViewModel();
            try
            {
                if (sOViewModel != null)
                {
                    sOViewModel.SOId = soId;
                    sOViewModel.SONo = sONo;
                    sOViewModel.CancelReason = cancelReason;
                    sOViewModel.CreatedBy = ContextUser.UserId;
                    sOViewModel.CompanyId = ContextUser.CompanyId;
                    sOViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = sOBL.CancelSO(sOViewModel);
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
