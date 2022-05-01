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
    public class ScheduleController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Schedule_ACCOUNT, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditSchedule(int scheduleId = 0, int accessMode = 3)
        {

            try
            {
                if (scheduleId != 0)
                {
                    ViewData["ScheduleId"] = scheduleId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["ScheduleId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Schedule_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditSchedule(ScheduleViewModel scheduleViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ScheduleBL scheduleBL = new ScheduleBL();
            try
            {
                if (scheduleViewModel != null)
                {
                    scheduleViewModel.CreatedBy = ContextUser.UserId;
                    scheduleViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = scheduleBL.AddEditSchedule(scheduleViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Schedule_ACCOUNT, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListSchedule()
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
        public PartialViewResult GetScheduleList(string scheduleName = "", int scheduleNo = 0 ,string status = "")
        {
            List<ScheduleViewModel> scheduleViewModel = new List<ScheduleViewModel>();
            ScheduleBL scheduleBL = new ScheduleBL();           
            try
            {
                scheduleViewModel = scheduleBL.GetScheduleList(scheduleName, scheduleNo,ContextUser.CompanyId, status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(scheduleViewModel);
        }


        [HttpGet]
        public JsonResult GetScheduleDetail(int scheduleId)
        {
            ScheduleBL scheduleBL = new ScheduleBL();
            ScheduleViewModel scheduleViewModel = new ScheduleViewModel();         
            try
            {
                scheduleViewModel = scheduleBL.GetScheduleDetail(scheduleId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(scheduleViewModel, JsonRequestBehavior.AllowGet);
        }

    }
}
