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
    public class TermsTemplateController : BaseController
    {
        //
        // GET: /Customer/
        #region Customer
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_TermTemplate_Admin, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditTermTemplate(int termtemplateId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (termtemplateId != 0)
                {
                    ViewData["termtemplateId"] = termtemplateId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["termtemplateId"] = 0;
                    ViewData["accessMode"] = 0;
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_TermTemplate_Admin, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditTermTemplate(TermTemplateViewModel termtemplateViewModel,  List<TermTemplateDetailViewModel> termtemplateDetail)
        {
            ResponseOut responseOut = new ResponseOut();
            TermTemplateBL termtemplateBL = new TermTemplateBL();
            try
            {
                if (termtemplateViewModel != null)
                {
                    termtemplateViewModel.CreatedBy = ContextUser.UserId;
                    termtemplateViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = termtemplateBL.AddEditTermTemplate(termtemplateViewModel, termtemplateDetail);
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_TermTemplate_Admin, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListTermTemplate()
        {
            try
            {
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
        public PartialViewResult GetTermTemplateList(string termtemplateName = "",  string status = "",int companyBranchId=0)
        {
            List<TermTemplateViewModel> termtemplates = new List<TermTemplateViewModel>();
            TermTemplateBL termtemplateBL = new TermTemplateBL();
            try
            {
                termtemplates = termtemplateBL.GetTermTemplateList(termtemplateName, ContextUser.CompanyId, status, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(termtemplates);
        }


        [HttpGet]
        public JsonResult GetTermTemplateDetail(int termtemplateId)
        {
            TermTemplateBL termtemplateBL = new TermTemplateBL();
            TermTemplateViewModel termtemplate = new TermTemplateViewModel();
            try
            {
                termtemplate = termtemplateBL.GetTermTemplateDetail(termtemplateId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(termtemplate, JsonRequestBehavior.AllowGet);
        }





        [HttpPost]
        public PartialViewResult GetTermTemplateDetailList(List<TermTemplateDetailViewModel> termTemplates, int termtemplateId)
        {
            TermTemplateBL termtemplateBL = new TermTemplateBL();
            try
            {
                if (termTemplates == null)
                {
                    termTemplates = termtemplateBL.GetTermTemplateDetailLists(termtemplateId);
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(termTemplates);
        }



        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_TermTemplate_Admin, (int)AccessMode.EditAccess, (int)RequestMode.Ajax)]
        public ActionResult RemoveTermTemplate(long trnId = 0)
        {
            ResponseOut responseOut = new ResponseOut();
            TermTemplateBL termtemplateBL = new TermTemplateBL();
            try
            {
                responseOut = termtemplateBL.RemoveTermTemplate(trnId);
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
