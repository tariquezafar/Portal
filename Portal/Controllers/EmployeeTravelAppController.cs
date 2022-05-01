using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices;
using System.IO;
using System.Data;
using System.Text;
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class EmployeeTravelAppController : BaseController
    {
        #region Employee Travel Application
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeTravelApplication, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeTravelApp(int applicationId = 0, int accessMode = 3, int essEmployeeId = 0, string essEmployeeName = "")
        {
            try
            {
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (applicationId != 0)
                {
                    ViewData["applicationId"] = applicationId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["essEmployeeId"] = essEmployeeId;
                    ViewData["essEmployeeName"] = essEmployeeName;

                }
                else
                {
                    ViewData["applicationId"] = 0;
                    ViewData["accessMode"] = 3;
                    ViewData["essEmployeeId"] = essEmployeeId;
                    ViewData["essEmployeeName"] = essEmployeeName;

                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeTravelApplication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditEmployeeTravelApp(HR_EmployeeTravelApplicationViewModel employeetravelApplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeTravelAppBL employeeadvanceAppBL= new EmployeeTravelAppBL();
            try
            {
                if (employeetravelApplicationViewModel != null)
                {
                    employeetravelApplicationViewModel.CompanyId = ContextUser.CompanyId; 
                    responseOut = employeeadvanceAppBL.AddEditEmployeeTravelApp(employeetravelApplicationViewModel); 

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeTravelApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeTravelApp(int essEmployeeId = 0, string essEmployeeName = "")
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["essEmployeeId"] = essEmployeeId;
                ViewData["essEmployeeName"] = essEmployeeName;

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpGet]
        public PartialViewResult GetEmployeeTravelAppList(string applicationNo = "", int employeeId = 0, string travelTypeId = "",  string travelStatus = "0", string travelDestination="", string fromDate = "", string toDate = "", int essEmployeeId = 0, string essEmployeeName = "",string companyBranch="")
        {
            List<HR_EmployeeTravelApplicationViewModel> employeetravelApplications = new List<HR_EmployeeTravelApplicationViewModel>();
            EmployeeTravelAppBL employeetravelAppBL = new EmployeeTravelAppBL();
            try
            {
                ViewData["essEmployeeId"] = essEmployeeId;
                ViewData["essEmployeeName"] = essEmployeeName;
                employeetravelApplications = employeetravelAppBL.GetEmployeeTravelAppList(applicationNo, essEmployeeId, travelTypeId, travelStatus, travelDestination,fromDate, toDate, ContextUser.CompanyId, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeetravelApplications);
        }

        [HttpGet]
        public JsonResult GetEmployeTravelAppDetail(long applicationId)
        {
            EmployeeTravelAppBL employeetravelAppBL = new EmployeeTravelAppBL();
            HR_EmployeeTravelApplicationViewModel employeetravelapplication = new HR_EmployeeTravelApplicationViewModel();
            try
            {
                employeetravelapplication = employeetravelAppBL.GetEmployeeTravelAppDetail(applicationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employeetravelapplication, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTravelTypeForEmpolyeeTravelAppList()
        {
            TravelTypeBL traveltypeBL = new TravelTypeBL();
            List<HR_TravelTypeViewModel> travelapplication = new List<HR_TravelTypeViewModel>();
            try
            {
                travelapplication = traveltypeBL.GetTravelTypeForEmpolyeeTravelAppList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(travelapplication, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Employee Travel Application Approval
        [ValidateRequest(true, UserInterfaceHelper.ApproveEmployeeTravelApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeTravelAppApproval()
        {
            try
            {
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
        public PartialViewResult GetEmployeeTravelAppApprovalList(string applicationNo = "", int employeeId = 0, string travelTypeId = "", string travelStatus = "0", string travelDestination = "", string fromDate = "", string toDate = "")
        {
            List<HR_EmployeeTravelApplicationViewModel> employeetravelApplications = new List<HR_EmployeeTravelApplicationViewModel>();
            EmployeeTravelAppBL employeetravelAppBL = new EmployeeTravelAppBL();
            try
            {
                employeetravelApplications = employeetravelAppBL.GetEmployeeTravelAppApprovalList(applicationNo, employeeId, travelTypeId, travelStatus, travelDestination,fromDate, toDate, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeetravelApplications);
        }
        [ValidateRequest(true, UserInterfaceHelper.ApproveEmployeeTravelApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ApproveEmployeeTravelApp(int applicationId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                if (applicationId != 0)
                {
                    ViewData["applicationId"] = applicationId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["applicationId"] = 0;
                    ViewData["accessMode"] = 3;

                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.ApproveEmployeeTravelApplication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult ApproveEmployeeTravelApp(HR_EmployeeTravelApplicationViewModel employeetravelApplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeTravelAppBL employeetravelAppBL = new EmployeeTravelAppBL();
            try
            {
                if (employeetravelApplicationViewModel != null)
                {
                    employeetravelApplicationViewModel.CompanyId = ContextUser.CompanyId;
                    employeetravelApplicationViewModel.ApproveBy = ContextUser.UserId;
                    responseOut = employeetravelAppBL.ApproveRejectEmployeeTravelApp(employeetravelApplicationViewModel);

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


    }
}
