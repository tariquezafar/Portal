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
    public class FollowUpActivityTypeController :BaseController
    {
        //
        // GET: /User/
        #region FollowUpActivityType
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_FollowUpActivityType_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditFollowUpActivityType(int followUpActivityTypeId = 0, int accessMode = 3)
        {

            try
            {
                if (followUpActivityTypeId != 0)
                {

                    ViewData["followUpActivityTypeId"] = followUpActivityTypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["followUpActivityTypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_FollowUpActivityType_CP, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditFollowUpActivityType(FollowUpActivityTypeViewModel followUpActivityTypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            FollowUpActivityTypeBL followUpActivityTypeBL = new FollowUpActivityTypeBL();
            try
            {
                if (followUpActivityTypeViewModel != null)
                {
                    // paymentModeViewModel.PaymentModeId= ContextUser.CompanyId;
                    // paymentModeViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = followUpActivityTypeBL.AddEditFollowUpActivityType(followUpActivityTypeViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_FollowUpActivityType_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListFollowUpActivityType()
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
        public PartialViewResult GetFollowUpActivityTypeList(string followUpActivityTypeName = "", string Status = "")
        {
            List<FollowUpActivityTypeViewModel> followUpActivityTypeViewModel = new List<FollowUpActivityTypeViewModel>();
            FollowUpActivityTypeBL followUpActivityTypeBL = new FollowUpActivityTypeBL();
            try
            {
                followUpActivityTypeViewModel = followUpActivityTypeBL.GetFollowUpActivityTypeList(followUpActivityTypeName, Status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(followUpActivityTypeViewModel);
        }


        [HttpGet]
        public JsonResult GetFollowUpActivityTypeDetail(int followUpActivityTypeId)
        {
            FollowUpActivityTypeBL followUpActivityTypeBL = new FollowUpActivityTypeBL();
            FollowUpActivityTypeViewModel followUpActivityType = new FollowUpActivityTypeViewModel();
            try
            {
                followUpActivityType = followUpActivityTypeBL.GetFollowUpActivityTypeDetail(followUpActivityTypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(followUpActivityType, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}
