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
    public class PMS_AssessmentController : BaseController
    {
        #region PMS_Assessment Employee Self Assessment
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_EmployeeSelfAssessment, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeSelfAssessment(int empAppraisalTemplateMappingId = 0, int accessMode = 3)
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_EmployeeSelfAssessment, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmployeeSelfAssessment(PMS_EmployeeAppraisalTemplateMappingViewModel employeeAppraisalTemplateMappingViewModel, List<PMS_EmployeeGoalsViewModel> employeeGoals)
        {
            ResponseOut responseOut = new ResponseOut();
            PMS_EmployeeAppraisalTemplateMappingBL empAppraisalTemplateMappingBL = new PMS_EmployeeAppraisalTemplateMappingBL();
            try
            {
                if (employeeAppraisalTemplateMappingViewModel != null)
                {
                    int i = 0;
                    var categoryWeight = employeeGoals.GroupBy(a => a.GoalCategoryId).Select(a => new { key = a.Key, Weight = a.Sum(b => b.Weight) }).OrderByDescending(a => a.Weight).ToList();
                    foreach (var item in categoryWeight)
                    {
                        i++;
                        var key = item.key;
                        var weight = item.Weight;
                        if (weight > 100)
                        {
                            responseOut.message = "Goal Category" + item.key + " " + ActionMessage.GoalCategoryWeight;
                            responseOut.status = ActionStatus.Fail;
                            responseOut.trnId = 0;
                            break;
                        }
                        else
                        {
                            if (i == categoryWeight.Count)
                            {
                                employeeAppraisalTemplateMappingViewModel.CreatedBy = ContextUser.UserId;
                                employeeAppraisalTemplateMappingViewModel.CompanyId = ContextUser.CompanyId;
                                responseOut = empAppraisalTemplateMappingBL.AddEditEmployeeAssessment(employeeAppraisalTemplateMappingViewModel, employeeGoals);
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_EmployeeSelfAssessment, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmployeeSelfAssessment()
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
        public PartialViewResult GetEmployeeSelfAssessmentList(string templateName = "", string employeeName = "",string employeeMapping_Status="1",int companyBranchId=0)
        {
            List<PMS_EmployeeAppraisalTemplateMappingViewModel> appraisalTemplates = new List<PMS_EmployeeAppraisalTemplateMappingViewModel>();
            PMS_EmployeeAppraisalTemplateMappingBL appraisalTemplateBL = new PMS_EmployeeAppraisalTemplateMappingBL();
            try
            {
                appraisalTemplates = appraisalTemplateBL.GetEmployeeAppraisalTemplateMappingList(templateName, employeeName, ContextUser.CompanyId, "1", companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(appraisalTemplates);
        }
        [HttpGet]
        public JsonResult GetEmployeeSelfAssessmentDetail(long empAppraisalTemplateMappingId)
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
        [HttpPost]
        public PartialViewResult GetEmployeeSelfAssessmentGoalList(List<PMS_EmployeeGoalsViewModel> goals, long empAppraisalTemplateMappingId)
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
        #endregion

        #region PMS_Assessment Appraiser Assessment
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_AppraiserAssessment, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult ListAppraiserAssessment()
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
        public PartialViewResult GetAppraiserAssessmentList(string templateName = "", string employeeName = "", string employeeMapping_Status = "1",int companyBranchId=0)
        {
            List<PMS_EmployeeAppraisalTemplateMappingViewModel> appraisalTemplates = new List<PMS_EmployeeAppraisalTemplateMappingViewModel>();
            PMS_EmployeeAppraisalTemplateMappingBL appraisalTemplateBL = new PMS_EmployeeAppraisalTemplateMappingBL();
            try
            {
                appraisalTemplates = appraisalTemplateBL.GetEmployeeAppraisalTemplateMappingList(templateName, employeeName, ContextUser.CompanyId, "1", companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(appraisalTemplates);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_AppraiserAssessment, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditAppraiserAssessment(int empAppraisalTemplateMappingId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_AppraiserAssessment, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditAppraiserAssessment(PMS_EmployeeAppraisalTemplateMappingViewModel employeeAppraisalTemplateMappingViewModel, List<PMS_EmployeeGoalsViewModel> employeeGoals)
        {
            ResponseOut responseOut = new ResponseOut();
            PMS_EmployeeAppraisalTemplateMappingBL empAppraisalTemplateMappingBL = new PMS_EmployeeAppraisalTemplateMappingBL();
            try
            {
                if (employeeAppraisalTemplateMappingViewModel != null)
                {
                    employeeAppraisalTemplateMappingViewModel.CreatedBy = ContextUser.UserId;
                    employeeAppraisalTemplateMappingViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = empAppraisalTemplateMappingBL.AddEditEmployeeAssessment(employeeAppraisalTemplateMappingViewModel, employeeGoals);

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

        [HttpPost]
        public PartialViewResult GetAppraiserAssessmentGoalList(List<PMS_EmployeeGoalsViewModel> goals, long empAppraisalTemplateMappingId)
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

        #endregion
        #region PMS_Assessment Review Assessment
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_FinalAssessment, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListFinalAssessment()
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
        public PartialViewResult GetFinalAssessmentList(string templateName = "", string employeeName = "", string employeeMapping_Status = "1",int companyBranchId=0)
        {
            List<PMS_EmployeeAppraisalTemplateMappingViewModel> appraisalTemplates = new List<PMS_EmployeeAppraisalTemplateMappingViewModel>();
            PMS_EmployeeAppraisalTemplateMappingBL appraisalTemplateBL = new PMS_EmployeeAppraisalTemplateMappingBL();
            try
            {
                appraisalTemplates = appraisalTemplateBL.GetEmployeeAppraisalTemplateMappingList(templateName, employeeName, ContextUser.CompanyId, "1", companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(appraisalTemplates);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_FinalAssessment, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditFinalAssessment(int empAppraisalTemplateMappingId = 0, int accessMode = 3)
        {
            try
            {
                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_FinalAssessment, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditFinalAssessment(PMS_EmployeeAppraisalTemplateMappingViewModel employeeAppraisalTemplateMappingViewModel, List<PMS_EmployeeGoalsViewModel> employeeGoals)
        {
            ResponseOut responseOut = new ResponseOut();
            PMS_EmployeeAppraisalTemplateMappingBL empAppraisalTemplateMappingBL = new PMS_EmployeeAppraisalTemplateMappingBL();
            try
            {
                if (employeeAppraisalTemplateMappingViewModel != null)
                {
                    employeeAppraisalTemplateMappingViewModel.CreatedBy = ContextUser.UserId;
                    employeeAppraisalTemplateMappingViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = empAppraisalTemplateMappingBL.AddEditEmployeeAssessment(employeeAppraisalTemplateMappingViewModel, employeeGoals);

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

        [HttpPost]
        public PartialViewResult GetFinalAssessmentGoalList(List<PMS_EmployeeGoalsViewModel> goals, long empAppraisalTemplateMappingId)
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

        #endregion




    }
}
