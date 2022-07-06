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
using System.Web.Script.Serialization;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class SaleInvoiceController : BaseController
    {
        #region SaleInvoice

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SaleInvoice, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSaleInvoice(int siId = 0, int accessMode = 0, int customerId = 0, string customerCode = "", string customerName = "")
        {
            try
            {
                
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["RoleId"] = ContextUser.RoleId;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (siId != 0)
                {
                    ViewData["siId"] = siId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["siId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SaleInvoice, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditSaleInvoice(SaleInvoiceViewModel saleinvoiceViewModel, List<SaleInvoiceProductViewModel> siProducts, List<SaleInvoiceTaxViewModel> saleinvoiceTaxes, List<SaleInvoiceTermViewModel> saleinvoiceTerms, List<SaleInvoiceProductSerialDetailViewModel> saleInvoiceProductSerialDetail, List<SISupportingDocumentViewModel> siDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            SaleInvoiceBL saleinvoiceBL = new SaleInvoiceBL();
            try
            {
                if (saleinvoiceViewModel != null)
                {
                    saleinvoiceViewModel.CreatedBy = ContextUser.UserId;
                    saleinvoiceViewModel.CompanyId = ContextUser.CompanyId;
                    saleinvoiceViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut= saleinvoiceBL.AddEditSaleInvoice(saleinvoiceViewModel, siProducts, saleinvoiceTaxes, saleinvoiceTerms, saleInvoiceProductSerialDetail, siDocuments);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SaleInvoice, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSaleInvoice(string todayList="false")
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                ViewData["todayList"] = todayList;
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpGet]
        public PartialViewResult GetSaleInvoiceList(string saleinvoiceNo = "", string customerName = "", string refNo = "", string fromDate = "", string toDate = "",string invoiceType="",string approvalStatus="", int companyBranchId=0, string saleType="",string CreatedByUserName= "",int LocationId=0)
        {
            List<SaleInvoiceViewModel> saleinvoices = new List<SaleInvoiceViewModel>();
            SaleInvoiceBL saleinvoiceBL = new SaleInvoiceBL();
            try
            {
                saleinvoices = saleinvoiceBL.GetSaleInvoiceList(saleinvoiceNo, customerName, refNo, fromDate, toDate, ContextUser.CompanyId, invoiceType,"", approvalStatus,"", companyBranchId, saleType, CreatedByUserName, Convert.ToInt32(Session[SessionKey.CustomerId]),"Sale", LocationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(saleinvoices);
        }

        [HttpGet]
        public JsonResult GetSaleInvoiceDetail(long saleinvoiceId)
        {
            SaleInvoiceBL saleinvoiceBL = new SaleInvoiceBL();
            SaleInvoiceViewModel saleinvoice = new SaleInvoiceViewModel();
            try
            {
                saleinvoice = saleinvoiceBL.GetSaleInvoiceDetail(saleinvoiceId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(saleinvoice, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public PartialViewResult GetSaleInvoiceTaxList(List<SaleInvoiceTaxViewModel> saleinvoiceTaxes, long saleinvoiceId)
        {
            SaleInvoiceBL saleinvoiceBL = new SaleInvoiceBL();
            try
            {
                if (saleinvoiceTaxes == null)
                {
                    saleinvoiceTaxes = saleinvoiceBL.GetSaleInvoiceTaxList(saleinvoiceId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(saleinvoiceTaxes);
        }
        [HttpPost]
        public PartialViewResult GetSaleInvoiceTermList(List<SaleInvoiceTermViewModel> saleinvoiceTerms, long saleinvoiceId)
        {
            SaleInvoiceBL saleinvoiceBL = new SaleInvoiceBL();
            try
            {
                if (saleinvoiceTerms == null)
                {
                    saleinvoiceTerms = saleinvoiceBL.GetSaleInvoiceTermList(saleinvoiceId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(saleinvoiceTerms);
        }
    
        [HttpPost]
        public PartialViewResult GetSaleInvoiceProductSerialDetailList(List<SaleInvoiceProductSerialDetailViewModel> saleSerials, int saleinvoiceId = 0)
        {
            SaleInvoiceBL saleinvoiceBL = new SaleInvoiceBL();
            try
            {
                if (saleSerials == null)
                {
                    saleSerials = saleinvoiceBL.GetSaleInvoiceProductSerialDetailList(saleinvoiceId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(saleSerials);
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
        public JsonResult GetEmployeeDepartmentWiseAutoCompleteList(string term,int companybrachId)
        {
            EmployeeBL customerBL = new EmployeeBL();

            List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();
            try
            {
                employeeList = customerBL.GetEmployeeDepartmentWiseAutoCompleteList(term, ContextUser.CompanyId, companybrachId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employeeList, JsonRequestBehavior.AllowGet);
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
        public PartialViewResult GetSaleOrderList(string soNo = "", string customerName = "", string refNo = "", string fromDate = "", string toDate = "",string approvalStatus="", string displayType = "",int companyBranchId=0)
        {
            List<SOViewModel> soList = new List<SOViewModel>();
            SOBL soBL = new SOBL();
            try
            {
                soList = soBL.GetSOList(soNo, customerName, refNo, fromDate, toDate, ContextUser.CompanyId, approvalStatus, displayType,"", companyBranchId,"", Convert.ToInt32(Session[SessionKey.CustomerId]));
            
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(soList);
        }

        public ActionResult Report(long siId, string reportType = "PDF",string reportOption="Original")
        {
            LocalReport lr = new LocalReport();
            SaleInvoiceBL siBL = new SaleInvoiceBL();
            

            DataTable dt = new DataTable();
            dt = siBL.GetSaleInvoiceDetailDataTable(siId);

            decimal totalInvoiceAmount = 0;
            string approvalstatus = "";
            totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
            approvalstatus = dt.Rows[0]["ApprovalStatus"].ToString();
            string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));

            string BranchType= dt.Rows[0]["BranchType"].ToString();
            string path = "";

            DataTable dtProducts = siBL.GetSaleInvoiceProductListDataTable(siId);


            int CGSTCount = 0;
            int IGSTCount = 0;
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


            //if (BranchType== "Showroom")
            //{
            //    if (CGSTCount > 0)
            //    {
            //        path = Path.Combine(Server.MapPath("~/RDLC"), "SaleInvoicePrintNew.rdlc");
            //    }
            //    else
            //    {
            //        path = Path.Combine(Server.MapPath("~/RDLC"), "SaleInvoicePrintNew.rdlc");
            //    }
            //}
            //else
            //{
            //    if (CGSTCount > 0)
            //    {
            //        path = Path.Combine(Server.MapPath("~/RDLC"), "SaleInvoicePrintNew.rdlc");
                    
            //    }
            //    else
            //    {
            //        path = Path.Combine(Server.MapPath("~/RDLC"), "SaleInvoicePrintNew.rdlc");

            //    }

            //}
            path = Path.Combine(Server.MapPath("~/RDLC"), "SaleInvoicePrint.rdlc");

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
            ReportDataSource rdTax = new ReportDataSource("DataSetTax", siBL.GetSaleInvoiceTaxListDataTable(siId));
            ReportDataSource rdTerms = new ReportDataSource("DataSetTerms", siBL.GetSaleInvoiceTermListDataTable(siId));
            ReportDataSource rdSaleInvoiceProductSerialList = new ReportDataSource("DataSetSaleInvoiceProductSerialList", siBL.GetSaleInvoiceProductSerialDetailDataTable(siId));
            ReportDataSource rddocumentList = new ReportDataSource("SIDocumentDataSet", siBL.GetSaleInvoiceDocumentList(siId));

            ReportDataSource rdService = new ReportDataSource("ServiceItemDataSet", siBL.GetSIServiceItmeList(siId));


            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            lr.DataSources.Add(rdTax);
            lr.DataSources.Add(rdTerms);
            lr.DataSources.Add(rdSaleInvoiceProductSerialList);
            lr.DataSources.Add(rddocumentList);
            lr.DataSources.Add(rdService);
            ReportParameter rp1 = new ReportParameter("AmountInWords", totalWords);
            ReportParameter rp2 = new ReportParameter("ReportOption", reportOption);
            ReportParameter rp3 = new ReportParameter("approvalstatus", approvalstatus);

            lr.SetParameters(rp1);
            lr.SetParameters(rp2);
            lr.SetParameters(rp3);


            string mimeType;
            string encoding;
            string fileNameExtension;



            //string deviceInfo =

            //"<DeviceInfo>" +
            //"  <OutputFormat>" + reportType + "</OutputFormat>" +
            //"  <PageWidth>8.5in</PageWidth>" +
            //"  <PageHeight>11in</PageHeight>" +
            //"  <MarginTop>0.50in</MarginTop>" +
            //"  <MarginLeft>.1in</MarginLeft>" +
            //"  <MarginRight>.1in</MarginRight>" +
            //"  <MarginBottom>0.5in</MarginBottom>" +
            //"</DeviceInfo>";

            string deviceInfo = "<DeviceInfo>" +
                      "  <OutputFormat>" + reportType + "</OutputFormat>" +
                      "  <PageWidth>8.5in</PageWidth>" +
                      "  <PageHeight>11in</PageHeight>" +
                      "  <MarginTop>0.10in</MarginTop>" +
                      "  <MarginLeft>.15in</MarginLeft>" +
                      "  <MarginRight>.05in</MarginRight>" +
                      "  <MarginBottom>0.02in</MarginBottom>" +
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

        public ActionResult ReportForm(long siId, string reportType = "PDF", string reportOption = "Original")
        {
            LocalReport lr = new LocalReport();
            SaleInvoiceBL siBL = new SaleInvoiceBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "SaleInvoicePrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
              ReportDataSource rdProduct = new ReportDataSource("SaleInvoiceFormDataSet", siBL.GetSaleInvoiceProductSerialFormList(siId));
            
            lr.DataSources.Add(rdProduct);
            
            string mimeType;
            string encoding;
            string fileNameExtension;
            
            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>0.10in</MarginLeft>" +
            "  <MarginRight>0.10in</MarginRight>" +
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

        public ActionResult ReportTxt(long siId,string fileName,string reportType = "TXT")
        {
            LocalReport lr = new LocalReport();
            SaleInvoiceBL siBL = new SaleInvoiceBL();
            string path = Path.Combine(Server.MapPath("~/Content/VahaanLogFiles/"), fileName);
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rdProduct = new ReportDataSource("SaleInvoiceFormDataSet", siBL.GetSaleInvoiceProductSerialFormList(siId));

            lr.DataSources.Add(rdProduct);

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>0.10in</MarginLeft>" +
            "  <MarginRight>0.10in</MarginRight>" +
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
        public ActionResult SaleInvoiceMail(long siId, string reportType = "PDF", string reportOption = "Original")
        {
            ResponseOut responseOut = new ResponseOut();
            LocalReport lr = new LocalReport();
            SaleInvoiceBL siBL = new SaleInvoiceBL();
            EmailTemplateBL emailTemplateBL = new EmailTemplateBL();
            try
            { 


                string path = Path.Combine(Server.MapPath("~/RDLC"), "SaleInvoicePrint.rdlc");
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
                emaildt = emailTemplateBL.GetEmailTemplateDetailByEmailType((int)MailSendingMode.SaleInvoice);

                DataTable dt = new DataTable();
                dt = siBL.GetSaleInvoiceDetailDataTable(siId);

                if (dt.Rows.Count > 0)
                {
                    StringBuilder mailBody = new StringBuilder(" ");
                    SendMail sendMail = new SendMail();
                    mailBody.Append("<html><head></head><body>");
                    //mailBody.Append("<img src='" + Convert.ToString(ConfigurationManager.AppSettings["Logo_Path"]) + "' alt='ICS Logo' />");
                    //mailBody.Append("<hr/><br/>");
                    mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Dear " + dt.Rows[0]["ContactPerson"].ToString() + " </p><br/>");
                    // mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Please find attached Invoice for your reference</p><br/>");
                    //  mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Regards,</p><br/>");
                    // mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Sale Team</p><br/>");
                   mailBody.Append(emaildt.Rows[0][4].ToString());
                    mailBody.Append("</body></html>");

                    decimal totalInvoiceAmount = 0;
                    totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
                    string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));



                    ReportDataSource rd = new ReportDataSource("DataSet1", dt);
                    ReportDataSource rdProduct = new ReportDataSource("DataSetProduct", siBL.GetSaleInvoiceProductListDataTable(siId));
                    ReportDataSource rdTax = new ReportDataSource("DataSetTax", siBL.GetSaleInvoiceTaxListDataTable(siId));
                    ReportDataSource rdTerms = new ReportDataSource("DataSetTerms", siBL.GetSaleInvoiceTermListDataTable(siId));
                    ReportDataSource rdSaleInvoiceProductSerialList = new ReportDataSource("DataSetSaleInvoiceProductSerialList", siBL.GetSaleInvoiceProductSerialDetailDataTable(siId));

                    lr.DataSources.Add(rd);
                    lr.DataSources.Add(rdProduct);
                    lr.DataSources.Add(rdTax);
                    lr.DataSources.Add(rdTerms);
                    lr.DataSources.Add(rdSaleInvoiceProductSerialList);

                    ReportParameter rp1 = new ReportParameter("AmountInWords", totalWords);
                    ReportParameter rp2 = new ReportParameter("ReportOption", reportOption);
                    lr.SetParameters(rp1);
                    lr.SetParameters(rp2);
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
                    if (userEmailSetting.Rows.Count>0)
                    {
                        sendMailStatus = sendMail.SendEmail(userEmailSetting.Rows[0]["SmtpUser"].ToString(), dt.Rows[0]["Email"].ToString(), "Sale Invoice", mailBody.ToString(), renderedBytes, "SaleInvoice.pdf", userEmailSetting.Rows[0]["SmtpPass"].ToString(), userEmailSetting.Rows[0]["SmtpDisplayName"].ToString(), userEmailSetting.Rows[0]["SmtpServer"].ToString(),Convert.ToInt32(userEmailSetting.Rows[0]["SmtpPort"]),Convert.ToBoolean(userEmailSetting.Rows[0]["EnableSsl"]));
                    }
                    else
                    {
                        sendMailStatus = sendMail.SendEmail("", dt.Rows[0]["Email"].ToString(), "Sale Invoice", mailBody.ToString(), renderedBytes, "SaleInvoice.pdf");
                    }
                    //bool sendMailStatus = sendMail.SendEmail("", dt.Rows[0]["Email"].ToString(), "Invoice", mailBody.ToString(), renderedBytes, "Invoice.pdf");
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
                    responseOut.message = "Invoice Detail not found!!!";
                    responseOut.status = ActionStatus.Fail;

                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CancelSaleInvoice, (int)AccessMode.CancelAccess, (int)RequestMode.GetPost)]
        public ActionResult CancelSaleInvoice(int siId = 0, int accessMode =4)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (siId != 0)
                {
                    ViewData["siId"] = siId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["siId"] = 0;
                    ViewData["accessMode"] = 4;

                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }


        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CancelSaleInvoice, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult CancelSaleInvoice(long invoiceId, string sINo, string cancelReason)
        {
            ResponseOut responseOut = new ResponseOut();
            SaleInvoiceBL saleinvoiceBL = new SaleInvoiceBL();
            SaleInvoiceViewModel saleinvoiceViewModel = new SaleInvoiceViewModel();
            try
            {
                if (saleinvoiceViewModel != null)
                {
                    saleinvoiceViewModel.InvoiceId = invoiceId;
                    saleinvoiceViewModel.InvoiceNo = sINo;
                    saleinvoiceViewModel.CancelReason = cancelReason;
                    saleinvoiceViewModel.CreatedBy = ContextUser.UserId;
                    saleinvoiceViewModel.CompanyId = ContextUser.CompanyId;
                    saleinvoiceViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = saleinvoiceBL.CancelSaleInvoice(saleinvoiceViewModel);
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
                        newFileName = Convert.ToString(rnd.Next(10000, 99999)) + "_" + fileName;
                        var path = Path.Combine(Server.MapPath("~/Images/SIDocument"), newFileName);
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


        

        [HttpPost]
        public PartialViewResult SaveProductSerialDetail()
        {
            UploadUtilityBL utilityBL = new UploadUtilityBL();
            SaleInvoiceBL saleInvoiceBL = new SaleInvoiceBL();
            List<SaleInvoiceProductSerialDetailViewModel> productSerialDetail = new List<SaleInvoiceProductSerialDetailViewModel>();
            ResponseOut responseOut = new ResponseOut();
            HttpFileCollectionBase files = Request.Files;
            //int packingListTypeId = Convert.ToInt32(Request.Form["PackingTypeListId"].ToString());
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
                        string extension = System.IO.Path.GetExtension(fname).ToLower();
                        string query = null;
                        string connString = "";
                        string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                        string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), fname);
                        if (!Directory.Exists(path1))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                        }
                        if (validFileTypes.Contains(extension))
                        {
                            DataTable dt = new DataTable();
                            if (System.IO.File.Exists(path1))
                            { System.IO.File.Delete(path1); }
                            file.SaveAs(path1);
                            if (extension == ".csv")
                            {
                                dt = CommonHelper.ConvertCSVtoDataTable(path1);
                                ViewBag.Data = dt;
                            }
                            //Connection String to Excel Workbook  
                            else if (extension.Trim() == ".xls")
                            {
                                connString = "Provider=Microsoft.ACE.OLEDB.8.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                                dt = CommonHelper.ConvertXSLXtoDataTable(path1, connString);
                                ViewBag.Data = dt;
                            }
                            else if (extension.Trim() == ".xlsx")
                            {
                                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                                dt = CommonHelper.ConvertXSLXtoDataTable(path1, connString);
                                ViewBag.Data = dt;
                            }

                            // Code to Update ID based on Name of field
                            StringBuilder strErrMsg = new StringBuilder(" ");

                            if (dt.Rows.Count > 0)
                            {

                                Int32 productId = 0;
                                int rowCounter = 1;
                                SaleInvoiceViewModel saleInvoiceViewModel;
                                SaleInvoiceProductSerialDetailViewModel saleInvoiceProductSerialDetailViewModel;
                                dt.Columns.Add("VahaanDetail", typeof(string));
                                dt.Columns.Add("UploadStatus", typeof(bool));
                                bool rowVerifyStatus = true;
                                //Random rnd = new Random(50000);
                                foreach (DataRow dr in dt.Rows)
                                {
                                    saleInvoiceViewModel = new SaleInvoiceViewModel();
                                    saleInvoiceProductSerialDetailViewModel = new SaleInvoiceProductSerialDetailViewModel();

                                    //code to validate data in excel//
                                    //if (string.IsNullOrEmpty(Convert.ToString(dr["ProductName"])))
                                    //{
                                    //    strErrMsg.Append("Product Name Column has not proper data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    //    rowVerifyStatus = false;
                                    //}
                                     if (string.IsNullOrEmpty(Convert.ToString(dr["RefSerial1"]).Trim()))
                                    {
                                        strErrMsg.Append("Chasis Serial No. not proper format. " + rowCounter.ToString() + Environment.NewLine);
                                        rowVerifyStatus = false;
                                    }

                                    //end of code to validate data in excel//

                                    //code to get Id from Name data in excel//
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["RefSerial1"]).Trim()))
                                    {
                                        productId = Convert.ToInt32(utilityBL.GetIdByChasisSerialNo(Convert.ToString(dr["RefSerial1"]).Trim()));
                                        rowVerifyStatus = true;
                                        if (productId == 0)
                                        {
                                            strErrMsg.Append("Chasis Serial No. not found in Chasis Serial Mapping  Row No. " + rowCounter.ToString() +"    in Excel file."+ Environment.NewLine);
                                            rowVerifyStatus = false;
                                           
                                        }

                                    }
                                    string productName="";
                                    ProductPurchasePIBL productPurchasePIBL = new ProductPurchasePIBL();
                                    if(productId!=0)
                                    {
                                        productName = productPurchasePIBL.GetProductName(productId);
                                    }
                                    //if (!string.IsNullOrEmpty(Convert.ToString(dr["ProductName"])))
                                    //{
                                    //    productId = Convert.ToInt32(utilityBL.GetProductIdByProductName(Convert.ToString(dr["ProductName"])));
                                    //    if (productId == 0)
                                    //    {
                                    //        strErrMsg.Append("Invalid Product Name data in Row #" + rowCounter.ToString() + Environment.NewLine);
                                    //        rowVerifyStatus = false;
                                    //    }
                                    //}
                                    //End of code to get Id from Name data in excel//


                                    if (rowVerifyStatus == true)
                                    {
                                        productSerialDetail.Add(new SaleInvoiceProductSerialDetailViewModel
                                        {
                                            ProductId = productId,
                                            ProductName = productName.Trim(),
                                            RefSerial1 = Convert.ToString(dr["RefSerial1"]).Trim(),
                                            RefSerial2 = Convert.ToString(dr["RefSerial2"]).Trim(),
                                            RefSerial3 = Convert.ToString(dr["RefSerial3"]).Trim(),
                                            RefSerial4 = Convert.ToString(dr["RefSerial4"]).Trim(),
                                         // PackingListTypeID = packingListTypeId

                                        });
                                        string str = Convert.ToString(dr["RefSerial1"]);
                                        dr["VahaanDetail"] = Convert.ToString(dr["RefSerial1"]);
                                        dr["ProductName"] = productName;
                                        dr["UploadStatus"] = true;
                                      
                                    }
                                    else
                                    {
                                        dr["UploadStatus"] = false;
                                       
                                    
                                    }
                                    rowCounter += 1;

                                }
                                dt.AcceptChanges();
                            }
                            else
                            {
                                strErrMsg.Append("Import not found");
                               // productSerialDetail = null;
                            }

                            ViewBag.Error = strErrMsg.ToString();
                            // End of Code to Update ID based on Name of field
                           
                        }
                        else
                        {
                            ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format";
                      
                        }
                       
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
            return PartialView("GetSaleInvoiceProductSerialDetailList", productSerialDetail);
        }
        public FileResult DocumentDownload(string fileName)
        {
            var path = Path.Combine(Server.MapPath("~/Images/SIDocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public PartialViewResult GetSISupportingDocumentList(List<SISupportingDocumentViewModel> siDocuments, long siId)
        {            
            SaleInvoiceBL saleInvoiceBL = new SaleInvoiceBL();
            try
            {
                if (siDocuments == null)
                {
                    siDocuments = saleInvoiceBL.GetSISupportingDocumentList(siId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(siDocuments);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_GETGSTR1, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListGSTR1B2B()
        {
            try
            {
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
        public PartialViewResult GetGSTR1List(string fromDate = "", string toDate = "")
        {
            List<GSTR1ViewModel> gSTR1invoices = new List<GSTR1ViewModel>();
            SaleInvoiceBL saleinvoiceBL = new SaleInvoiceBL();
            try
            {
                gSTR1invoices = saleinvoiceBL.GetGSTR1B2BList(fromDate, toDate, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(gSTR1invoices);
        }

        public ActionResult ReportGSTR1B2B(string fromDate, string toDate, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SaleInvoiceBL saleInvoiceBL = new SaleInvoiceBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "GSTR1_B2BPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("GSTDataSet", saleInvoiceBL.GetGSTR1B2BDataTable(fromDate, toDate, ContextUser.CompanyId));
            lr.DataSources.Add(rd);

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>11.8in</PageWidth>" +
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

        public ActionResult ReportGSTR1B2CL(string fromDate, string toDate, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SaleInvoiceBL saleInvoiceBL = new SaleInvoiceBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "GSTR1_B2CLPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("GSTDataSet", saleInvoiceBL.GetGSTR1B2CLDataTable(fromDate, toDate, ContextUser.CompanyId));
            lr.DataSources.Add(rd);

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>9.8in</PageWidth>" +
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

        public ActionResult ReportGSTR1B2CS(string fromDate, string toDate, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SaleInvoiceBL saleInvoiceBL = new SaleInvoiceBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "GSTR1_B2CSPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("GSTDataSet", saleInvoiceBL.GetGSTR1B2CSDataTable(fromDate, toDate, ContextUser.CompanyId));
            lr.DataSources.Add(rd);

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>9.8in</PageWidth>" +
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

        public ActionResult ReportGSTR1CDNR(string fromDate, string toDate, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SaleInvoiceBL saleInvoiceBL = new SaleInvoiceBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "GSTR1_CDNRPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("GSTDataSet", saleInvoiceBL.GetGSTR1CDNRDataTable(fromDate, toDate, ContextUser.CompanyId));
            lr.DataSources.Add(rd);

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>18in</PageWidth>" +
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

        public ActionResult ReportGSTR1CDNUR(string fromDate, string toDate, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SaleInvoiceBL saleInvoiceBL = new SaleInvoiceBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "GSTR1_CDNURPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("GSTDataSet", saleInvoiceBL.GetGSTR1CDNURDataTable(fromDate, toDate, ContextUser.CompanyId));
            lr.DataSources.Add(rd);

            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>18in</PageWidth>" +
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
        public PartialViewResult GetSISOProductList(List<SOProductViewModel> soProducts, long soId)
        {          
            SaleInvoiceBL sIBL = new SaleInvoiceBL();
            try
            {
                if (soProducts == null)
                {
                    soProducts = sIBL.GetSISOProductList(soId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(soProducts);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ChassisNoSoldDetails, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult PrintChassisNoSoldDetails()
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

        public ActionResult ChassisNoSoldReport(string customerName="", long productId = 0, int productSubGroupId = 0, string chassisNo="",string invoiceNo="", string fromDate = "", string toDate = "", int companyBranchId=0, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SaleInvoiceBL saleInvoiceBL = new SaleInvoiceBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "ChassisNoSoldReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("PrintChassisNoSoldDetails");
            }

            DataTable dt = new DataTable();
            dt = saleInvoiceBL.GetChassisNoSoldDetailsDataTable(customerName, productId, productSubGroupId, ContextUser.CompanyId, chassisNo, invoiceNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyBranchId);

            ReportDataSource rd = new ReportDataSource("ChassisNoSoldDetailsDataSet", dt);
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
            "  <PageWidth>15in</PageWidth>" +
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SubGroupWiseChassisNoSoldDetails, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult PrintSubGroupWiseChassisNoSoldDetails()
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

        public ActionResult SubGroupWiseChassisNoSoldReport(int productSubGroupId = 0, string fromDate = "", string toDate = "", int companyBranchId = 0, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SaleInvoiceBL saleInvoiceBL = new SaleInvoiceBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "SubGroupWiseChassisQtySoldReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("SubGroupWiseChassisNoSoldDetails");
            }

            DataTable dt = new DataTable();
            dt = saleInvoiceBL.GetSubGroupWiseChassisNoSoldDetailsDataTable(productSubGroupId, ContextUser.CompanyId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyBranchId);

            ReportDataSource rd = new ReportDataSource("SubGroupWiseChassisQtySoldDataSet", dt);
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
            "  <PageWidth>7.5in</PageWidth>" +
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

        public ActionResult ReportGSTR3B(string fromDate, string toDate, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SaleInvoiceBL saleInvoiceBL = new SaleInvoiceBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "GSTR3BPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("GSTR3BDataSet", saleInvoiceBL.GetGSTR3BDataTable(fromDate, toDate, ContextUser.CompanyId));
            ReportDataSource rd1 = new ReportDataSource("GSTR3TempDataSet", saleInvoiceBL.GetGSTR3BTempDataTable(fromDate, toDate, ContextUser.CompanyId));
            ReportDataSource rd2 = new ReportDataSource("GSTR3BITCDataSet", saleInvoiceBL.GetGSTR3BITCDataTable(fromDate, toDate, ContextUser.CompanyId));
            ReportDataSource rd3 = new ReportDataSource("GSTR3NONGSTInwardSuppliesDataSet", saleInvoiceBL.GetGSTR3NONGSTInwardSupplies(fromDate, toDate, ContextUser.CompanyId));

            lr.DataSources.Add(rd);
            lr.DataSources.Add(rd1);
            lr.DataSources.Add(rd2);
            lr.DataSources.Add(rd3);
            DateTime datevalue = (Convert.ToDateTime(fromDate.ToString()));
            ReportParameter rp1 = new ReportParameter("FromDate", fromDate);
            ReportParameter rp2 = new ReportParameter("ToDate", toDate);
            ReportParameter rp3 = new ReportParameter("Month", datevalue.ToString("MMMM"));
            ReportParameter rp4 = new ReportParameter("Year", datevalue.Year.ToString());
            lr.SetParameters(rp1);
            lr.SetParameters(rp2);
            lr.SetParameters(rp3);
            lr.SetParameters(rp4);
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
        public PartialViewResult GetPorductSaleList(long productId,string companyBranch)
        {
            List<SaleInvoiceViewModel> siViewModel = new List<SaleInvoiceViewModel>();          
            SaleInvoiceBL saleInvoiceBL = new SaleInvoiceBL();
            try
            {
                siViewModel = saleInvoiceBL.GetPrpductSaleList(productId, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(siViewModel);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_GETGSTR1, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSaleInvoiceCancelReport()
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

        public ActionResult SaleInvoiceCancelReport(string cancelReportType, string fromDate, string toDate,string companyBranch,string customerName, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SaleInvoiceBL saleInvoiceBL = new SaleInvoiceBL();
            if (cancelReportType == "saleinvoice")
            {
                string path = Path.Combine(Server.MapPath("~/RDLC"), "SaleInvoicesCancelReport.rdlc");
                if (System.IO.File.Exists(path))
                {
                    lr.ReportPath = path;
                }
                else
                {
                    return View("Index");
                }
                ReportDataSource rd = new ReportDataSource("DataSet1", saleInvoiceBL.GetCancelSaleInvoices(fromDate, toDate, companyBranch,customerName, ContextUser.CompanyId));
                lr.DataSources.Add(rd);
            }
            if (cancelReportType == "saleorder")
            {
                string path = Path.Combine(Server.MapPath("~/RDLC"), "SaleOrderCancelReport.rdlc");
                if (System.IO.File.Exists(path))
                {
                    lr.ReportPath = path;
                }
                else
                {
                    return View("Index");
                }
                ReportDataSource rd = new ReportDataSource("DataSet1", saleInvoiceBL.GetCancelSaleOrders(fromDate, toDate, companyBranch, customerName, ContextUser.CompanyId));
                lr.DataSources.Add(rd);
            }
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
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
        public PartialViewResult GetJobCardList(string jobCardNo = "", string customerName = "", string approvalStatus = "", string modelName = "", string engineNo = "", string regNo = "", string keyNo = "", string fromDate = "", string toDate = "")
        {
            List<JobCardViewModel> jobCardViewModel = new List<JobCardViewModel>();
            JobCardBL jobCardBL = new JobCardBL();
            try
            {
                jobCardViewModel = jobCardBL.GetJobCardList(jobCardNo, customerName, approvalStatus, modelName, engineNo, regNo, keyNo, fromDate, toDate, ContextUser.CompanyId, Convert.ToInt32(Session[SessionKey.CustomerId]));
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(jobCardViewModel);
        }
        [HttpPost]
        public PartialViewResult GetSaleInvoiceJobCardProductList(List<SaleInvoiceProductViewModel> saleinvoiceProducts, long jobCardID)
        {
            SaleInvoiceBL siBL = new SaleInvoiceBL();
            try
            {
                if (saleinvoiceProducts == null)
                {
                    saleinvoiceProducts = siBL.GetSaleInvoiceJobCardProductList(jobCardID);
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
