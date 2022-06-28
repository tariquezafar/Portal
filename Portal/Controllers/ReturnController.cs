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
    public class ReturnController : BaseController
    {
        //
        // GET: /JobWork/


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Return, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditReturn(int returnedID = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (returnedID != 0)
                {
                    ViewData["returnedID"] = returnedID;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["returnedID"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Return, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditReturn(ReturnViewModel returnViewModel, List<ReturnedProductDetailViewModel> returnedProducts)
        {
            ResponseOut responseOut = new ResponseOut();            
            ReturnBL returnBL = new ReturnBL();
            try
            {
                if (returnViewModel != null)
                {
                    returnViewModel.CreatedBy = ContextUser.UserId;
                    returnViewModel.CompanyId = ContextUser.CompanyId;
                    if(string.IsNullOrEmpty(returnViewModel.InvoiceNo))
                    {
                        returnViewModel.InvoiceNo = "0";
                    }
                    returnViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = returnBL.AddEditReturn(returnViewModel, returnedProducts);

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
        public PartialViewResult GetReturnProductList(List<ReturnedProductDetailViewModel> returnProducts, long returnedID, string complaintId = null, int companyBranch = 0)
        {            
            ReturnBL returnBL = new ReturnBL();
            try
            {
                if(!string.IsNullOrEmpty(complaintId))
                {
                    returnProducts = returnBL.GetProductDetail(complaintId, companyBranch);
                }
                else if (returnProducts == null)
                {
                    returnProducts = returnBL.GetReturnProductList(returnedID);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(returnProducts);
        }
        [HttpGet]
        public PartialViewResult GetReturnList(string returnedNo = "", string invoiceNo = "",  string approvalStatus = "", int companyBranchId = 0, string fromDate = "", string toDate = "")
        {
            List<ReturnViewModel> returnViewModel = new List<ReturnViewModel>();
            ReturnBL returnBL = new ReturnBL();
            try
            {
                returnViewModel = returnBL.GetReturnList(returnedNo, invoiceNo, approvalStatus, companyBranchId, ContextUser.CompanyId, fromDate, toDate);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(returnViewModel);
        }

      

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Return, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListReturn()
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
        public PartialViewResult GetSaleInvoiceReturnList(string saleInvoiceNo = "", string invoicePackingListNo = "",int companyBranchId=0)
        {
            List<WarrantyViewModel> warrantyViewModel = new List<WarrantyViewModel>();
            ReturnBL returnBL = new ReturnBL();
            try
            {
                warrantyViewModel = returnBL.GetSaleInvoiceReturnList(saleInvoiceNo, invoicePackingListNo, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(warrantyViewModel);
        }

        [HttpGet]
        public PartialViewResult GetComplaintInvoiceReturnList(string complaintInvoiceNo = "", string customerMobileNo = "", int companyBranchId = 0)
        {
            List<ComplaintViewModel> complaintViewModel = new List<ComplaintViewModel>();
            ReturnBL returnBL = new ReturnBL();
            try
            {
                complaintViewModel = returnBL.GetComplaintInvoiceReturnList(complaintInvoiceNo, customerMobileNo, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(complaintViewModel);
        }

        [HttpGet]
        public JsonResult GetProductAutoCompleteWarrantyList(string term, long warrantyId,int warrantyStatus)
        {
            ReturnBL returnBL = new ReturnBL();
            List<WarrantyProductDetailViewModel> productList = new List<WarrantyProductDetailViewModel>();
            try
            {
                productList = returnBL.GetProductAutoCompleteWarrantyList(term, warrantyId, warrantyStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetReturnDetail(long returnedID)
        {
            ReturnBL returnBL = new ReturnBL();
            ReturnViewModel returnViewModel = new ReturnViewModel();
            try
            {
                returnViewModel = returnBL.GetReturnDetail(returnedID);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(returnViewModel, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Return, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListReturnReport()
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

        public ActionResult GenerateReturnSummaryReports(string returnedNo, string companyBranchId,string approvalStatus,string invoiceNo, string fromDate, string toDate,string customerName, string reportType = "PDF")

        {
            LocalReport lr = new LocalReport();
            ReturnBL returnBL = new ReturnBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "ReturnProductReports.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            ReportDataSource rd = new ReportDataSource("DataSet1", returnBL.GenerateReturnSummaryReports(returnedNo, companyBranchId, approvalStatus, invoiceNo,Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),customerName, ContextUser.UserId, ContextUser.CompanyId));
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
            "  <PageWidth>14.8in</PageWidth>" +
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


    }
}
