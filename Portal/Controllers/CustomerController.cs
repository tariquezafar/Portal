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
    public class CustomerController : BaseController
    {
        //
        // GET: /Customer/
        #region Customer
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Customer, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditCustomer(int customerId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                
                if (customerId != 0)
                {
                    ViewData["customerId"] = customerId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["customerId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Customer, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditCustomer(CustomerViewModel customerViewModel, List<CustomerBranchViewModel> customerBranchs, List<CustomerProductViewModel> customerProducts,List<CustomerFollowUpViewModel> customerFollowUps)
        {
            ResponseOut responseOut = new ResponseOut();
            CustomerBL customerBL = new CustomerBL();
            try
            {
                if (customerViewModel != null)
                {
                    customerViewModel.CreatedBy = ContextUser.UserId;
                    customerViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = customerBL.AddEditCustomer(customerViewModel, customerBranchs, customerProducts, customerFollowUps);
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

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Customer, (int)AccessMode.EditAccess, (int)RequestMode.Ajax)]
        public ActionResult RemoveCustomerBranch(long customerBranchId = 0)
        {
            ResponseOut responseOut = new ResponseOut();
            CustomerBL customerBL = new CustomerBL();
            try
            {
                responseOut = customerBL.RemoveCustomerBranch(customerBranchId);
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Customer, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListCustomer(string listStatus="false")
        {
            try
            {
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["ListStatus"] = listStatus;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult GetCustomerList(string customerName = "", string customerCode = "", string mobileNo = "", int customerTypeid = 0, string city = "", string state = "", string customerStatus = "",int companyBranchId=0)
        {
            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            CustomerBL customerBL = new CustomerBL();
            try
            {
                customers = customerBL.GetCustomerList(customerName, customerCode, mobileNo, customerTypeid,city,state, ContextUser.CompanyId, customerStatus, companyBranchId, ((UserViewModel)Session[SessionKey.UserId]).UserId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(customers);
        }

        [HttpPost]
        public PartialViewResult GetCustomerBranchList(List<CustomerBranchViewModel> customerBranchs, int customerId)
        {
            CustomerBL customerBL = new CustomerBL();
            try
            {
                if (customerBranchs == null)
                {
                    customerBranchs = customerBL.GetCustomerBranchList(customerId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(customerBranchs);
        }

        [HttpPost]
        public PartialViewResult GetCustomerProductList(List<CustomerProductViewModel> customerProducts, int customerId)
        {
            
            CustomerBL customerBL = new CustomerBL();
            try
            {
                if (customerProducts == null)
                {
                    customerProducts = customerBL.GetCustomerProductList(customerId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(customerProducts);
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Customer, (int)AccessMode.EditAccess, (int)RequestMode.Ajax)]
        public ActionResult RemoveCustomerProduct(long mappingId = 0)
        {
            ResponseOut responseOut = new ResponseOut();
            CustomerBL customerBL = new CustomerBL();
            try
            {
                responseOut = customerBL.RemoveCustomerProduct(mappingId);
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Customer, (int)AccessMode.EditAccess, (int)RequestMode.Ajax)]
        public ActionResult RemoveVendorProduct(long mappingId = 0)
        {
            ResponseOut responseOut = new ResponseOut();
            CustomerBL customerBL = new CustomerBL();
            try
            {
                responseOut = customerBL.RemoveVendorProduct(mappingId);
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
        public JsonResult GetCustomerDetail(int customerId)
        {
            CustomerBL customerBL = new CustomerBL();
            CustomerViewModel customer = new CustomerViewModel();
            try
            {
                customer = customerBL.GetCustomerDetail(customerId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(customer, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCustomerTypeList()
        {
            CustomerTypeBL customerTypeBL = new CustomerTypeBL();
            List<CustomerTypeViewModel> customerTypes = new List<CustomerTypeViewModel>();
            try
            { 
                customerTypes = customerTypeBL.GetCustomerTypeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(customerTypes, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Customer, (int)AccessMode.EditAccess, (int)RequestMode.Ajax)]
        public JsonResult CustomerFollowUpValidation(CustomerFollowUpViewModel customerFollowUps)
        {
            ResponseOut responseOut = new ResponseOut();
            CustomerBL customerBL = new CustomerBL();
            try
            {
                responseOut = customerBL.CustomerFollowUpValidation(customerFollowUps);
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
        public PartialViewResult GetCustomerFollowUpList(List<CustomerFollowUpViewModel> customerFollowUps, int custId)
        {
            CustomerBL customerBL = new CustomerBL();
            try
            {
                if (customerFollowUps == null)
                {
                    customerFollowUps = customerBL.GetCustomerFollowUpList(custId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(customerFollowUps);

        }

        public ActionResult CustomerExport(string customerName = "", string customerCode = "", string mobileNo = "", int customerTypeid = 0, string city = "", string state = "", string customerStatus = "", string reportType = "PDF")
        {
            LocalReport lr = new LocalReport();
            SORegisterBL sORegisterBL = new SORegisterBL();
            CustomerBL customerBL = new CustomerBL();
            string path = Path.Combine(Server.MapPath("~/RDLC"), "CustomerReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("ListCustomer");
            }
            ReportDataSource rd = new ReportDataSource("CustomerReportDataSet", customerBL.CustomerExport(customerName, customerCode, mobileNo, customerTypeid, ContextUser.CompanyId,city,state,customerStatus));
            lr.DataSources.Add(rd);                     
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =

             "<DeviceInfo>" +
            "  <OutputFormat>" + reportType + "</OutputFormat>" +
            "  <PageWidth>22.5in</PageWidth>" +
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


        //Customer Pop Up Master------------

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Customer, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditCustomerMaster(CustomerViewModel customerViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            CustomerBL customerBL = new CustomerBL();
            try
            {
                if (customerViewModel != null)
                {
                    customerViewModel.CreatedBy = ContextUser.UserId;
                    customerViewModel.CompanyId = ContextUser.CompanyId;

                    responseOut = customerBL.AddEditCustomerMaster(customerViewModel);
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
        //Customer Details By Customer Id
        [HttpPost]
        public JsonResult GetCustomerDetailsById(int customerId)
        {
            CustomerBL customerBL = new CustomerBL();
            List<CustomerViewModel> customerList = new List<CustomerViewModel>();
            try
            {
                customerList = customerBL.GetCustomerDetailsById(customerId, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(customerList, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public PartialViewResult GetTodayCustomerList()
        {
            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            CustomerBL customerBL = new CustomerBL();
            try
            {
                customers = customerBL.GetTodayCustomerList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(customers);
        }

        [HttpGet]
        
        public ActionResult BindCustomerByType(int customerTypeId)
        {
            List<CustomerFormViewModel> customers = new List<CustomerFormViewModel>();
            CustomerBL customerBL = new CustomerBL();
            try
            {
                customers = customerBL.GetCustomerByTypeId(customerTypeId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json( customers,JsonRequestBehavior.AllowGet);
        }
        #endregion



    }
}
