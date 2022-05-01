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
    public class PaymentTermController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PaymentTerm_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPaymentTerm(int paymenttermId = 0, int accessMode = 3)
        {

            try
            {
                if (paymenttermId != 0)
                {
                    ViewData["paymenttermId"] = paymenttermId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["paymenttermId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PaymentTerm_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPaymentTerm(PaymentTermViewModel paymenttermViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            PaymentTermBL paymenttermBL = new PaymentTermBL();
            try
            {
                if (paymenttermViewModel != null)
                {
                    paymenttermViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = paymenttermBL.AddEditPaymentTerm(paymenttermViewModel);

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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PaymentTerm_ACCOUNT, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPaymentTerm()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult GetPaymentTermList(string paymenttermDesc = "", string Status = "")
        {
            List<PaymentTermViewModel> paymentterm = new List<PaymentTermViewModel>();
            PaymentTermBL paymenttermBL = new PaymentTermBL();
            try
            {
                paymentterm = paymenttermBL.GetPaymentTermList(paymenttermDesc, Status, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(paymentterm);
        }


        [HttpGet]
        public JsonResult GetPaymentTermDetail(int paymenttermId)
        {
            PaymentTermBL paymenttermBL = new PaymentTermBL();
            PaymentTermViewModel paymentterm = new PaymentTermViewModel();
            try
            {
                paymentterm = paymenttermBL.GetPaymentTermDetail(paymenttermId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(paymentterm, JsonRequestBehavior.AllowGet);
        }

    }
}