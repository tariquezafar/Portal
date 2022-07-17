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
using System.IO;
using System.Text;
using System.Data;
using ClosedXML.Excel;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class POController : BaseController
    {
        #region PO
        
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PO, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPO(int poId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;


                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["RoleId"] = ContextUser.RoleId;

                if (poId != 0)
                {
                    ViewData["poId"] = poId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["poId"] = 0;
                    ViewData["accessMode"] =0 ;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PO, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPO(POViewModel poViewModel, List<POProductViewModel> poProducts, List<POTaxViewModel> poTaxes, List<POTermViewModel> poTerms, List<POSupportingDocumentViewModel> poDocuments, List<POScheduleViewModel> poSchedules)
        {
            ResponseOut responseOut = new ResponseOut();
            POBL poBL = new POBL();
            try
            {
                if (poViewModel != null)
                {
                    poViewModel.CreatedBy = ContextUser.UserId;
                    poViewModel.CompanyId = ContextUser.CompanyId;
                    poViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = poBL.AddEditPO(poViewModel, poProducts, poTaxes, poTerms,poDocuments, poSchedules);
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
                        newFileName = Convert.ToString(rnd.Next(10000,99999)) + "_" + fileName;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PO, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPO(string listStatus="false")
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
        public PartialViewResult GetPOList(string poNo = "", string vendorName = "", string refNo = "", string fromDate = "", string toDate = "",string approvalStatus="", string displayType = "",string CreatedByUserName = "",int companyBranch=0,string poType="")
        {
            List<POViewModel> pos = new List<POViewModel>();
            long companyBranchId = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
            POBL poBL = new POBL();
            try
            {
                pos = poBL.GetPOList(poNo, vendorName, refNo, fromDate, toDate,approvalStatus, ContextUser.CompanyId, displayType, CreatedByUserName, Convert.ToInt32(companyBranchId), poType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pos);
        }

        [HttpPost]
        public PartialViewResult GetPOProductList(List<POProductViewModel> poProducts, long poId)
        {
            POBL poBL = new POBL();
            try
            {
                if (poProducts == null)
                {
                    poProducts = poBL.GetPOProductList(poId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(poProducts);
        }

        [HttpPost]
        public PartialViewResult GetPOScheduleList(List<POScheduleViewModel> poSchedules, long poId)
        {
            POBL poBL = new POBL();
            try
            {
                if (poSchedules == null)
                {
                    poSchedules = poBL.GetPOScheduleList(poId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(poSchedules);
        }

        public void POScheduleExport(long poId, string reportType = "PDF")
        {
            POBL poBL = new POBL();
            DataTable dt = poBL.POScheduleExport(poId);
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "POSchedule");
                ////Response.Clear();
                ////Response.Buffer = true;
                ////Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=POSchedule.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        [HttpPost]
        public PartialViewResult GetPOTaxList(List<POTaxViewModel> poTaxes, long poId)
        {

            POBL poBL = new POBL();
            try
            {
                if (poTaxes == null)
                {
                    poTaxes = poBL.GetPOTaxList(poId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(poTaxes);
        }

        [HttpPost]
        public PartialViewResult GetPOSupportingDocumentList(List<POSupportingDocumentViewModel> poDocuments, long poId)
        {

            POBL poBL = new POBL();
            try
            {
                if (poDocuments == null)
                {
                    poDocuments = poBL.GetPOSupportingDocumentList(poId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(poDocuments);
        }
        [HttpPost]
        public PartialViewResult GetPOTermList(List<POTermViewModel> poTerms, long poId)
        {
            POBL poBL = new POBL();
            try
            {
                if (poTerms == null)
                {
                    poTerms = poBL.GetPoTermList(poId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(poTerms);
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
        public JsonResult GetDocumentTypeList()
        {
            DocumentTypeBL docBL = new DocumentTypeBL();
            List<DocumentTypeViewModel> documentList = new List<DocumentTypeViewModel>();
            try
            {
                documentList = docBL.GetDocumentTypeList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(documentList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModuleDocumentTypeList(string employeeDoc)
        {
            DocumentTypeBL docBL = new DocumentTypeBL();
            List<DocumentTypeViewModel> documentList = new List<DocumentTypeViewModel>();
            try
            {
                documentList = docBL.GetDocumentTypeListModuleType(ContextUser.CompanyId, employeeDoc);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(documentList, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetPODetail(long poId)
        {

            POBL poBL = new POBL();
            POViewModel po = new POViewModel();
            try
            {
                po = poBL.GetPODetail(poId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(po, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Report(long poId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            POBL poBL = new POBL();
            string path = "";
            DataTable dt = new DataTable();
            dt = poBL.GetPODetailDataTable(poId);

            if (dt.Rows[0]["POType"].ToString().Trim() == "Import")
            {
                path = Path.Combine(Server.MapPath("~/RDLC"), "POExportPrint.rdlc");
            }
            else
            {
                path = Path.Combine(Server.MapPath("~/RDLC"), "POPrint.rdlc");
            }

            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
          
            decimal totalInvoiceAmount = 0;
            string approvalstatus = "";
            totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
            approvalstatus = dt.Rows[0]["POStatus"].ToString();
            string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));

            ReportDataSource rd = new ReportDataSource("DataSet2",dt );
            ReportDataSource rdProduct = new ReportDataSource("DataSetPOProduct", poBL.GetPOProductListDataTable(poId));
            ReportDataSource rdTax = new ReportDataSource("DataSetPOTax", poBL.GetPOTaxList(poId));
            ReportDataSource rdTerms = new ReportDataSource("DataSetPOTerms", poBL.GetPOTermListDataTable(poId));

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



            string deviceInfo = "<DeviceInfo>" +
                        "  <OutputFormat>" + reportType + "</OutputFormat>" +
                        "  <PageWidth>8.5in</PageWidth>" +
                        "  <PageHeight>11in</PageHeight>" +
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
        public ActionResult POMail(long poId, string reportType = "PDF")
        {
            ResponseOut responseOut = new ResponseOut();
            LocalReport lr = new LocalReport();
            POBL poBL = new POBL();
            EmailTemplateBL emailTemplateBL = new EmailTemplateBL();
            try
            {  
                string path = Path.Combine(Server.MapPath("~/RDLC"), "POPrint.rdlc");
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
                emaildt = emailTemplateBL.GetEmailTemplateDetailByEmailType((int)MailSendingMode.PO);


                DataTable dt = new DataTable();
                dt = poBL.GetPODetailDataTable(poId);

                if (dt.Rows.Count > 0)
                {
                    StringBuilder mailBody = new StringBuilder(" ");
                    SendMail sendMail = new SendMail();
                    mailBody.Append("<html><head></head><body>");
                    //mailBody.Append("<img src='" + Convert.ToString(ConfigurationManager.AppSettings["Logo_Path"]) + "' alt='ICS Logo' />");
                    //mailBody.Append("<hr/><br/>");
                        mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Dear " + dt.Rows[0]["ContactPersonName"].ToString() + " </p><br/>");
                    //  mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Please find attached Purchase Order for your reference</p><br/>");
                    //  mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Regards,</p><br/>");
                    //  mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Sale Team</p><br/>");
                    mailBody.Append(emaildt.Rows[0][4].ToString());
                    mailBody.Append("</body></html>");

                    decimal totalInvoiceAmount = 0;
                    totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
                    string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));

                    ReportDataSource rd = new ReportDataSource("DataSet2", dt);
                    ReportDataSource rdProduct = new ReportDataSource("DataSetPOProduct", poBL.GetPOProductListDataTable(poId));
                    ReportDataSource rdTax = new ReportDataSource("DataSetPOTax", poBL.GetPOTaxList(poId));
                    ReportDataSource rdTerms = new ReportDataSource("DataSetPOTerms", poBL.GetPOTermListDataTable(poId));
                    lr.DataSources.Add(rd);
                    lr.DataSources.Add(rdProduct);
                    lr.DataSources.Add(rdTax);
                    lr.DataSources.Add(rdTerms);

                    ReportParameter rp1 = new ReportParameter("AmountInWords", totalWords);
                    lr.SetParameters(rp1);
                    string mimeType;
                    string encoding;
                    string fileNameExtension;



                    string deviceInfo =  "<DeviceInfo>" +
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
                    if (userEmailSetting.Rows.Count>0)
                    {
                        sendMailStatus = sendMail.SendEmail(userEmailSetting.Rows[0]["SmtpUser"].ToString(), dt.Rows[0]["Email"].ToString(), "Purchase Order", mailBody.ToString(), renderedBytes, "PO.pdf", userEmailSetting.Rows[0]["SmtpPass"].ToString(), userEmailSetting.Rows[0]["SmtpDisplayName"].ToString(), userEmailSetting.Rows[0]["SmtpServer"].ToString(),Convert.ToInt32(userEmailSetting.Rows[0]["SmtpPort"]),Convert.ToBoolean(userEmailSetting.Rows[0]["EnableSsl"]));
                    }
                    else
                    {
                        sendMailStatus = sendMail.SendEmail("", dt.Rows[0]["Email"].ToString(), "Purchase Order", mailBody.ToString(), renderedBytes, "PO.pdf");
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
                    responseOut.message = "Purchase Order Detail not found!!!";
                    responseOut.status = ActionStatus.Fail;

                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetPurchaseQuotationList(string quotationNo = "", string vendorName = "", string fromDate = "", string toDate = "", string displayType = "", string approvalStatus = "",string companyBranch="")
        {
            List<PurchaseQuotationViewModel> quotations = new List<PurchaseQuotationViewModel>();
            POBL pOBL = new POBL();
            try
            {
                quotations = pOBL.GetPurchaseQuotationList(quotationNo, vendorName, fromDate, toDate, ContextUser.CompanyId, displayType, approvalStatus, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(quotations);
        }

        [HttpPost]
        public PartialViewResult GetPurchaseQuotationProductList(List<PurchaseQuotationProductViewModel> quotationProducts, long quotationId)
        {
            
            POBL pOBL = new POBL();
            try
            {
                if (quotationProducts == null)
                {
                    quotationProducts = pOBL.GetPurchaseQuotationProductList(quotationId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(quotationProducts);
        }


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PO, (int)AccessMode.CancelAccess, (int)RequestMode.GetPost)]
        public ActionResult CancelPO(int poId = 0, int accessMode = 4)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (poId != 0)
                {
                    ViewData["poId"] = poId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["poId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PO, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult CancelPO(long poId, string cancelReason)
        {
            ResponseOut responseOut = new ResponseOut();
            POBL pOBL = new POBL();
            POViewModel pOViewModel = new POViewModel();          
            try
            {
                if (pOViewModel != null)
                {
                    pOViewModel.POId = poId;
                    pOViewModel.CancelReason = cancelReason;
                    pOViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = pOBL.CancelPO(pOViewModel);
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
            List<POViewModel> pos = new List<POViewModel>();

            POBL poBL = new POBL();
            try
            {
                pos = poBL.GetPrpductPurchaseList(productId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pos);
        }
        #endregion

        #region Revised PO
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PO, (int)AccessMode.ReviseAccess, (int)RequestMode.GetPost)]
        public ActionResult AddRevisedPO(int poId = 0, int AccessMode = 5)
        {
            try
            {
                if (poId != 0)
                {
                    ViewData["poId"] = poId;
                    ViewData["accessMode"] = AccessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["poId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PO, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddRevisedPO(POViewModel poViewModel, List<POProductViewModel> poProducts, List<POTaxViewModel> poTaxes, List<POTermViewModel> poTerms,List<POSupportingDocumentViewModel> revisedPODocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            POBL poBL = new POBL();
            try
            {
                if (poViewModel != null)
                {
                    poViewModel.CreatedBy = ContextUser.UserId;
                    poViewModel.CompanyId = ContextUser.CompanyId;
                    poViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = poBL.AddRevisedPO(poViewModel, poProducts, poTaxes, poTerms, revisedPODocuments);
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
                        var path = Path.Combine(Server.MapPath("~/Images/REVISEDPODocument"), newFileName);
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
            var path = Path.Combine(Server.MapPath("~/Images/REVISEDPODocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        #endregion

        #region Approval PO
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PO, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ApprovalPO(int poId = 0, int accessMode = 3)
        {

            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                if (poId != 0)
                {

                    ViewData["poId"] = poId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["poId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PO, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ApprovalPO(POViewModel pOViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            StoreRequisitionBL storeRequisitionBL = new StoreRequisitionBL();

            try
            {
                if (pOViewModel != null)
                {
                    pOViewModel.CompanyId = ContextUser.CompanyId;
                    pOViewModel.ApprovedBy = ContextUser.UserId;
                    pOViewModel.RejectedBy = ContextUser.UserId;
                    POBL poBL = new POBL();
                    responseOut = poBL.ApproveRejectPO(pOViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ApprovePo, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult ListApprovedPO(string listStatus = "false")
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
        public PartialViewResult GetApprovedPOList(string poNo = "", string vendorName = "", string refNo = "", string fromDate = "", string toDate = "", string approvalStatus = "", string displayType = "", string companyBranch ="")
        {
            List<POViewModel> pos = new List<POViewModel>();
            companyBranch = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId.ToString() : "0";
            POBL poBL = new POBL();
            try
            {
                pos = poBL.GetApprovePOList(poNo, vendorName, refNo, fromDate, toDate, approvalStatus, ContextUser.CompanyId, displayType, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pos);
        }


        [HttpPost]
        public PartialViewResult GetPOProductApprovalList(List<POProductViewModel> poProducts, long poId)
        {
            POBL poBL = new POBL();
            try
            {
                if (poProducts == null)
                {
                    poProducts = poBL.GetPOProductList(poId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(poProducts);
        }

        [HttpGet]
        public JsonResult GetPOApprovalDetail(long poId)
        {

            POBL poBL = new POBL();
            POViewModel po = new POViewModel();
            try
            {
                po = poBL.GetPOApprovalDetail(poId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(po, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PurchaseIndent
        [HttpGet]
        public PartialViewResult GetPurchaseIndentList(string indentNo = "", string indentType = "", string customerName = "", int companyBranchId = 0, string fromDate = "", string toDate = "", string displayType = "", string approvalStatus = "0")
        {
            List<PurchaseIndentViewModel> indents = new List<PurchaseIndentViewModel>();
            POBL purchaseOrderBL = new POBL();

            try
            {
                indents = purchaseOrderBL.GetPurchaseOrderIndentList(indentNo, indentType, customerName, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), ContextUser.CompanyId, displayType, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(indents);
        }

        [HttpPost]
        public PartialViewResult GetPurchaseIndentProductList(List<PurchaseIndentProductDetailViewModel> purchaseIndentProducts, long indentId)
        {
            POBL purchaseOrderBL = new POBL();
            try
            {
                if (purchaseIndentProducts == null)
                {
                    purchaseIndentProducts = purchaseOrderBL.GetPurchaseOrderIndentProductList(indentId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(purchaseIndentProducts);
        }

        #endregion
    }
}
