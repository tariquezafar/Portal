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
    public class DeliveryChallanController : BaseController
    {
        #region DeliveryChallan

        //
        // GET: /DeliveryChallan/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DeliveryChallan, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditDeliveryChallan(int challanId = 0, int accessMode = 3)
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
               
                if (challanId != 0)
                {
                    ViewData["challanId"] = challanId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["challanId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DeliveryChallan, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditDeliveryChallan(DeliveryChallanViewModel challanViewModel, List<ChallanProductViewModel> challanProducts, List<ChallanTaxViewModel> challanTaxes, List<ChallanTermsViewModel> challanTerms, List<DeliveryChallanSupportingDocumentViewModel> deliveryChallanDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            DeliveryChallanBL deliverychallanBL = new DeliveryChallanBL();
            try
            {
                if (challanViewModel != null)
                {
                    challanViewModel.CreatedBy = ContextUser.UserId;
                    challanViewModel.CompanyId = ContextUser.CompanyId;
                    challanViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = deliverychallanBL.AddEditDeliveryChallan(challanViewModel, challanProducts, challanTaxes, challanTerms, deliveryChallanDocuments);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_DeliveryChallan, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListChallan()
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


        [HttpGet]
        public PartialViewResult GetChallanList(string challanNo = "",string invoiceno="", string customerName = "", string dispatchrefNo = "", string fromDate = "", string toDate = "",string approvalStatus="",string createdByUserName="",int companyBranchId=0)
        {
            List<DeliveryChallanViewModel> deliverychallans = new List<DeliveryChallanViewModel>();
            DeliveryChallanBL deliverychallanBL = new DeliveryChallanBL();
            try
            {
                deliverychallans = deliverychallanBL.GetChallanList(challanNo,customerName, dispatchrefNo, fromDate, toDate, approvalStatus, ContextUser.CompanyId,invoiceno,createdByUserName, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(deliverychallans);
        }

        [HttpGet]
        public JsonResult GetChallanDetail(long challanId)
        {
            DeliveryChallanBL deliverychallanBL = new DeliveryChallanBL();
            DeliveryChallanViewModel deliverychallan = new DeliveryChallanViewModel();
            try
            {
                deliverychallan = deliverychallanBL.GetChallanDetail(challanId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(deliverychallan, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult GetChallanProductList(List<ChallanProductViewModel> challanProducts, long challanId)
        {
            DeliveryChallanBL deliverychallanBL = new DeliveryChallanBL();
            try
            {
                if (challanProducts == null)
                {
                    challanProducts = deliverychallanBL.GetChallanProductList(challanId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(challanProducts);
        }
        [HttpPost]
        public PartialViewResult GetChallanTaxList(List<ChallanTaxViewModel> challanTaxes, long challanId)
        {
            DeliveryChallanBL deliverychallanBL = new DeliveryChallanBL();
            try
            {
                if (challanTaxes == null)
                {
                    challanTaxes = deliverychallanBL.GetChallanTaxList(challanId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(challanTaxes);
        }
        [HttpPost]
        public PartialViewResult GetChallanTermList(List<ChallanTermsViewModel> challanTerms, long challanId)
        {
            DeliveryChallanBL deliverychallanBL = new DeliveryChallanBL();
            try
            {
                if (challanTerms == null)
                {
                    challanTerms = deliverychallanBL.GetChallanTermList(challanId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(challanTerms);
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
        public PartialViewResult GetSaleInvoiceList(string saleinvoiceNo = "", string customerName = "", string refNo = "", string fromDate = "", string toDate = "",string invoiceType="",string displayType="", string approvalStatus = "", int companyBranchId = 0, string saleType = "0")
        {
            List<SaleInvoiceViewModel> invoices = new List<SaleInvoiceViewModel>();
            SaleInvoiceBL saleinvoiceBL = new SaleInvoiceBL();
            try
            {
                invoices = saleinvoiceBL.GetSaleInvoiceList(saleinvoiceNo, customerName, refNo, fromDate, toDate, ContextUser.CompanyId,invoiceType, displayType,approvalStatus,"",companyBranchId,saleType,string.Empty, Convert.ToInt32(Session[SessionKey.CustomerId]));
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(invoices);
        }

        [HttpGet]
        public PartialViewResult GetSaleDispatchList(string saledispatchNo = "", string customerName = "", string fromDate = "", string toDate = "",int companyBranchId = 0)
        {
            List<SaleDispatchViewModel> invoices = new List<SaleDispatchViewModel>();
            SaleInvoiceBL saleinvoiceBL = new SaleInvoiceBL();
            try
            {
                invoices = saleinvoiceBL.GetSaleDispatchList(saledispatchNo, customerName,fromDate, toDate, companyBranchId, Convert.ToInt32(Session[SessionKey.CustomerId]));
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(invoices);
        }


        [HttpGet]
        public JsonResult GetCustomerBranchList(int customerId)
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
        public JsonResult GetCompanyBranchList()
        {
            CompanyBL companyBL = new CompanyBL();
            List<CompanyBranchViewModel> companyBranchList = new List<CompanyBranchViewModel>();
            try
            {
                companyBranchList = companyBL.GetCompanyBranchList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(companyBranchList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCompanyBranchType(int companyBranchId=0)
        {
            CompanyBL companyBL = new CompanyBL();
            List<CompanyBranchViewModel> companyBranchTypeList = new List<CompanyBranchViewModel>();
            try
            {
                companyBranchTypeList = companyBL.GetCompanyBranchTypeList(ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(companyBranchTypeList, JsonRequestBehavior.AllowGet);
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

        public ActionResult Report(long challanId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            DeliveryChallanBL challanBL = new DeliveryChallanBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "ChallanPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable dt = new DataTable();
            dt = challanBL.GetChallanDetailTable(challanId);

            decimal totalInvoiceAmount = 0;
            totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
            string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdProduct = new ReportDataSource("DataSetProduct", challanBL.GetChallanProductListDataTable(challanId));
            ReportDataSource rdTerms = new ReportDataSource("DataSetTerms", challanBL.GetChallanTermListDataTable(challanId));
            ReportDataSource rdProductSerial = new ReportDataSource("DataSetProductSerial", challanBL.GetChallanProductSerialList(challanId));

            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            lr.DataSources.Add(rdTerms);
            lr.DataSources.Add(rdProductSerial);
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

        public ActionResult ChallanMail(long challanId, string reportType = "PDF")
        {
            ResponseOut responseOut = new ResponseOut();
            LocalReport lr = new LocalReport();
            DeliveryChallanBL challanBL = new DeliveryChallanBL();
            EmailTemplateBL emailTemplateBL = new EmailTemplateBL();
            try
            {
                string path = Path.Combine(Server.MapPath("~/RDLC"), "ChallanPrint.rdlc");
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
                emaildt = emailTemplateBL.GetEmailTemplateDetailByEmailType((int)MailSendingMode.Challan);

                DataTable dt = new DataTable();
                dt = challanBL.GetChallanDetailTable(challanId);

                if (dt.Rows.Count > 0)
                {
                    StringBuilder mailBody = new StringBuilder(" ");
                    SendMail sendMail = new SendMail();
                    mailBody.Append("<html><head></head><body>");
                    //mailBody.Append("<img src='" + Convert.ToString(ConfigurationManager.AppSettings["Logo_Path"]) + "' alt='ICS Logo' />");
                    //mailBody.Append("<hr/><br/>");
                    // mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Dear " + dt.Rows[0]["ContactPerson"].ToString() + " </p><br/>");
                    // mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Please find attached delivery challan for your reference</p><br/>");
                    // mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Regards,</p><br/>");
                    // mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Sale Team</p><br/>");
                    mailBody.Append(emaildt.Rows[0][4].ToString());
                    mailBody.Append("</body></html>");

                    decimal totalInvoiceAmount = 0;
                    totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
                    string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));

                    ReportDataSource rd = new ReportDataSource("DataSet1", dt);
                    ReportDataSource rdProduct = new ReportDataSource("DataSetProduct", challanBL.GetChallanProductListDataTable(challanId));
                    //ReportDataSource rdTax = new ReportDataSource("DataSetTax", soBL.GetSOTaxList(soId));
                    ReportDataSource rdTerms = new ReportDataSource("DataSetTerms", challanBL.GetChallanTermListDataTable(challanId));


                    lr.DataSources.Add(rd);
                    lr.DataSources.Add(rdProduct);
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
                    //bool sendMailStatus = sendMail.SendEmail("", dt.Rows[0]["ShippingEmail"].ToString(), "Delivery Challan", mailBody.ToString(), renderedBytes, "Challan.pdf");
                    if (userEmailSetting.Rows.Count > 0)
                    {
                        sendMailStatus = sendMail.SendEmail(userEmailSetting.Rows[0]["SmtpUser"].ToString(), dt.Rows[0]["ShippingEmail"].ToString(), emaildt.Rows[0]["EmailTemplateSubject"].ToString(), mailBody.ToString(), renderedBytes, "DeliveryChallan.pdf", userEmailSetting.Rows[0]["SmtpPass"].ToString(), userEmailSetting.Rows[0]["SmtpDisplayName"].ToString(), userEmailSetting.Rows[0]["SmtpServer"].ToString(), Convert.ToInt32(userEmailSetting.Rows[0]["SmtpPort"]), Convert.ToBoolean(userEmailSetting.Rows[0]["EnableSsl"]));
                    }
                    else
                    {
                        sendMailStatus = sendMail.SendEmail("", dt.Rows[0]["ShippingEmail"].ToString(), emaildt.Rows[0]["EmailTemplateSubject"].ToString(), mailBody.ToString(), renderedBytes, "DeliveryChallan.pdf");
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
                    responseOut.message = "Challan Detail not found!!!";
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
                        var path = Path.Combine(Server.MapPath("~/Images/DCDocument"), newFileName);
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
            var path = Path.Combine(Server.MapPath("~/Images/DCDocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public PartialViewResult GetDeliveryChallanSupportingDocumentList(List<DeliveryChallanSupportingDocumentViewModel> deliveryChallanDocuments, long challanId)
        { 
            DeliveryChallanBL deliveryChallanBL = new DeliveryChallanBL();
            try
            {
                if (deliveryChallanDocuments == null)
                {
                    deliveryChallanDocuments = deliveryChallanBL.GetDeliveryChallanSupportingDocumentList(challanId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(deliveryChallanDocuments);
        }
       
        [HttpGet]
        public JsonResult GetCompanyBranchByStateIdList(int companyBranchID)
        {
            CompanyBL companyBL = new CompanyBL();
            CompanyBranchViewModel companyBranchList = new CompanyBranchViewModel();
            try
            {
                companyBranchList = companyBL.GetCompanyBranchByStateIdList(companyBranchID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(companyBranchList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult GetChallanSaleInvoiceProductList(List<SaleInvoiceProductViewModel> saleinvoiceProducts, long saleinvoiceId)
        {
            DeliveryChallanBL deliveryChallanBL = new DeliveryChallanBL();
            try
            {
                if (saleinvoiceProducts == null)
                {

                    saleinvoiceProducts = deliveryChallanBL.GetChallanSaleInvoiceProductList(saleinvoiceId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(saleinvoiceProducts);
        }


        #endregion
    }
}
