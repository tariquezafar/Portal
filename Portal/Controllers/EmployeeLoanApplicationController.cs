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
    public class EmployeeLoanApplicationController : BaseController
    {
        #region Employee Loan Application

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeLoanApplication, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeLoanApplication(int applicationId = 0, int accessMode = 3,int essEmployeeId=0,string essEmployeeName="")
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeLoanApplication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditEmployeeLoanApplication(EmployeeLoanApplicationViewModel employeeLoanApplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeLoanApplicationBL employeeLoanApplicationBL = new EmployeeLoanApplicationBL();
           
            try
            {
                if (employeeLoanApplicationViewModel != null)
                {
                    employeeLoanApplicationViewModel.CompanyId = ContextUser.CompanyId;
                    employeeLoanApplicationViewModel.ApproveBy = ContextUser.UserId;
                    responseOut = employeeLoanApplicationBL.AddEditEmployeeLoanApplication(employeeLoanApplicationViewModel); 

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeLoanApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeLoanApplication(int essEmployeeId = 0, string essEmployeeName = "")
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
        public PartialViewResult GetEmployeeLoanApplicationList(string applicationNo = "", int employeeId = 0, string loanTypeName = "", string loanStatus = "0", string fromDate = "", string toDate = "", int essEmployeeId = 0, string essEmployeeName = "")
        {
            List<EmployeeLoanApplicationViewModel> employeeLoanApplicationViewModel = new List<EmployeeLoanApplicationViewModel>();
           
            EmployeeLoanApplicationBL employeeLoanApplicationBL = new EmployeeLoanApplicationBL();
            try
            {
                ViewData["essEmployeeId"] = essEmployeeId;
                ViewData["essEmployeeName"] = essEmployeeName;
                employeeLoanApplicationViewModel = employeeLoanApplicationBL.GetEmployeeLoanApplicationList(applicationNo, essEmployeeId, loanTypeName, loanStatus, fromDate, toDate,ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeLoanApplicationViewModel);
        }

        [HttpGet]
        public JsonResult GetEmployeeLoanApplicationDetail(long applicationId)
        {           
            EmployeeLoanApplicationBL employeeLoanApplicationBL = new EmployeeLoanApplicationBL();            
            EmployeeLoanApplicationViewModel employeeLoanApplicationViewModel = new EmployeeLoanApplicationViewModel();
            try
            {
                employeeLoanApplicationViewModel = employeeLoanApplicationBL.GetEmployeeLoanApplicationDetail(applicationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employeeLoanApplicationViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeeLoanApplicationTypeList()
        {

            LoanTypeBL loanTypeBL = new LoanTypeBL();           
            List<HR_LoanTypeViewModel> loanTypeViewModel = new List<HR_LoanTypeViewModel>();
            try
            {
                loanTypeViewModel = loanTypeBL.GetEmployeeLoanApplicationTypeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(loanTypeViewModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Approval Employee Loan Application

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeApprovalLoanApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ApprovalEmployeeLoanApplication(int applicationId = 0, int accessMode = 3)
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeApprovalLoanApplication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult ApproveRejectEmployeeLoanApplication(EmployeeLoanApplicationViewModel employeeLoanApplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeLoanApplicationBL employeeLoanApplicationBL = new EmployeeLoanApplicationBL();

            try
            {
                if (employeeLoanApplicationViewModel != null)
                {
                    employeeLoanApplicationViewModel.CompanyId = ContextUser.CompanyId;
                    employeeLoanApplicationViewModel.ApproveBy = ContextUser.UserId;
                    employeeLoanApplicationViewModel.RejectBy= ContextUser.UserId;
                    responseOut = employeeLoanApplicationBL.ApproveRejectEmployeeLoanApplication(employeeLoanApplicationViewModel);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeApprovalLoanApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeLoanApplicationApproval()
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
        public PartialViewResult GetEmployeeLoanApplicationApprovalList(string applicationNo = "", int employeeId = 0, string loanTypeName = "", string loanStatus = "0", string fromDate = "", string toDate = "")
       {
            List<EmployeeLoanApplicationViewModel> employeeLoanApplicationViewModel = new List<EmployeeLoanApplicationViewModel>();

            EmployeeLoanApplicationBL employeeLoanApplicationBL = new EmployeeLoanApplicationBL();
            try
            {
                employeeLoanApplicationViewModel = employeeLoanApplicationBL.GetEmployeeLoanApplicationApprovalList(applicationNo, employeeId, loanTypeName, loanStatus, fromDate, toDate, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeLoanApplicationViewModel);
        }

        [HttpGet]
        public JsonResult GetEmployeeLoanApplicationApprovalDetail(long applicationId)
        {
            EmployeeLoanApplicationBL employeeLoanApplicationBL = new EmployeeLoanApplicationBL();
            EmployeeLoanApplicationViewModel employeeLoanApplicationViewModel = new EmployeeLoanApplicationViewModel();
            try
            {
                employeeLoanApplicationViewModel = employeeLoanApplicationBL.GetEmployeeLoanApplicationApprovalDetail(applicationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employeeLoanApplicationViewModel, JsonRequestBehavior.AllowGet);
        }       
        #endregion



    }
}
