using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class CustomerFormController : BaseController
    {
        #region CustomerForm

        //
        // GET: /CustomerForm/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CustomerForm, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditCustomerForm(int customerFormTrnId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");


                if (customerFormTrnId != 0)
                {
                    ViewData["CustomerFormTrnId"] = customerFormTrnId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["CustomerFormTrnId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CustomerForm, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditCustomerForm(CustomerFormViewModel customerFormViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            CustomerPaymentBL customerpaymentBL = new CustomerPaymentBL();
            CustomerFormBL customerFormBL = new CustomerFormBL();
            try
            {
                if (customerFormViewModel != null)
                {
                    customerFormViewModel.CreatedBy = ContextUser.UserId;
                    customerFormViewModel.CompanyId = ContextUser.CompanyId;
                  
                    responseOut = customerFormBL.AddEditCustomerForm(customerFormViewModel); 
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_CustomerForm, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListCustomerForm()
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
        public PartialViewResult GetCustomerFormList(string formStatus = "0", string customerName = "",  string invoiceNo = "", string refNo = "", string fromDate = "", string toDate = "")
        {
         
            List<CustomerFormViewModel> customerFormViewModel = new List<CustomerFormViewModel>();
            CustomerFormBL customerFormBL = new CustomerFormBL(); 
            try
            {
                customerFormViewModel = customerFormBL.GetCustomerFormList(formStatus, customerName, invoiceNo, refNo, fromDate, toDate, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(customerFormViewModel);
        }

        [HttpGet]
        public JsonResult GetCustomerFormDetail(long customerFormTrnId)
        {         
            CustomerFormBL customerFormBL = new CustomerFormBL();
            CustomerFormViewModel customerFormViewModel = new CustomerFormViewModel();
            try
            {
                customerFormViewModel = customerFormBL.GetCustomerFormDetail(customerFormTrnId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(customerFormViewModel, JsonRequestBehavior.AllowGet);
        }


     
        [HttpGet]
        public PartialViewResult GetSaleInvoiceList(string saleinvoiceNo = "", string customerName = "", string refNo = "", string fromDate = "", string toDate = "",string invoiceType="",string displayType="",string approvalStatus="")
        {
            List<SaleInvoiceViewModel> invoices = new List<SaleInvoiceViewModel>();
            SaleInvoiceBL saleinvoiceBL = new SaleInvoiceBL();
            try
            {
                
                invoices = saleinvoiceBL.GetSaleInvoiceList(saleinvoiceNo, customerName, refNo, fromDate, toDate, ContextUser.CompanyId, invoiceType, displayType, approvalStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(invoices);
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
        public JsonResult GetBookList()
        {
            BookBL bookBL = new BookBL();
            List<BookViewModel> bookList = new List<BookViewModel>();
            try
            {
                bookList = bookBL.GetBookList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(bookList, JsonRequestBehavior.AllowGet);
        }
         
        [HttpGet]
        public JsonResult GetPaymentModeList()
        {
            PaymentModeBL paymentModeBL = new PaymentModeBL();
            List<PaymentModeViewModel> paymentList = new List<PaymentModeViewModel>();
            try
            {
                paymentList = paymentModeBL.GetPaymentModeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(paymentList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetFormTypeList()
        {
            
            FormTypeBL formTypeBL = new FormTypeBL();
            List<FormTypeViewModel> FormTypeViewModelList = new List<FormTypeViewModel>();
            
            try
            {
                FormTypeViewModelList = formTypeBL.GetFormTypeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(FormTypeViewModelList, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
