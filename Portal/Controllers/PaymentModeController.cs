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
    public class PaymentModeController : BaseController
    {
        //
        // GET: /User/
        #region Payment Mode
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PaymentMode_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPaymentMode(int paymentModeId = 0, int accessMode = 3)
        {

            try
            {
                if (paymentModeId != 0)
                {

                    ViewData["paymentModeId"] = paymentModeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["paymentModeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PaymentMode_CP, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPaymentMode(PaymentModeViewModel paymentModeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            PaymentModeBL paymentModeBL = new PaymentModeBL();
            try
            {
                if (paymentModeViewModel != null)
                {
                   // paymentModeViewModel.PaymentModeId= ContextUser.CompanyId;
                   // paymentModeViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = paymentModeBL.AddEditPaymentMode(paymentModeViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PaymentMode_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPaymentMode()
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
        public JsonResult GetPaymentMode(string servicesName = "", string Status = "")
        {
            List<PaymentModeViewModel> paymentMode = new List<PaymentModeViewModel>();
            PaymentModeBL paymentModeBL = new PaymentModeBL();
            try
            {
                paymentMode = paymentModeBL.GetPaymentModeList(servicesName, Status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(paymentMode, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult GetPaymentModeList(string paymentModeName = "", string Status = "")
        {
            List<PaymentModeViewModel> paymentMode = new List<PaymentModeViewModel>();
            PaymentModeBL paymentModeBL = new PaymentModeBL();
            try
            {
                paymentMode = paymentModeBL.GetPaymentModeList(paymentModeName, Status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(paymentMode);
        }


        [HttpGet]
        public JsonResult GetPaymentModeDetail(int paymentModeId)
        {
            PaymentModeBL paymentModeBL = new PaymentModeBL();
            PaymentModeViewModel paymentMode = new PaymentModeViewModel();
            try
            {
                paymentMode = paymentModeBL.GetPaymentModeDetail(paymentModeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(paymentMode, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}