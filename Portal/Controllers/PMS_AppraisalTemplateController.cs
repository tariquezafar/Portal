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
    public class PMS_AppraisalTemplateController : BaseController
    {
        #region PMS_AppraisalTemplate

        //
        // GET: /PMS_AppraisalTemplate/
        public ActionResult Index()
        {
            return View();
        }
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_AppraisalTemplate, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditAppraisalTemplate(int templateId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                if (templateId != 0)
                {
                    ViewData["templateId"] = templateId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["templateId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_AppraisalTemplate, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditAppraisalTemplate(PMS_AppraisalTemplateViewModel templateViewModel,  List<PMS_AppraisalTemplateGoalViewModel> templateGoals)
        {
            ResponseOut responseOut = new ResponseOut();
            PMS_AppraisalTemplateBL appraisalTemplateBL = new PMS_AppraisalTemplateBL();
            try
            {
                if (templateViewModel != null)
                {
                    templateViewModel.CreatedBy = ContextUser.UserId;
                    templateViewModel.CompanyId = ContextUser.CompanyId; 
                    responseOut = appraisalTemplateBL.AddEditAppraisalTemplate(templateViewModel, templateGoals);

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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_PMS_AppraisalTemplate, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListAppraisalTemplate()
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
        public PartialViewResult GetAppraisalTemplateLists(string templateName = "", int department = 0, int designation = 0,  string appraisaltemplateStatus = "",int companyBranchId=0)
        {
            List<PMS_AppraisalTemplateViewModel> appraisalTemplates = new List<PMS_AppraisalTemplateViewModel>();
            PMS_AppraisalTemplateBL appraisalTemplateBL = new PMS_AppraisalTemplateBL();
            try
            {
                appraisalTemplates = appraisalTemplateBL.GetAppraisalTemplateLists(templateName, department, designation, ContextUser.CompanyId, appraisaltemplateStatus,companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(appraisalTemplates);
        }





        [HttpPost]
        public PartialViewResult GetAppraisalTemplateGoalList(List<PMS_AppraisalTemplateGoalViewModel> templateGoals, long templateId)
        {
            PMS_AppraisalTemplateBL appraisalTemplateBL = new PMS_AppraisalTemplateBL();
            try
            {
                if (templateGoals == null)
                {
                    templateGoals = appraisalTemplateBL.GetAppraisalTemplateGoalList(templateId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(templateGoals);
        }

        [HttpGet]
        public PartialViewResult GetTemplateGoalList(string goalName = "", int sectionId = 0, int goalCategoryId = 0, int performanceCycleId = 0, string fromDate = "", string toDate = "")
        {
            List<PMSGoalViewModel> goalList = new List<PMSGoalViewModel>();
            GoalBL goalBL = new GoalBL();
            try
            {
                goalList = goalBL.GetTemplateGoalList(goalName, sectionId, goalCategoryId, performanceCycleId, fromDate, toDate, ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(goalList);
        }

        [HttpGet]
        public JsonResult GetAppraisalTemplateDetail(long templateId)
        {

            PMS_AppraisalTemplateBL appraisalTemplateBL = new PMS_AppraisalTemplateBL();
            PMS_AppraisalTemplateViewModel appraisalTemplate = new PMS_AppraisalTemplateViewModel();
            try
            {
                appraisalTemplate = appraisalTemplateBL.GetAppraisalTemplateDetail(templateId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(appraisalTemplate, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public PartialViewResult GetGoalTemplateList(List<PMS_AppraisalTemplateGoalViewModel> goals, long templateId)
        //{
        //    PMS_AppraisalTemplateBL appraisalTemplateBL = new PMS_AppraisalTemplateBL();
        //    try
        //    {
        //        if (goals == null)
        //        {
        //            goals = appraisalTemplateBL.GetGoalTemplateList(templateId);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //    }
        //    return PartialView(goals);
        //}






        #endregion




    }
}
