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
    public class EmployeeLeaveDetailController : BaseController
    {
        // GET: /EmployeeLeaveDetail/
              
        #region EmployeeLeaveDetail
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeLeaveDetail, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeLeaveDetail(int employeeLeaveId=0,int accessMode=3)
        {
            try
            {
                if (employeeLeaveId != 0)
                {
                    ViewData["employeeLeaveId"] = employeeLeaveId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["employeeLeaveId"] = 0;
                    ViewData["accessMode"] = 0;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                }

            }
            catch(Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeLeaveDetail, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeLeaveDetail(EmployeeLeaveDetailViewmodel employeeLeaveDetailViewmodel)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeLeaveDetailBL employeeLeaveDetailBL = new EmployeeLeaveDetailBL();
            try 
            {
                if (employeeLeaveDetailViewmodel != null)
                {
                    employeeLeaveDetailViewmodel.CreatedBy = ContextUser.UserId;
                    employeeLeaveDetailViewmodel.CompanyId = ContextUser.CompanyId;
                    employeeLeaveDetailViewmodel.FinYearId = Session[SessionKey.CurrentFinYear] != null ? ((FinYearViewModel)Session[SessionKey.CurrentFinYear]).FinYearId : DateTime.Now.Year;
                    responseOut = employeeLeaveDetailBL.AddEditEmployeeLeaveDetail(employeeLeaveDetailViewmodel);
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
        public JsonResult GetEmployeeLeaveDetail(int employeeLeaveId)
        {
            EmployeeLeaveDetailBL employeeLeaveDetailBL = new EmployeeLeaveDetailBL();
            EmployeeLeaveDetailViewmodel employeeLeaveDetail = new EmployeeLeaveDetailViewmodel();
            try
            {
                employeeLeaveDetail = employeeLeaveDetailBL.GetEmployeeLeaveDetail(employeeLeaveId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employeeLeaveDetail, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeLeaveDetail, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeLeaveDetail()
        {
            try
            {
                FinYearViewModel currentFinYear = (FinYearViewModel)Session[SessionKey.CurrentFinYear];
                ViewData["currentFinyearId"] = currentFinYear.FinYearId;
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
        public PartialViewResult GetEmployeeLeaveDetailList(int employeeId=0, int leaveTypeId=0, string fromDate="", string toDate="", int companyId=0, string status="")
        {
            List<EmployeeLeaveDetailViewmodel> employeeLeaveDetails = new List<EmployeeLeaveDetailViewmodel>();
            EmployeeLeaveDetailBL employeeLeaveDetailBL = new EmployeeLeaveDetailBL();
            try
            {

                employeeLeaveDetails = employeeLeaveDetailBL.GetEmployeeLeaveDetailList(employeeId, leaveTypeId, fromDate, toDate, ContextUser.CompanyId, status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeLeaveDetails);
        }

       

        #endregion


    }
}
