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
    public class HolidayCalenderController : BaseController
    {
        //
        // GET: /User/
        #region ActivityCalender
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_HolidayCalender, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditHolidayCalender(int holidaycalenderId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (holidaycalenderId != 0)
                {

                    ViewData["holidaycalenderId"] = holidaycalenderId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["holidaycalenderId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_HolidayCalender, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditHolidayCalender(HolidayCalenderViewModel holidaycalenderViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HolidayCalenderBL holidaycalenderBL = new HolidayCalenderBL();
            try
            {
                if (holidaycalenderViewModel != null)
                { 
                    responseOut = holidaycalenderBL.AddEditHolidayCalender(holidaycalenderViewModel);
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

        public JsonResult GetHolidayTypeIdList(int companyBranchId)
        {
            HolidayTypeBL holidaytypeBL = new HolidayTypeBL();
            List<HolidayTypeViewModel> holidaytype = new List<HolidayTypeViewModel>();
            try
            {
                holidaytype = holidaytypeBL.GetHolidayTypeIdList(companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(holidaytype, JsonRequestBehavior.AllowGet);
        }


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_HolidayCalender, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListHolidayCalender()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
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
        public PartialViewResult GetHolidayCalenderList(int calenderId = 0, int holidaytypeId = 0, string fromDate = "", string toDate = "", string Status = "",int companyBranchId=0)
        {
            List<HolidayCalenderViewModel> holidayCalender = new List<HolidayCalenderViewModel>();
            HolidayCalenderBL holidaycalenderBL = new HolidayCalenderBL();
            try
            {
                holidayCalender = holidaycalenderBL.GetHolidayCalenderList(calenderId, holidaytypeId,fromDate, toDate, Status, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(holidayCalender);
        }


        [HttpGet]
        public JsonResult GetHolidayCalenderDetail(int holidaycalenderId)
        {
            HolidayCalenderBL holidaycalenderBL = new HolidayCalenderBL();
            HolidayCalenderViewModel holidayCalender = new HolidayCalenderViewModel();
            try
            {
                holidayCalender = holidaycalenderBL.GetHolidayCalenderDetail(holidaycalenderId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(holidayCalender, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}