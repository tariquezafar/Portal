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
    public class SaleReturnController : BaseController
    {
        #region SaleReturn
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SaleReturn, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSaleReturn(int saleReturnId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (saleReturnId != 0)
                {
                    ViewData["saleReturnId"] = saleReturnId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["saleReturnId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SaleReturn, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditSaleReturn(SaleReturnViewModel saleReturnViewModel, List<SaleReturnProductViewModel> saleReturnProducts,List<SaleRetrunProductSerialDetailViewModel> saleRetrunProductSerialDetailViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SaleReturnBL saleReturnBL = new SaleReturnBL();
            try
            {
                if (saleReturnViewModel != null)
                {
                    saleReturnViewModel.CreatedBy = ContextUser.UserId;
                    saleReturnViewModel.CompanyId = ContextUser.CompanyId;
                    saleReturnViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = saleReturnBL.AddEditSaleReturn(saleReturnViewModel, saleReturnProducts, saleRetrunProductSerialDetailViewModel);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SaleReturn, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSaleReturn(string totalSaleReturnList="false")
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["totalSaleReturnList"] = totalSaleReturnList;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }


        [HttpGet]
        public PartialViewResult GetSaleReturnList(string saleReturnNo = "", string customerName = "", string dispatchrefNo = "", string fromDate = "", string toDate = "",string approvalStatus="",string CreatedByUserName="", int companyBranchId=0)
        {
            List<SaleReturnViewModel> saleReturnViewModel = new List<SaleReturnViewModel>();
            SaleReturnBL saleReturnBL = new SaleReturnBL();
            try
            {
                saleReturnViewModel = saleReturnBL.GetSaleReturnList(saleReturnNo, customerName, dispatchrefNo, fromDate, toDate, approvalStatus, ContextUser.CompanyId, CreatedByUserName, companyBranchId, Convert.ToInt32(Session[SessionKey.CustomerId]));
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(saleReturnViewModel);
        }

        [HttpGet]
        public JsonResult GetSaleReturnDetail(long saleReturnId)
        {

            SaleReturnViewModel saleReturnViewModel = new SaleReturnViewModel();
            SaleReturnBL saleReturnBL = new SaleReturnBL();
            try
            {
                saleReturnViewModel = saleReturnBL.GetSaleReturnDetail(saleReturnId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(saleReturnViewModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult GetSaleReturnProductList(List<SaleReturnProductViewModel> saleReturnProducts, long saleReturnId)
        {
            SaleReturnBL saleReturnBL = new SaleReturnBL();
            try
            {
                if (saleReturnProducts == null)
                {
                    saleReturnProducts = saleReturnBL.GetSaleReturnProductList(saleReturnId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(saleReturnProducts);
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
        public PartialViewResult GetSaleReturnSIList(string saleinvoiceNo = "", string customerName = "", string refNo = "", string fromDate = "", string toDate = "",string invoiceType="",string displayType="",string approvalStatus="",int companyBranchId=0)
        {
            List<SaleInvoiceViewModel> invoices = new List<SaleInvoiceViewModel>();           
            SaleReturnBL saleReturnBL = new SaleReturnBL();
            try
            {
                invoices = saleReturnBL.GetSaleReturnSIList(saleinvoiceNo, customerName, refNo, fromDate, toDate, ContextUser.CompanyId,invoiceType,displayType,approvalStatus,"",companyBranchId);
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

        public ActionResult Report(long saleReturnId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SaleReturnBL saleReturnBL = new SaleReturnBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "SaleReturnPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable dt = new DataTable();
            dt = saleReturnBL.GetSaleReturnDetailDataTable(saleReturnId);

            decimal totalInvoiceAmount = 0;
            totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
            string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));

            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdProduct = new ReportDataSource("DataSetProduct", saleReturnBL.GetSaleReturnProductListDataTable(saleReturnId));
            lr.DataSources.Add(rd);
            lr.DataSources.Add(rdProduct);
            
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
        public PartialViewResult GetSaleReturnSIProductList(List<SaleInvoiceProductViewModel> saleinvoiceProducts, long saleinvoiceId)
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
        public PartialViewResult GetSaleReturnChasisSIProductList(long saleinvoiceId,int mode)
        {
            SaleInvoiceBL siBL = new SaleInvoiceBL();

            List<SaleInvoiceChasisProductSerialDetailViewModel> saleInvoiceChasisProductSerialDetailViewModel = new List<SaleInvoiceChasisProductSerialDetailViewModel>();
            try
            {
                if (saleinvoiceId!=0)
                {
                    saleInvoiceChasisProductSerialDetailViewModel = siBL.GetSaleInvoiceChasisProductList(saleinvoiceId, mode);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(saleInvoiceChasisProductSerialDetailViewModel);
        }

        public ActionResult CancelSaleReturn(long saleReturnID=0,string cancelReason="")
        {
            ResponseOut responseOut = new ResponseOut();
            SaleReturnBL saleReturnBL = new SaleReturnBL();
            SaleReturnViewModel saleReturnViewModel = new SaleReturnViewModel();
            try
            {
                if (saleReturnID >0)
                {
                    saleReturnViewModel.SaleReturnId = saleReturnID;
                    saleReturnViewModel.CreatedBy = ContextUser.UserId;
                    saleReturnViewModel.CompanyId = ContextUser.CompanyId;
                    saleReturnViewModel.CancelReason = cancelReason;
                    saleReturnViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = saleReturnBL.CancelSaleReturn(saleReturnViewModel);

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
