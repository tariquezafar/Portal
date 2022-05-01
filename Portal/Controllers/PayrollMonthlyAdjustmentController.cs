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
    public class PayrollMonthlyAdjustmentController : BaseController
    {
        #region Payroll Monthly Adjustment

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PayrollMonthlyAdjustment, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPayrollMonthlyAdjustment(int payrollAdjustmentId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (payrollAdjustmentId != 0)
                {
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["payrollAdjustmentId"] = payrollAdjustmentId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["payrollAdjustmentId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PayrollMonthlyAdjustment, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPayrollMonthlyAdjustment(PayrollMonthlyAdjustmentViewModel payrollMonthlyAdjustmentViewModel)
        {
            ResponseOut responseOut = new ResponseOut();                       
            PayrollMonthlyAdjustmentBL payrollMonthlyAdjustmentBL = new PayrollMonthlyAdjustmentBL();
            try
            {
                if (payrollMonthlyAdjustmentViewModel != null)
                {
                    payrollMonthlyAdjustmentViewModel.CompanyId = ContextUser.CompanyId;
                    payrollMonthlyAdjustmentViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = payrollMonthlyAdjustmentBL.AddEditPayrollMonthlyAdjustment(payrollMonthlyAdjustmentViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PayrollMonthlyAdjustment, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPayrollMonthlyAdjustment()
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
        public PartialViewResult GetPayrollMonthlyAdjustmentList(int payrollProcessingPeriodId = 0, int employeeId = 0, int departmentID = 0, int companyBranchID=0 )
        {
            List<PayrollMonthlyAdjustmentViewModel> payrollMonthlyAdjustmentViewModel = new List<PayrollMonthlyAdjustmentViewModel>();           
            PayrollMonthlyAdjustmentBL payrollMonthlyAdjustmentBL = new PayrollMonthlyAdjustmentBL();
            try
            {
                payrollMonthlyAdjustmentViewModel = payrollMonthlyAdjustmentBL.GetPayrollMonthlyAdjustmentList(payrollProcessingPeriodId, employeeId, departmentID, companyBranchID, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(payrollMonthlyAdjustmentViewModel);
        }

        [HttpGet]
        public JsonResult GetPayrollMonthlyAdjustmentDetail(long payrollAdjustmentId)
        {                                 
            PayrollMonthlyAdjustmentBL payrollMonthlyAdjustmentBL = new PayrollMonthlyAdjustmentBL();
            PayrollMonthlyAdjustmentViewModel payrollMonthlyAdjustmentViewModel = new PayrollMonthlyAdjustmentViewModel();
            try
            {
                payrollMonthlyAdjustmentViewModel = payrollMonthlyAdjustmentBL.GetPayrollMonthlyAdjustmentDetail(payrollAdjustmentId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(payrollMonthlyAdjustmentViewModel, JsonRequestBehavior.AllowGet);
        }
      
        #endregion

     
    }
}
