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
using System.Text;
using System.Data;

namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class EmployeeClearanceProcessController : BaseController
    {
        #region Employee Clearance Process Mapping

        //
        // GET: /EmployeeClearanceProces/
      
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeClearanceProces, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeClearanceProcess(int employeeClearanceId = 0, int accessMode = 3)
        { 
            try
            {

                if (employeeClearanceId != 0)
                {
                    ViewData["employeeClearanceId"] = employeeClearanceId;
                    ViewData["accessMode"] = accessMode;
                    
                }
                else
                {
                    ViewData["employeeClearanceId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeClearanceProces, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeClearanceProcess(EmployeeClearanceProcessViewModel employeeClearanceProcess, List<EmployeeClearanceProcessDetailViewModel> employeeClearanceProcessDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeClearanceProcessBL empClearanceProcessBL = new EmployeeClearanceProcessBL();
            try
            {
                if (employeeClearanceProcess != null)
                {
                    employeeClearanceProcess.CreatedBy = ContextUser.UserId;
                    employeeClearanceProcess.CompanyId = ContextUser.CompanyId;
                    responseOut = empClearanceProcessBL.AddEditEmployeeClearanceProcessMapping(employeeClearanceProcess, employeeClearanceProcessDetails);

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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_EmployeeClearanceProces, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeClearanceProcess()
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

        [HttpPost]
        public PartialViewResult GetClearanceProcessList(long clearancetemplateId)
        {
            List<ClearanceTemplateDetailViewModel> clearaneTemplateDetailList = new List<ClearanceTemplateDetailViewModel>();
            ClearanceTemplateBL clearanceTemplateBL = new ClearanceTemplateBL();
            try
            {
                clearaneTemplateDetailList = clearanceTemplateBL.GetClearanceTemplateDetailList(clearancetemplateId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(clearaneTemplateDetailList);
        }

        [HttpGet]
        public PartialViewResult GetEmployeeClearanceProcessList(string employeeClearanceNo = "", long employeeId = 0, int clearanceTemplateId = 0)
        {
            List<EmployeeClearanceProcessViewModel> clearanceTemplates = new List<EmployeeClearanceProcessViewModel>();
            EmployeeClearanceProcessBL empClearanceProcessBL = new EmployeeClearanceProcessBL();
            try
            {
                clearanceTemplates = empClearanceProcessBL.GetEmployeeClearanceProcessMappingList(employeeClearanceNo, employeeId, clearanceTemplateId,ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(clearanceTemplates);
        }

       
        [HttpGet]
        public JsonResult GetClearanceTemplateList()
        {
            ClearanceTemplateBL clearanceTemplateBL = new ClearanceTemplateBL();
           List<ClearanceTemplateViewModel> clearanceTemplates = new List<ClearanceTemplateViewModel>();
            try
            {
                clearanceTemplates = clearanceTemplateBL.GetClearanceTemplateList(ContextUser.CompanyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(clearanceTemplates, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeeClearanceProcessDetail(long employeeClearanceId)
        {
            EmployeeClearanceProcessBL empClearanceProcessBL = new EmployeeClearanceProcessBL();
            EmployeeClearanceProcessViewModel clearanceTemplate = new EmployeeClearanceProcessViewModel();
            try
            {
                clearanceTemplate = empClearanceProcessBL.GetEmployeeClearanceProcessDetail(employeeClearanceId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(clearanceTemplate, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult GetEmployeeClearanceProcesses(List<EmployeeClearanceProcessDetailViewModel> employeeClearanceProcesses, long employeeClearanceId)
        {
            EmployeeClearanceProcessBL clearanceTemplateBL = new EmployeeClearanceProcessBL();
            try
            {
                if (employeeClearanceProcesses == null)
                {
                    employeeClearanceProcesses = clearanceTemplateBL.GetEmployeeClearanceProcesses(employeeClearanceId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeClearanceProcesses);
        }

        #endregion

        #region Employee Clearance

        [ValidateRequest(true, UserInterfaceHelper.ListEmployeeClearance, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeClearance()
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
        public PartialViewResult GetEmployeeClearanceList(string employeeClearanceNo = "", long employeeId = 0, int clearanceTemplateId = 0)
        {
            List<EmployeeClearanceProcessViewModel> clearanceTemplates = new List<EmployeeClearanceProcessViewModel>();
            EmployeeClearanceProcessBL empClearanceProcessBL = new EmployeeClearanceProcessBL();
            try
            {
                clearanceTemplates = empClearanceProcessBL.GetEmployeeClearanceProcessMappingList(employeeClearanceNo, employeeId, clearanceTemplateId, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(clearanceTemplates);
        }

        [ValidateRequest(true, UserInterfaceHelper.ListEmployeeClearance, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeClearance(int employeeClearanceId = 0, int accessMode = 3)
        {
            try
            {

                if (employeeClearanceId != 0)
                {
                    ViewData["employeeClearanceId"] = employeeClearanceId;
                    ViewData["accessMode"] = accessMode;

                }
                else
                {
                    ViewData["employeeClearanceId"] = 0;
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
        public PartialViewResult GetEmployeeClearances(List<EmployeeClearanceProcessDetailViewModel> employeeClearanceProcesses, long employeeClearanceId)
        {
            EmployeeClearanceProcessBL clearanceTemplateBL = new EmployeeClearanceProcessBL();
            try
            {
                if (employeeClearanceProcesses == null)
                {
                    employeeClearanceProcesses = clearanceTemplateBL.GetEmployeeClearanceProcesses(employeeClearanceId);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeClearanceProcesses);
        }
        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.ListEmployeeClearance, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeClearance(EmployeeClearanceProcessViewModel employeeClearanceProcess, List<EmployeeClearanceProcessDetailViewModel> employeeClearanceProcessDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            EmployeeClearanceProcessBL empClearanceProcessBL = new EmployeeClearanceProcessBL();
            try
            {
                if (employeeClearanceProcess != null)
                {
                    employeeClearanceProcess.CreatedBy = ContextUser.UserId;
                    employeeClearanceProcess.CompanyId = ContextUser.CompanyId;
                    responseOut = empClearanceProcessBL.AddEditEmployeeClearance(employeeClearanceProcess, employeeClearanceProcessDetails);

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
