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
    public class RoasterController : BaseController
    {
        #region Roaster
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Roaster, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditRoaster(int roasterId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (roasterId != 0)
                {
                    ViewData["roasterId"] = roasterId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["roasterId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Roaster, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditRoaster(HR_RoasterViewModel roasterViewModel,List<HR_RoasterWeekViewModel> weeks)
        {
            ResponseOut responseOut = new ResponseOut();
            RoasterBL roasterBL = new RoasterBL();
            try
            {
                if (roasterViewModel != null)
                {
                    roasterViewModel.CreatedBy = ContextUser.UserId;
                    roasterViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = roasterBL.AddEditRoaster(roasterViewModel,weeks);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Roaster, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListRoaster()
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
        public PartialViewResult GetRoasterList(string roasterName = "", string fromDate = "", string toDate = "", string roasterType="", int department=0,  string roasterStatus = "",string companyBranch="")
        {
            List<HR_RoasterViewModel> roasters = new List<HR_RoasterViewModel>();
            RoasterBL roasterBL = new RoasterBL();
            try
            {
                roasters = roasterBL.GetRoasterList(roasterName, fromDate, toDate, roasterType, department, ContextUser.CompanyId, roasterStatus, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(roasters);
        }
    
        [HttpGet]
        public JsonResult GetRoasterDetail(int roasterId)
        {
            RoasterBL roasterBL = new RoasterBL();
            HR_RoasterViewModel roaster = new HR_RoasterViewModel();
            try
            {
                roaster = roasterBL.GetRoasterDetail(roasterId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(roaster, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetRoasterWeekList(List<HR_RoasterWeekViewModel> weeks, int roasterId)
        {
            RoasterBL roasterBL = new RoasterBL();
            try
            {
                if (weeks == null)
                {
                    weeks = roasterBL.GetRoasterWeekList(roasterId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(weeks);
        }
        #endregion

        #region Roster Change Employee
        [ValidateRequest(true, UserInterfaceHelper.UpdateDateWiseShift, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult UpdateDateWiseShift(int employeeRoasterId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (employeeRoasterId != 0)
                {
                    ViewData["employeeRoasterId"] = employeeRoasterId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["employeeRoasterId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.UpdateDateWiseShift, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult UpdateDateWiseShift(int rosterId, int shiftId, string startDate,string endDate,int companyBranchId, List<HR_EmployeeRosterViewModel> employeeRosters)
        {
            ResponseOut responseOut = new ResponseOut();
            RoasterBL roasterBL = new RoasterBL();
            try
            {
                if (rosterId != 0)
                {
                    responseOut = roasterBL.UpdateEmployeeRosterShift(rosterId,shiftId,startDate,endDate,ContextUser.UserId, companyBranchId,employeeRosters);
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
        public PartialViewResult GetRosterLinkedEmployeeList(int roasterId)
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            RoasterBL roasterBL = new RoasterBL();
            try
            {
                employees = roasterBL.GetRosterLinkedEmployeeDetail(roasterId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employees);
        }

        #endregion
        #region Helper Method
        [HttpGet]
        public JsonResult GetShiftList()
        {
            ShiftBL shiftBL = new ShiftBL();
            List<ShiftViewModel> shiftList = new List<ShiftViewModel>();
            try
            {
                shiftList = shiftBL.GetShiftList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(shiftList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRosterList()
        {
            RoasterBL rosterBL = new RoasterBL();
            List<HR_RoasterViewModel> rosterList = new List<HR_RoasterViewModel>();
            try
            {
                rosterList = rosterBL.GetRosterList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(rosterList, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
