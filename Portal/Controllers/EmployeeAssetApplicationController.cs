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
    public class EmployeeAssetApplicationController : BaseController
    {
        #region Employee Asset Application

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeAssetApplication, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeAssetApplication(int applicationId = 0, int accessMode = 3, int essEmployeeId = 0, string essEmployeeName = "")
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeAssetApplication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditEmployeeAssetApplication(EmployeeAssetApplicationViewModel employeeAssetApplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();        
            EmployeeAssetApplicationBL employeeAssetApplicationBL = new EmployeeAssetApplicationBL();


            try
            {

                if (employeeAssetApplicationViewModel != null)
                {
                    employeeAssetApplicationViewModel.CompanyId = ContextUser.CompanyId;
                    employeeAssetApplicationViewModel.ApproveBy = ContextUser.UserId;
                    responseOut = employeeAssetApplicationBL.AddEditEmployeeAssetApplication(employeeAssetApplicationViewModel); 

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeAssetApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeAssetApplication(int essEmployeeId = 0, string essEmployeeName = "")
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

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
        public PartialViewResult GetEmployeeAssetApplicationList(string applicationNo = "", int employeeId = 0, string assetTypeName = "", string assetStatus = "0", string fromDate = "", string toDate = "", int essEmployeeId = 0, string essEmployeeName = "",int companyBranchId=0)
        {
            List<EmployeeAssetApplicationViewModel> employeeAssetApplicationViewModel = new List<EmployeeAssetApplicationViewModel>();
           
            EmployeeAssetApplicationBL employeeAssetApplicationBL = new EmployeeAssetApplicationBL();
            try
            {
                ViewData["essEmployeeId"] = essEmployeeId;
                ViewData["essEmployeeName"] = essEmployeeName;
                employeeAssetApplicationViewModel = employeeAssetApplicationBL.GetEmployeeAssetApplicationList(applicationNo, essEmployeeId, assetTypeName, assetStatus, fromDate, toDate,ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeAssetApplicationViewModel);
        }





        [HttpGet]
        public JsonResult GetEmployeeAssetApplicationDetail(long applicationId)
        {           
            EmployeeAssetApplicationBL employeeAssetApplicationBL = new EmployeeAssetApplicationBL();
            EmployeeAssetApplicationViewModel employeeAssetApplicationViewModel = new EmployeeAssetApplicationViewModel();

            try
            {
                employeeAssetApplicationViewModel = employeeAssetApplicationBL.GetEmployeeAssetApplicationDetail(applicationId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(employeeAssetApplicationViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeeAssetApplicationTypeList()
        {
            GoalCategoryBL goalCategoryBL = new GoalCategoryBL();
            AssetTypeBL assetTypeBL = new AssetTypeBL();
            List<HR_AssetTypeViewModel> assetTypeViewModel = new List<HR_AssetTypeViewModel>();

            try
            {
                assetTypeViewModel = assetTypeBL.GetEmployeeAssetApplicationTypeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(assetTypeViewModel, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Approval Employee Asset Application

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ApprovalEmployeeAssetApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ApprovalRejectedEmployeeAssetApplication(int applicationId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ApprovalEmployeeAssetApplication, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult ApprovalRejectedEmployeeAssetApplication(EmployeeAssetApplicationViewModel employeeAssetApplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeAssetApplicationBL employeeAssetApplicationBL = new EmployeeAssetApplicationBL();


            try
            {
                if (employeeAssetApplicationViewModel != null)
                {
                    employeeAssetApplicationViewModel.CompanyId = ContextUser.CompanyId;
                    employeeAssetApplicationViewModel.ApproveBy = ContextUser.UserId;
                    employeeAssetApplicationViewModel.RejectBy = ContextUser.UserId;
                    responseOut = employeeAssetApplicationBL.ApprovalRejectedEmployeeAssetApplication(employeeAssetApplicationViewModel);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ApprovalEmployeeAssetApplication, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeAssetApplicationApproval()
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;

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
        public PartialViewResult GetEmployeeAssetApplicationApprovalList(string applicationNo = "", int employeeId = 0, string assetTypeName = "", string assetStatus = "0", string fromDate = "", string toDate = "",int companyBranchId=0)
        {
            List<EmployeeAssetApplicationViewModel> employeeAssetApplicationViewModel = new List<EmployeeAssetApplicationViewModel>();

            EmployeeAssetApplicationBL employeeAssetApplicationBL = new EmployeeAssetApplicationBL();
            try
            {
                employeeAssetApplicationViewModel = employeeAssetApplicationBL.GetEmployeeAssetApplicationApprovalList(applicationNo, employeeId, assetTypeName, assetStatus, fromDate, toDate, ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeAssetApplicationViewModel);
        }

        #endregion



    }
}
