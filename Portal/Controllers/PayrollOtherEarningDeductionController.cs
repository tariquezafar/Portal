using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class PayrollOtherEarningDeductionController : BaseController
    {
        #region Payroll Other Earning Deduction
              
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PayrollOtherEarningDeduction, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPayrollOtherEarningDeduction(int monthlyInputId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (monthlyInputId != 0)
                {
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["monthlyInputId"] = monthlyInputId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["monthlyInputId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PayrollOtherEarningDeduction, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPayrollOtherEarningDeduction(PayrollOtherEarningDeductionViewModel payrollOtherEarningDeductionViewModel)
        {
            ResponseOut responseOut = new ResponseOut();            
            PayrollOtherEarningDeductionBL payrollOtherEarningDeductionBL = new PayrollOtherEarningDeductionBL();
            try
            {
                if (payrollOtherEarningDeductionViewModel != null)
                {                                    
                    payrollOtherEarningDeductionViewModel.CompanyId = ContextUser.CompanyId;                 
                    responseOut = payrollOtherEarningDeductionBL.AddEditPayrollOtherEarningDeduction(payrollOtherEarningDeductionViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PayrollOtherEarningDeduction, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPayrollOtherEarningDeduction()
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
        public PartialViewResult GetPayrollOtherEarningDeductionList(int payrollProcessingPeriodId = 0, int employeeId = 0, int departmentID = 0, int companyBranchID=0 )
        {
            List<PayrollOtherEarningDeductionViewModel> payrollOtherEarningDeductionViewModel = new List<PayrollOtherEarningDeductionViewModel>();           
            PayrollOtherEarningDeductionBL payrollOtherEarningDeduction = new PayrollOtherEarningDeductionBL();
            try
            {
                payrollOtherEarningDeductionViewModel = payrollOtherEarningDeduction.GetPayrollOtherEarningDeductionList(payrollProcessingPeriodId, employeeId, departmentID, companyBranchID, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(payrollOtherEarningDeductionViewModel);
        }

        [HttpGet]
        public JsonResult GetPayrollOtherEarningDeductionDetail(long monthlyInputId)
        {            
            PayrollOtherEarningDeductionBL payrollOtherEarningDeductionBL = new PayrollOtherEarningDeductionBL();
            PayrollOtherEarningDeductionViewModel payrollOtherEarningDeductionViewModel = new PayrollOtherEarningDeductionViewModel();          
            try
            {
                payrollOtherEarningDeductionViewModel = payrollOtherEarningDeductionBL.GetPayrollOtherEarningDeductionDetail(monthlyInputId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(payrollOtherEarningDeductionViewModel, JsonRequestBehavior.AllowGet);
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
