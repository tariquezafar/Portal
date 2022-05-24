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
    [CheckSessionBeforeControllerExecuteAttribute(Order =1)]
    public class ActivityCalenderController : BaseController
    {
        //
        // GET: /User/
        #region ActivityCalender
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ActivityCalender, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditActivityCalender(int activitycalenderId = 0, int accessMode = 3)
        {

            try
            {


                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (activitycalenderId != 0)
                {

                    ViewData["activitycalenderId"] = activitycalenderId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["activitycalenderId"] = 0;
                    ViewData["accessMode"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ActivityCalender, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditActivityCalender(ActivityCalenderViewModel activitycalenderViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ActivityCalenderBL activitycalenderBL = new ActivityCalenderBL();
            try
            {
                if (activitycalenderViewModel != null)
                { 
                    responseOut = activitycalenderBL.AddEditActivityCalender(activitycalenderViewModel);
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
        [HttpGet]
        public JsonResult GetCalenderList()
        {
           CalenderBL calenderBL = new CalenderBL();
            List<CalenderViewModel> calender = new List<CalenderViewModel>();
            try
            { 
                calender = calenderBL.GetCalenderList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(calender, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ActivityCalender, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListActivityCalender()
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

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
        public PartialViewResult GetActivityCalenderList( int calenderId = 0, string fromDate = "", string toDate = "", string Status = "",int companyBranchId=0)
        {
            List<ActivityCalenderViewModel> activityCalender = new List<ActivityCalenderViewModel>();
            ActivityCalenderBL activitycalenderBL = new ActivityCalenderBL();
            try
            {
                activityCalender = activitycalenderBL.GetActivityCalenderList(calenderId, fromDate, toDate, Status, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(activityCalender);
        }


        [HttpGet]
        public JsonResult GetActivityCalenderDetail(int activitycalenderId)
        {
            ActivityCalenderBL activitycalenderBL = new ActivityCalenderBL();
            ActivityCalenderViewModel activityCalender = new ActivityCalenderViewModel();
            try
            {
                activityCalender = activitycalenderBL.GetActivityCalenderDetail(activitycalenderId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(activityCalender, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}