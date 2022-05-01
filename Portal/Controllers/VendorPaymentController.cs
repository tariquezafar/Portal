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
    public class VendorPaymentController : BaseController
    {
        #region Vendor Payment

        //
        // GET: /Quotation/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_VendorPayment, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditVendorPayment(int paymenttrnId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");


                if (paymenttrnId != 0)
                {
                    ViewData["paymenttrnId"] = paymenttrnId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["paymenttrnId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_VendorPayment, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditVendorPayment(VendorPaymentViewModel vendorpaymentViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            VendorPaymentBL vendorpaymentBL = new VendorPaymentBL();
            try
            {
                if (vendorpaymentViewModel != null)
                {
                    vendorpaymentViewModel.CreatedBy = ContextUser.UserId;
                    vendorpaymentViewModel.CompanyId = ContextUser.CompanyId;
                    vendorpaymentViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = vendorpaymentBL.AddEditVendorPayment(vendorpaymentViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_VendorPayment, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListVendorPayment()
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
        public PartialViewResult GetVendorPaymentList(string paymentNo = "", string vendorName = "", string invoiceNo = "", string refNo = "", string fromDate = "", string toDate = "")
        {
            List<VendorPaymentViewModel> vendorpayments = new List<VendorPaymentViewModel>();
            VendorPaymentBL vendorpaymentBL = new VendorPaymentBL();
            try
            {
                vendorpayments = vendorpaymentBL.GetVendorPaymentList(paymentNo, vendorName, invoiceNo, refNo, fromDate, toDate, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(vendorpayments);
        }

        [HttpGet]
        public JsonResult GetVendorPaymentDetail(long paymenttrnId)
        {
            VendorPaymentBL vendorpaymentBL = new VendorPaymentBL();
            VendorPaymentViewModel vendorpayment = new VendorPaymentViewModel();
            try
            {
                vendorpayment = vendorpaymentBL.GetVendorPaymentDetail(paymenttrnId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(vendorpayment, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetPIList(string piNo = "", string vendorName = "", string refNo = "", string fromDate = "", string toDate = "", string approvalStatus = "", string displayType = "")
        {
            List<PurchaseInvoiceViewModel> pis = new List<PurchaseInvoiceViewModel>();
            PurchaseInvoiceBL piBL = new PurchaseInvoiceBL();
            try
            {
                pis = piBL.GetPIList(piNo, vendorName, refNo, fromDate, toDate, ContextUser.CompanyId, approvalStatus, displayType,"","0","","");
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(pis);
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
 
        #endregion
    }
}
