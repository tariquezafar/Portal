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
    public class ShiftTypeController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ShiftType_HR, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditShiftType(int shiftTypeId = 0, int accessMode = 3)
        {

            try
            {
                if (shiftTypeId != 0)
                {
                    ViewData["shiftTypeId"] = shiftTypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["shiftTypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ShiftType_HR, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditShiftType(HR_ShiftTypeViewModel shiftTypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();                          
            ShiftTypeBL shiftTypeBL = new ShiftTypeBL();
            try
            {
                if (shiftTypeViewModel != null)
                {
                    responseOut = shiftTypeBL.AddEditShiftType(shiftTypeViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ShiftType_HR, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListShiftType()
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
        public PartialViewResult GetShiftTypeList(string shiftTypeName = "", string shiftTypeStatus = "")
        {
            List<HR_ShiftTypeViewModel> hR_ShiftTypeViewModel = new List<HR_ShiftTypeViewModel>();

            ShiftTypeBL shiftTypeBL = new ShiftTypeBL();
            try
            {
                hR_ShiftTypeViewModel = shiftTypeBL.GetShiftTypeList(shiftTypeName, shiftTypeStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(hR_ShiftTypeViewModel);
        }     
        [HttpGet]
        public JsonResult GetShiftTypeDetail(int shiftTypeId)
        {            
            ShiftTypeBL shiftTypeBL = new ShiftTypeBL();
          
            HR_ShiftTypeViewModel shiftTypeViewModel = new HR_ShiftTypeViewModel();
            try
            {
                shiftTypeViewModel = shiftTypeBL.GetShiftTypeDetail(shiftTypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(shiftTypeViewModel, JsonRequestBehavior.AllowGet);
        }

    }
}
