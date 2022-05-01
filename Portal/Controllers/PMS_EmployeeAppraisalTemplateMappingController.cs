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
    public class PMS_EmployeeAppraisalTemplateMappingController : BaseController
    {
        #region PMS_EmployeeAppraisalTemplateMapping

       
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_EmployeeAppraisalTemplateMapping, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeAppraisalTemplateMapping(int empAppraisalTemplateMappingId = 0, int accessMode = 3)
        { 
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;

                if (empAppraisalTemplateMappingId != 0)
                {
                    ViewData["empAppraisalTemplateMappingId"] = empAppraisalTemplateMappingId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["empAppraisalTemplateMappingId"] = 0;
                    ViewData["accessMode"] = 3;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                } 

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_EmployeeAppraisalTemplateMapping, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeAppraisalTeamplateMapping(PMS_EmployeeAppraisalTemplateMappingViewModel employeeAppraisalTemplateMappingViewModel, List<PMS_EmployeeGoalsViewModel> employeeGoals)
        {
            ResponseOut responseOut = new ResponseOut();
            PMS_EmployeeAppraisalTemplateMappingBL empAppraisalTemplateMappingBL = new PMS_EmployeeAppraisalTemplateMappingBL();
            try
            {               
                if (employeeAppraisalTemplateMappingViewModel != null)
                {

                    int i = 0;
                    var categoryWeight = employeeGoals.GroupBy(a => a.GoalCategoryId).Select(a => new { key = a.Key, Weight = a.Sum(b => b.Weight) }).OrderByDescending(a=>a.Weight).ToList();
                    foreach (var item in categoryWeight)
                    {
                        i++;
                        var key = item.key;
                        var weight = item.Weight;
                        if(weight>100)
                        {
                            responseOut.message ="Goal Category  "+ item.key+"  .."+ ActionMessage.GoalCategoryWeight;
                            responseOut.status = ActionStatus.Fail;                          
                            responseOut.trnId = 0;
                            break;
                        }
                        else
                        {
                            if(i==categoryWeight.Count)
                            {
                                employeeAppraisalTemplateMappingViewModel.CreatedBy = ContextUser.UserId;
                                employeeAppraisalTemplateMappingViewModel.CompanyId = ContextUser.CompanyId;
                                responseOut = empAppraisalTemplateMappingBL.AddEditEmployeeAppraisalTemplateMapping(employeeAppraisalTemplateMappingViewModel, employeeGoals);
                            }
                            
                           
                        }                                                                                           
                    }                   
                   

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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_EmployeeAppraisalTemplateMapping, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeAppraisalTeamplateMapping()
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
        public PartialViewResult GetEmployeeAppraisalTemplateMappingList(string templateName = "", string employeeName = "",string employeeMapping_Status="",int companyBranchId =0)
        {
            List<PMS_EmployeeAppraisalTemplateMappingViewModel> appraisalTemplates = new List<PMS_EmployeeAppraisalTemplateMappingViewModel>();
            PMS_EmployeeAppraisalTemplateMappingBL appraisalTemplateBL = new PMS_EmployeeAppraisalTemplateMappingBL();
            try
            {
                appraisalTemplates = appraisalTemplateBL.GetEmployeeAppraisalTemplateMappingList(templateName, employeeName, ContextUser.CompanyId, employeeMapping_Status, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(appraisalTemplates);
        }


        [HttpGet]
        public JsonResult GetEmployeeAppraisalTemplateMappingDetail(long empAppraisalTemplateMappingId)
        {

            PMS_EmployeeAppraisalTemplateMappingBL appraisalTemplateBL = new PMS_EmployeeAppraisalTemplateMappingBL();
            PMS_EmployeeAppraisalTemplateMappingViewModel appraisalTemplate = new PMS_EmployeeAppraisalTemplateMappingViewModel();
            try
            {
                appraisalTemplate = appraisalTemplateBL.GetEmployeeAppraisalTemplateMappingDetail(empAppraisalTemplateMappingId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(appraisalTemplate, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public PartialViewResult GetAppraisalTemplateDetailList(string templateName = "", int department = 0, int designation = 0)
        {
            List<PMS_AppraisalTemplateViewModel> appraisalTemplates = new List<PMS_AppraisalTemplateViewModel>();
            PMS_AppraisalTemplateBL appraisalTemplateBL = new PMS_AppraisalTemplateBL();
            try
            {
                appraisalTemplates = appraisalTemplateBL.GetAppraisalTemplateDetailList(templateName, department, designation, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(appraisalTemplates);
        }


        [HttpGet]
        public JsonResult GetFinancialYearForEmployeeAppraisalTemplateList()
        {
            FinYearBL finyearBL = new FinYearBL(); 
            List<FinYearViewModel> finyearViewModel = new List<FinYearViewModel>(); 
            try
            {
                finyearViewModel = finyearBL.GetFinancialYearForEmployeeAppraisalTemplateList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(finyearViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetAppraisalTemplateGoalDetailList(long templateId)
        {
            List<PMS_EmployeeGoalsViewModel> employeeGoals = new List<PMS_EmployeeGoalsViewModel>();
            PMS_EmployeeAppraisalTemplateMappingBL employeeTemplateMappingBL = new PMS_EmployeeAppraisalTemplateMappingBL();
            try
            {
                employeeGoals = employeeTemplateMappingBL.GetAppraisalTemplateGoalDetailList(templateId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(employeeGoals);
        }


        [HttpPost]
        public PartialViewResult GetEmployeeGoalList(List<PMS_EmployeeGoalsViewModel> goals, long empAppraisalTemplateMappingId)
        {
            PMS_EmployeeAppraisalTemplateMappingBL employeeTemplateMappingBL = new PMS_EmployeeAppraisalTemplateMappingBL();
            try
            {
                if (goals == null)
                {
                    goals = employeeTemplateMappingBL.GetEmployeeGoalList(empAppraisalTemplateMappingId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(goals);
        }


        [HttpGet]
        public JsonResult GetGoalAutoCompleteList(string term)
        {           
            GoalBL goalBL = new GoalBL();

            List<PMSGoalViewModel> pMSGoalViewModelList = new List<PMSGoalViewModel>();
            try
            {
                pMSGoalViewModelList = goalBL.GetGoalAutoCompleteList(term, ContextUser.CompanyId);

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(pMSGoalViewModelList, JsonRequestBehavior.AllowGet);
        }

        #endregion




    }
}
