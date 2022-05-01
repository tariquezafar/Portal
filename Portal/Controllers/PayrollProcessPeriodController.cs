using Portal.Common;
using Portal.Core;
using Portal.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class PayrollProcessPeriodController : BaseController
    {
        #region Payroll Processing
        //
        // GET: /PayrollProcessPeriod/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PayrollProcessPeriod, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPayrollProcessPeriod(int payrollProcessingPeriodId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (payrollProcessingPeriodId != 0)
                {
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["payrollProcessingPeriodId"] = payrollProcessingPeriodId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["payrollProcessingPeriodId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PayrollProcessPeriod, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPayrollProcessPeriod(PayrollProcessPeriodViewModel payrollProcessPeriodViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            try
            {
                if (payrollProcessPeriodViewModel != null)
                {
                    payrollProcessPeriodViewModel.PayrollProcessStatus = "IN PROCESS";
                    payrollProcessPeriodViewModel.CreatedBy = ContextUser.UserId;
                    payrollProcessPeriodViewModel.CompanyId = ContextUser.CompanyId;
                    payrollProcessPeriodViewModel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = payrollProcessBL.AddEditPayrollProcessing(payrollProcessPeriodViewModel);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                    responseOut.trnId = 0;
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PayrollProcessPeriod, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPayrollProcessPeriod()
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
        public PartialViewResult GetPayrollProcessPeriodList(int monthId = 0, string payrollProcessStatus = "", string payrollLocked = "",string companyBranch="")
        {
            List<PayrollProcessPeriodViewModel> payrollPeriods = new List<PayrollProcessPeriodViewModel>();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            try
            {
                payrollPeriods = payrollProcessBL.GetPayrollProcessPeriodList(monthId, payrollProcessStatus, payrollLocked, ContextUser.CompanyId, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(payrollPeriods);
        }

        [HttpGet]
        public JsonResult GetPayrollProcessPeriodDetail(long payrollProcessingPeriodId)
        {
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            PayrollProcessPeriodViewModel payrollPeriodViewModel = new PayrollProcessPeriodViewModel();
            try
            {
                payrollPeriodViewModel = payrollProcessBL.GetPayrollProcessPeriodDetail(payrollProcessingPeriodId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(payrollPeriodViewModel, JsonRequestBehavior.AllowGet);
        }
      
        [HttpGet]
        public ActionResult LockUnlockPayrollProcessPeriod(int payrollProcessingPeriodId = 0, int accessMode = 3)
        {

            try
            {
                if (payrollProcessingPeriodId != 0)
                {
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["payrollProcessingPeriodId"] = payrollProcessingPeriodId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["payrollProcessingPeriodId"] = 0;
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
        public ActionResult LockUnlockPayrollProcessPeriod(PayrollProcessPeriodViewModel payrollProcessPeriodViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            PayrollProcessPeriodBL payrollProcessBL = new PayrollProcessPeriodBL();
            try
            {
                if (payrollProcessPeriodViewModel != null)
                {                   
                    payrollProcessPeriodViewModel.CreatedBy = ContextUser.UserId;                 
                    responseOut = payrollProcessBL.LockUnlockPayrollProcessPeriod(payrollProcessPeriodViewModel);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                    responseOut.trnId = 0;
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
        #endregion

        #region Helper Method
        [HttpGet]
        public JsonResult GetPayrollMonthList()
        {
            PayrollProcessPeriodBL payrollProcessPeriodBL = new PayrollProcessPeriodBL();
            List<PayrollMonthViewModel> payrollMonthList = new List<PayrollMonthViewModel>();

            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                payrollMonthList = payrollProcessPeriodBL.GetPayrollMonthList(finYear.FinYearId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(payrollMonthList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetESSPayrollMonthList()
        {
            PayrollProcessPeriodBL payrollProcessPeriodBL = new PayrollProcessPeriodBL();
            List<PayrollMonthViewModel> payrollMonthList = new List<PayrollMonthViewModel>();

            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                payrollMonthList = payrollProcessPeriodBL.GetESSPayrollMonthList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(payrollMonthList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPayrollMonthStartAndEndDate(int monthId)
        {
            PayrollProcessPeriodBL payrollProcessPeriodBL = new PayrollProcessPeriodBL();
            PayrollProcessPeriodViewModel payrollProcessDate = new PayrollProcessPeriodViewModel();

            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                payrollProcessDate = payrollProcessPeriodBL.GetPayrollMonthStartEndDate(monthId, finYear.FinYearId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(payrollProcessDate, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
