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
    public class ClearanceTemplateController : BaseController
    {
        #region ClearanceTemplate

        //
        // GET: /ClearanceTemplate/
        public ActionResult Index()
        {
            return View();
        }
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ClearanceTemplate, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditClearanceTemplate(int clearancetemplateId = 0, int accessMode = 3)
        {

            try
            {

                FinYearViewModel finYear = Session[SessionKey.CurrentFinYear] != null ? (FinYearViewModel)Session[SessionKey.CurrentFinYear] : new FinYearViewModel();

                ViewData["fromDate"] = finYear.StartDate;
                ViewData["toDate"] = finYear.EndDate;
                if (clearancetemplateId != 0)
                {
                    ViewData["clearancetemplateId"] = clearancetemplateId;
                    ViewData["accessMode"] = accessMode;
                    ViewData["currentDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["clearancetemplateId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ClearanceTemplate, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditClearanceTemplate(ClearanceTemplateViewModel clearancetemplateViewModel, List<ClearanceTemplateDetailViewModel> templateDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            ClearanceTemplateBL clearancetemplateBL = new ClearanceTemplateBL();
            try
            {
                if (clearancetemplateViewModel != null)
                {
                    clearancetemplateViewModel.CreatedBy = ContextUser.UserId;
                    clearancetemplateViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = clearancetemplateBL.AddEditClearanceTemplate(clearancetemplateViewModel, templateDetails);

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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_ClearanceTemplate, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListClearanceTemplate()
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
        public PartialViewResult GetClearanceTemplateLists(string clearancetemplateName = "", int department = 0, int designation = 0, int separationCategory = 0, string clearancetemplateStatus = "")
        {
            List<ClearanceTemplateViewModel> clearanceTemplates = new List<ClearanceTemplateViewModel>();
            ClearanceTemplateBL clearancetemplateBL = new ClearanceTemplateBL();
            try
            {
                clearanceTemplates = clearancetemplateBL.GetClearanceTemplateLists(clearancetemplateName, department, designation, separationCategory, ContextUser.CompanyId, clearancetemplateStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(clearanceTemplates);
        } 
         


        [HttpGet]
        public JsonResult GetClearanceTemplateDetail(long clearancetemplateId)
        { 
           ClearanceTemplateBL clearanceTemplateBL = new ClearanceTemplateBL();
           ClearanceTemplateViewModel clearanceTemplate = new ClearanceTemplateViewModel();
            try
            {
                clearanceTemplate = clearanceTemplateBL.GetClearanceTemplateDetail(clearancetemplateId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(clearanceTemplate, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public PartialViewResult GetClearanceTemplateDetailList(List<ClearanceTemplateDetailViewModel> details, long clearancetemplateId)
        {
           ClearanceTemplateBL clearanceTemplateBL = new ClearanceTemplateBL();
            try
            {
                if (details == null)
                {
                    details = clearanceTemplateBL.GetClearanceTemplateDetailList(clearancetemplateId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(details);
        }
     



        [HttpGet]
        public JsonResult GetSeparationClearListForClearanceTemplate()
        {
            SeparationClearListBL separationClearBL = new SeparationClearListBL();
            List<SeparationClearListViewModel> separationclearlistViewModel = new List<SeparationClearListViewModel>();

            try
            {
                separationclearlistViewModel = separationClearBL.GetSeparationClearListForClearanceTemplate();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(separationclearlistViewModel, JsonRequestBehavior.AllowGet);
        }


        
        #endregion




    }
}
