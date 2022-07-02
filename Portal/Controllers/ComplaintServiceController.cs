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
using Portal.Common.ViewModel;

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
                ViewData["RoleId"] = Session[SessionKey.RoleId] != null ? ((UserViewModel)Session[SessionKey.RoleId]).RoleId : 0;
                int userid = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                UserBL userBL = new UserBL();
                UserViewModel user = userBL.GetDetails(userid);
                if(user != null)
                {
                    ViewData["userId"] = user.UserId;
                    ViewData["username"] = user.UserName;
                }
                ViewData["RoleId"] = ContextUser.RoleId;
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
        public ActionResult AddEditComplaintService(ComplaintServiceViewModel complaintServiceViewModel,List<ComplaintServiceProductDetailViewModel> complaintProducts, List<ComplaintServiceSupportingDocumentViewModel> complaintDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            ComplaintServiceBL complaintServiceBL = new ComplaintServiceBL();
            try
            {
                if (complaintServiceViewModel != null)
                {
                   
                    responseOut = complaintServiceBL.AddEditComplaintService(complaintServiceViewModel,complaintProducts, complaintDocuments);
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
        public PartialViewResult GetComplaintServiceList(string complaintNo = "", string enquiryType = "", string complaintMode = "", string customerMobile = "", string customerName = "", string approvalStatus="",int companyBranchId=0, int serviceEngineerId = 0, int dealerId = 0, int complaintStatus = 0)
        {
            List<ComplaintServiceViewModel> complaints = new List<ComplaintServiceViewModel>();
            ComplaintServiceBL complaintServiceBL = new ComplaintServiceBL();
            int roleId = Session[SessionKey.RoleId] != null ? ((UserViewModel)Session[SessionKey.RoleId]).RoleId : 0;
            try
            {
                if(roleId == 104)
                {
                    complaintStatus = 1;
                }
                complaints = complaintServiceBL.GetComplaintServiceList(complaintNo, enquiryType, complaintMode, customerMobile, customerName,approvalStatus, companyBranchId, serviceEngineerId, dealerId, complaintStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(complaints);
        }

        [HttpGet]
        public PartialViewResult GetAPCSComplaintServiceList(string complaintNo = "", string enquiryType = "", string complaintMode = "", string customerMobile = "", string customerName = "", string approvalStatus = "", int companyBranchId = 0, int serviceEngineerId = 0, int dealerId = 0, int complaintStatus = 4)
        {
            List<ComplaintServiceViewModel> complaints = new List<ComplaintServiceViewModel>();
            ComplaintServiceBL complaintServiceBL = new ComplaintServiceBL();

            try
            {
                complaints = complaintServiceBL.GetComplaintServiceList(complaintNo, enquiryType, complaintMode, customerMobile, customerName, approvalStatus, companyBranchId, serviceEngineerId, dealerId, complaintStatus);
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

        public FileResult DocumentDownload(string fileName)
        {
            var path = Path.Combine(Server.MapPath("~/Images/ComplaintDocument"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
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
                        var path = Path.Combine(Server.MapPath("~/Images/ComplaintDocument"), newFileName);
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
        public PartialViewResult GetComplaintSupportingDocumentList(List<ComplaintServiceSupportingDocumentViewModel> complaintDocuments, long complaintID)
        {

            ComplaintServiceBL complaintServiceBL = new ComplaintServiceBL();
            try
            {
                if (complaintDocuments == null)
                {
                    complaintDocuments = complaintServiceBL.GetComplaintSupportingDocumentList(complaintID);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(complaintDocuments);
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

        /// <summary>
        /// This method is used to get customer Type List.
        /// Author By : Dheeraj kumar on 21 May, 2022
        /// </summary>
        /// <param name="customerTypeId">primary key of the table</param>
        /// <returns>
        /// This method retruns list of customer based on parameters.
        /// </returns>
        [HttpGet]
        public JsonResult GetCustomerTypeList()
        {
            List<SelectListModel> lstSelectListModel = new List<SelectListModel>();
            ComplaintServiceBL complaintServiceBL = new ComplaintServiceBL();
            try
            {
                lstSelectListModel = complaintServiceBL.GetCustomerTypeList(6);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(lstSelectListModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ComplaintService, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListAPCS()
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

    }
}
