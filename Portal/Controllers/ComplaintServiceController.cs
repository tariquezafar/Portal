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

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class ComplaintServiceController : BaseController
    {
        //
        // GET: /ComplaintService/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ComplaintService, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditComplaintService(int complaintServiceId = 0, int accessMode = 3)
        {
            
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (complaintServiceId != 0)
                {
                    ViewData["complaintServiceId"] = complaintServiceId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["complaintServiceId"] = 0;
                    ViewData["accessMode"] = 1;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ComplaintService, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditComplaintService(ComplaintServiceViewModel complaintServiceViewModel,List<ComplaintServiceProductDetailViewModel> complaintProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            ComplaintServiceBL complaintServiceBL = new ComplaintServiceBL();
            try
            {
                if (complaintServiceViewModel != null)
                {
                   
                    responseOut = complaintServiceBL.AddEditComplaintService(complaintServiceViewModel,complaintProducts);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
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
        public JsonResult GetCustomerMobileAutoCompleteList(string term)
        {
            CustomerBL customerBL = new CustomerBL();

            List<CustomerViewModel> customerList = new List<CustomerViewModel>();
            try
            {
                customerList = customerBL.GetCustomerMobileAutoCompleteList(term, ContextUser.CompanyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ComplaintService, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListComplaintService()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        public JsonResult GetComplaintProductAutoCompleteList(string term)
        {
            ProductBL productBL = new ProductBL();
            List<ProductViewModel> productList = new List<ProductViewModel>();
            try
            {
                productList = productBL.GetComplaintProductAutoCompleteList(term);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
           
        }

        [HttpGet]
        public PartialViewResult GetChallanSaleInvoiceList(string saleinvoiceNo = "", string customerName = "",string challanNo="", string fromDate = "", int companyBranchId = 0, string toDate = "",  string approvalStatus = "", string saleType = "")
        {
            List<SaleInvoiceViewModel> saleinvoices = new List<SaleInvoiceViewModel>();
            SaleInvoiceBL saleinvoiceBL = new SaleInvoiceBL();
            try
            {
                saleinvoices = saleinvoiceBL.GetChallanSaleInvoiceList(saleinvoiceNo, customerName, challanNo, fromDate, companyBranchId, toDate, approvalStatus,  saleType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(saleinvoices);
        }
        [HttpPost]
        public PartialViewResult GetComplaintServiceProductList(List<ComplaintServiceProductDetailViewModel> complaintServiceProducts, long complaintServiceId)
        {
            ComplaintServiceBL complaintServiceBL = new ComplaintServiceBL();
          try
            {
                if (complaintServiceProducts == null)
                {
                    complaintServiceProducts = complaintServiceBL.GetComplaintServiceProductList(complaintServiceId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(complaintServiceProducts);
        }

        [HttpGet]
        public PartialViewResult GetComplaintServiceList(string complaintNo = "", string enquiryType = "", string complaintMode = "", string customerMobile = "", string customerName = "", string approvalStatus="",int companyBranchId=0)
        {
            List<ComplaintServiceViewModel> complaints = new List<ComplaintServiceViewModel>();
            ComplaintServiceBL complaintServiceBL = new ComplaintServiceBL();

            try
            {
                complaints = complaintServiceBL.GetComplaintServiceList(complaintNo, enquiryType, complaintMode, customerMobile, customerName,approvalStatus, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(complaints);
        }

        public JsonResult GetComplaintServiceDetail(int ComplaintId)
        {
            ComplaintServiceBL complaintServiceBL = new ComplaintServiceBL();
            ComplaintServiceViewModel complaints = new ComplaintServiceViewModel();
            try
            {
                complaints = complaintServiceBL.GetComplaintServiceDetail(ComplaintId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(complaints, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public PartialViewResult GetComplaintServiceSIProductList(List<SaleInvoiceProductViewModel> saleinvoiceProducts, long saleinvoiceId)
        {
            ComplaintServiceBL complaintServiceBL = new ComplaintServiceBL();
            try
            {
                if (saleinvoiceProducts == null)
                {
                    saleinvoiceProducts = complaintServiceBL.GetComplaintServiceSIProductList(saleinvoiceId);
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
