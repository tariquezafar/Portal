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
    public class ShiftController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Shift_HR, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditShift(int shiftId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (shiftId != 0)
                {
                    ViewData["shiftId"] = shiftId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["shiftId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Shift_HR, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditShift(ShiftViewModel shiftViewModel)
        {
            ResponseOut responseOut = new ResponseOut();                                   
            HRShiftBL hRShiftBL = new HRShiftBL();
            try
            {
                if (shiftViewModel != null)
                {
                    shiftViewModel.CreatedBy = ContextUser.UserId;
                    shiftViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = hRShiftBL.AddEditShift(shiftViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Shift_HR, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListShift()
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
        [HttpGet]
        public PartialViewResult GetShiftList(string shiftName="",int shiftTypeId = 0, string shiftTypeStatus = "",int companyBranchId=0)
        {
            List<ShiftViewModel> shiftViewModel = new List<ShiftViewModel>();
            HRShiftBL hRShiftBL = new HRShiftBL(); 
            try
            {
                shiftViewModel = hRShiftBL.GetShiftList(shiftName, shiftTypeId, shiftTypeStatus, ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(shiftViewModel);
        }     
        [HttpGet]
        public JsonResult GetShiftDetail(int shiftId)
        {
            HRShiftBL hRShiftBL = new HRShiftBL();

            ShiftViewModel shiftViewModel = new ShiftViewModel();
            try
            {
                shiftViewModel = hRShiftBL.GetShiftDetail(shiftId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(shiftViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetShiftTypeList()
        {
            ShiftTypeBL shiftTypeBL = new ShiftTypeBL();
            List<HR_ShiftTypeViewModel> shiftTypeViewModel = new List<HR_ShiftTypeViewModel>();
            try
            {
                shiftTypeViewModel = shiftTypeBL.GetHRShiftTypeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(shiftTypeViewModel, JsonRequestBehavior.AllowGet);
        }

    }
}
