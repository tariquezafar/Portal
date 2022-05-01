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
    public class PurchaseReturnController : BaseController
    {
        #region PurchaseReturn
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PurchaseReturn, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPurchaseReturn(int purchaseReturnId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (purchaseReturnId != 0)
                {
                    ViewData["purchaseReturnId"] = purchaseReturnId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["purchaseReturnId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PurchaseReturn, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPurchaseReturn(PurchaseReturnViewModel purchaseReturnViewModel, List<PurchaseReturnProductViewModel> purchaseReturnProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            PurchaseReturnBL purchaseReturnBL = new PurchaseReturnBL();
            try
            {
                if (purchaseReturnViewModel != null)
                {
                    purchaseReturnViewModel.CreatedBy = ContextUser.UserId;
                    purchaseReturnViewModel.CompanyId = ContextUser.CompanyId;
                    purchaseReturnViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = purchaseReturnBL.AddEditPurchaseReturn(purchaseReturnViewModel, purchaseReturnProducts);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SaleReturn, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPurchaseReturn()
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
        public PartialViewResult GetPurchaseReturnList(string purchaseReturnNo = "", string vendorName = "", string dispatchrefNo = "", string fromDate = "", string toDate = "", string approvalStatus = "",string companyBranch="")
        {
            List<PurchaseReturnViewModel> purchaseReturnViewModel = new List<PurchaseReturnViewModel>();
            PurchaseReturnBL purchaseReturnBL = new PurchaseReturnBL();
            try
            {
                purchaseReturnViewModel = purchaseReturnBL.GetPurchaseReturnList(purchaseReturnNo, vendorName, dispatchrefNo, fromDate, toDate, approvalStatus, ContextUser.CompanyId, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(purchaseReturnViewModel);
        }

        [HttpGet]
        public JsonResult GetPurchaseReturnDetail(long purchaseReturnId)
        {
            PurchaseReturnViewModel purchaseReturnViewModel = new PurchaseReturnViewModel();
            PurchaseReturnBL purchaseReturnBL = new PurchaseReturnBL();
            try
            {
                purchaseReturnViewModel = purchaseReturnBL.GetPurchaseReturnDetail(purchaseReturnId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(purchaseReturnViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetPurchaseReturnProductList(List<PurchaseReturnProductViewModel> purchaseReturnProducts, long purchaseReturnId)
        {
            PurchaseReturnBL purchaseReturnBL = new PurchaseReturnBL();
            try
            {
                if (purchaseReturnProducts == null)
                {
                    purchaseReturnProducts = purchaseReturnBL.GetPurchaseReturnProductList(purchaseReturnId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(purchaseReturnProducts);
        }


        [HttpGet]
        public JsonResult GetVendorAutoCompleteList(string term)
        {
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
        public PartialViewResult GetPurchaseInvoiceList(string piNo = "", string vendorName = "", string refNo = "", string fromDate = "", string toDate = "", string approvalStatus = "", string displayType = "",string companyBranchId="")
        {
            List<PurchaseInvoiceViewModel> invoices = new List<PurchaseInvoiceViewModel>();         
            PurchaseReturnBL purchaseReturnBL = new PurchaseReturnBL();
            try
            {

                invoices = purchaseReturnBL.GetPurchasereturnPIList(piNo, vendorName, refNo, fromDate, toDate, ContextUser.CompanyId, approvalStatus, displayType,"", companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(invoices);
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

        public ActionResult Report(long purchaseReturnId, string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            PurchaseReturnBL purchaseReturnBL = new PurchaseReturnBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "PurchaseReturnPrint.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }

            DataTable dt = new DataTable();
            dt = purchaseReturnBL.GetPurchaseReturnDetailDataTable(purchaseReturnId);

            decimal totalInvoiceAmount = 0;
            totalInvoiceAmount = Convert.ToDecimal(dt.Rows[0]["TotalValue"].ToString());
            string totalWords = CommonHelper.ConvertMyword(Convert.ToInt32(totalInvoiceAmount));


            ReportDataSource rd = new ReportDataSource("DataSet1", dt);
            ReportDataSource rdProduct = new ReportDataSource("DataSetProduct", purchaseReturnBL.GetPurchaseReturnProductListDataTable(purchaseReturnId));
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
        public PartialViewResult GetPurchaseReturnPIProductList(List<PurchaseInvoiceProductDetailViewModel> piProducts, long invoiceId)
        {
           
            PurchaseReturnBL purchaseReturnBL = new PurchaseReturnBL();
            try
            {
                if (piProducts == null)
                {
                    piProducts = purchaseReturnBL.GetPurchaseReturnPIProductList(invoiceId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(piProducts);
        }


        #endregion
    }
}
