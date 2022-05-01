using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.IO;
using Microsoft.Reporting.WebForms;

namespace Portal.Controllers
{
    public class PayrollTdsSlabController : BaseController
    {
        //
        // GET: /PayrollTdsSlab/

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PayrollTds, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPayrollTdsSlab(int PayrollTdsSlaBid = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (PayrollTdsSlaBid != 0)
                {
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["tdsSlaBid"] = PayrollTdsSlaBid;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    ViewData["tdsSlaBid"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PayrollTds, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPayrollTdsSlab(PayRollTdsViewModel payRollTdsViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            PayrollTdsBL PayrollTdsBL = new PayrollTdsBL();
            try
            {
                if (payRollTdsViewModel != null)
                {
                    //PayRollTdsViewModel.TdsSlaBid = 0;
                    payRollTdsViewModel.Companyid = ContextUser.CompanyId;
                    payRollTdsViewModel.CreatedBy = ContextUser.UserId;
                    payRollTdsViewModel.Modifiedby = ContextUser.UserId;
                    responseOut = PayrollTdsBL.AddEditPayrollTds(payRollTdsViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PayrollTds, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPayrollTds()
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
        public PartialViewResult GetPayrollTdsList(string txtProductMainGroupName, string txtProductMainGroupCode, string ddlCategory)
        {
            List<PayRollTdsViewModel> payRollTds = new List<PayRollTdsViewModel>();
            PayrollTdsBL payrollTdsBL = new PayrollTdsBL();
            try
             {
                payRollTds = payrollTdsBL.GetPayRollTdsList(txtProductMainGroupName, txtProductMainGroupCode, ddlCategory);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(payRollTds);
        }

        [HttpGet]
        public JsonResult GetPayrollTdsDetails(int PayrollTdsid)
        {
            PayrollTdsBL payrollTdsBL = new PayrollTdsBL();
            PayRollTdsViewModel payrollTdsGroup = new PayRollTdsViewModel();
            try
            {
                payrollTdsGroup = payrollTdsBL.GetPayrollTdsDetails(PayrollTdsid);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(payrollTdsGroup, JsonRequestBehavior.AllowGet);
        }
        
    }
}
