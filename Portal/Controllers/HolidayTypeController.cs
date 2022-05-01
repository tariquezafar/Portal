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
    public class HolidayTypeController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_HolidayType, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditHolidayType(int holidaytypeId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (holidaytypeId != 0)
                {
                    ViewData["holidaytypeId"] = holidaytypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["holidaytypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_HolidayType, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditHolidayType(HolidayTypeViewModel holidaytypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HolidayTypeBL holidaytypeBL = new HolidayTypeBL();
            try
            {
                if (holidaytypeViewModel != null)
                {
                    responseOut = holidaytypeBL.AddEditHolidayType(holidaytypeViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_HolidayType, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListHolidayType()
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
        public PartialViewResult GetHolidayTypeList(string holidaytypeName = "", string holidaytypeStatus = "",int companyBranchId=0)
        {
            List<HolidayTypeViewModel> holidaytypes = new List<HolidayTypeViewModel>();
            HolidayTypeBL holidaytypeBL = new HolidayTypeBL();
            try
            {
                holidaytypes = holidaytypeBL.GetHolidayTypeList(holidaytypeName, holidaytypeStatus, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(holidaytypes);
        }


        [HttpGet]
        public JsonResult GetHolidayTypeDetail(int holidaytypeId)
        {
            HolidayTypeBL holidaytypeBL = new HolidayTypeBL();
            HolidayTypeViewModel holidaytype = new HolidayTypeViewModel();
            try
            {
                holidaytype = holidaytypeBL.GetHolidayTypeDetail(holidaytypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(holidaytype, JsonRequestBehavior.AllowGet);
        }

    }
}
