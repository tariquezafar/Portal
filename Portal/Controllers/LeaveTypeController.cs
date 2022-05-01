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
    public class LeaveTypeController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LeaveType, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditLeaveType(int leaveTypeId = 0, int accessMode = 3)
        {

            try
            {
                if (leaveTypeId != 0)
                {
                    ViewData["leaveTypeId"] = leaveTypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["leaveTypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LeaveType, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditLeaveType(HR_LeaveTypeViewModel leavetypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();                   
            LeaveTypeBL leaveTypeBL = new LeaveTypeBL();
            try
            {
                if (leavetypeViewModel != null)
                {
                    responseOut = leaveTypeBL.AddEditLeaveType(leavetypeViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_LeaveType, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListLeaveType()
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
        public PartialViewResult GetLeaveTypeList(string leavetypeName = "", string leaveTypeCode = "", string leavetypeStatus = "")
        {
            List<HR_LeaveTypeViewModel> hR_LeaveTypeViewModel = new List<HR_LeaveTypeViewModel>();

            LeaveTypeBL leaveTypeBL = new LeaveTypeBL();
            try
            {
                hR_LeaveTypeViewModel = leaveTypeBL.GetLeaveTypeList(leavetypeName, leaveTypeCode, leavetypeStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(hR_LeaveTypeViewModel);
        }     
        [HttpGet]
        public JsonResult GetLeaveTypeDetail(int leaveTypeId)
        {
            
            LeaveTypeBL leaveTypeBL = new LeaveTypeBL();
            HR_LeaveTypeViewModel hR_LeaveTypeViewModel = new HR_LeaveTypeViewModel();
            try
            {
                hR_LeaveTypeViewModel = leaveTypeBL.GetLeaveTypeDetail(leaveTypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(hR_LeaveTypeViewModel, JsonRequestBehavior.AllowGet);
        }

    }
}
