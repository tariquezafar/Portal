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
    public class SLTypeController : BaseController
    {
        //
        // GET: /SLType/
        #region SLType
       [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SLType_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSLType(int sLTypeId = 0, int accessMode = 3)
        {

            try
            {
                if (sLTypeId != 0)
                {

                    ViewData["sLTypeId"] = sLTypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["sLTypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SLType_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSLType(SLTypeViewModel sLTypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
             SLTypeBL sLTypeBL = new SLTypeBL();
            try
            {
                if (sLTypeViewModel != null)
                {
                    // paymentModeViewModel.PaymentModeId= ContextUser.CompanyId;
                    // paymentModeViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = sLTypeBL.AddEditSLType(sLTypeViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_SLType_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSLType()
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
        public PartialViewResult GetSLTypeList(string sLTypeName = "", string Status = "")
        {
            List<SLTypeViewModel> sLType = new List<SLTypeViewModel>();
            SLTypeBL sLTypeBL = new SLTypeBL();
            try
            {
                sLType = sLTypeBL.GetSLTypeList(sLTypeName, Status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(sLType);
        }


        [HttpGet]
        public JsonResult GetSLTypeDetail(int sLTypeId)
        {
            SLTypeBL sLTypeBL = new SLTypeBL();
            SLTypeViewModel sLType = new SLTypeViewModel();
            try
            {
                sLType = sLTypeBL.GetSLTypeDetail(sLTypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(sLType, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}
