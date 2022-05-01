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
    public class CalenderController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Calender, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditCalender(int calenderId = 0, int accessMode = 3)
        {

            try
            {
                if (calenderId != 0)
                {
                    ViewData["calenderId"] = calenderId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["calenderId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Calender, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditCalender(CalenderViewModel calenderViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            CalenderBL calenderBL = new CalenderBL();
            try
            {
                if (calenderViewModel != null)
                {
                    calenderViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = calenderBL.AddEditCalender(calenderViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Calender, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListCalender()
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
        public PartialViewResult GetCalenderList(string calenderName = "", int calenderYear = 0, string calenderStatus = "")
        {
            List<CalenderViewModel> calenders = new List<CalenderViewModel>();
            CalenderBL calenderBL = new CalenderBL();
            try
            {
                calenders = calenderBL.GetCalenderList(calenderName, calenderYear, calenderStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(calenders);
        }


        [HttpGet]
        public JsonResult GetCalenderDetail(int calenderId)
        {
            CalenderBL calenderBL = new CalenderBL();
            CalenderViewModel calender = new CalenderViewModel();
            try
            {
                calender = calenderBL.GetCalenderDetail(calenderId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(calender, JsonRequestBehavior.AllowGet);
        }

    }
}
