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
    public class EmployeeClaimApplicationController : BaseController
    {
        #region Employee Claim Application

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeClaimApplication, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeClaimApplication(int applicationId = 0, int accessMode = 3, int essEmployeeId = 0, string essEmployeeName = "")
        {
            try
            {
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");

                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeClaimApplication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditEmployeeClaimApplication(EmployeeClaimApplicationViewModel employeeClaimApplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();        
            EmployeeClaimApplicationBL employeeClaimApplicationBL = new EmployeeClaimApplicationBL();


            try
            {
                if (employeeClaimApplicationViewModel != null)
                {
                    employeeClaimApplicationViewModel.CompanyId = ContextUser.CompanyId;                  
                    responseOut = employeeClaimApplicationBL.AddEditEmployeeClaimApplication(employeeClaimApplicationViewModel); 

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeClaimApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeClaimApplication(int essEmployeeId = 0, string essEmployeeName = "")
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

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
        public PartialViewResult GetEmployeeClaimApplicationList(string applicationNo = "", int employeeId = 0, int claimTypeId =0, string claimStatus = "0", string fromDate = "", string toDate = "", int essEmployeeId = 0, string essEmployeeName = "",int companyBranchId=0)
        {
            List<EmployeeClaimApplicationViewModel> employeeClaimApplicationViewModel = new List<EmployeeClaimApplicationViewModel>();
           
            EmployeeClaimApplicationBL employeeClaimApplicationBL = new EmployeeClaimApplicationBL();
            try
            {
                ViewData["essEmployeeId"] = essEmployeeId;
                ViewData["essEmployeeName"] = essEmployeeName;
                employeeClaimApplicationViewModel = employeeClaimApplicationBL.GetEmployeeClaimApplicationList(applicationNo, essEmployeeId, claimTypeId, claimStatus, fromDate, toDate,ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeClaimApplicationViewModel);
        }

        [HttpGet]
        public JsonResult GetEmployeeClaimApplicationDetail(long applicationId)
        {           
            EmployeeClaimApplicationBL employeeClaimApplicationBL = new EmployeeClaimApplicationBL();
            EmployeeClaimApplicationViewModel employeeClaimApplicationViewModel = new EmployeeClaimApplicationViewModel();

            try
            {
                employeeClaimApplicationViewModel = employeeClaimApplicationBL.GetEmployeeClaimApplicationDetail(applicationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employeeClaimApplicationViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeeClaimApplicationTypeList()
        {                   
            ClaimTypeBL ClaimTypeBL = new ClaimTypeBL();
            List<HR_ClaimTypeViewModel> claimTypeViewModel = new List<HR_ClaimTypeViewModel>();            
            try
            {
                claimTypeViewModel = ClaimTypeBL.GetEmployeeClaimApplicationTypeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(claimTypeViewModel, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Approval Employee Claim Application

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ApprovalEmployeeClaimApplication, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult ApprovalRejectedEmployeeClaimApplication(int applicationId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ApprovalEmployeeClaimApplication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult ApprovalRejectedEmployeeClaimApplication(EmployeeClaimApplicationViewModel employeeClaimApplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();         
            EmployeeClaimApplicationBL employeeClaimApplicationBL = new EmployeeClaimApplicationBL();
            try
            {
                if (employeeClaimApplicationViewModel != null)
                {
                    employeeClaimApplicationViewModel.CompanyId = ContextUser.CompanyId;
                    employeeClaimApplicationViewModel.ApproveBy = ContextUser.UserId;
                    employeeClaimApplicationViewModel.RejectBy = ContextUser.UserId;
                    responseOut = employeeClaimApplicationBL.ApprovalRejectedEmployeeClaimApplication(employeeClaimApplicationViewModel);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ApprovalEmployeeClaimApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeClaimApplicationApproval()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
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
        public PartialViewResult GetEmployeeClaimApplicationApprovalList(string applicationNo = "", int employeeId = 0, int claimTypeId =0, string claimStatus = "0", string fromDate = "", string toDate = "",int companyBranchId=0)
        {
            List<EmployeeClaimApplicationViewModel> employeeClaimApplicationViewModel = new List<EmployeeClaimApplicationViewModel>();           
            EmployeeClaimApplicationBL employeeClaimApplicationBL = new EmployeeClaimApplicationBL();
            try
            {
                employeeClaimApplicationViewModel = employeeClaimApplicationBL.GetEmployeeClaimApplicationApprovalList(applicationNo, employeeId, claimTypeId, claimStatus, fromDate, toDate, ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeClaimApplicationViewModel);
        }

        #endregion



    }
}
