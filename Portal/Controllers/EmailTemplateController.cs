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
    public class EmailTemplateController : BaseController
    {
        //
        // GET: /EmailTemplate/
        #region EmailTemplate
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Email_Template, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmailTemplate(int emailTemplateId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (emailTemplateId != 0)
                {
                    ViewData["emailTemplateId"] = emailTemplateId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["termtemplateId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Email_Template, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditEmailTemplate(EmailTemplateViewModel emailTemplateViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            EmailTemplateBL emailTemplateBL = new EmailTemplateBL();

            try
            {
                if (emailTemplateViewModel != null)
                {
                    emailTemplateViewModel.CreatedBy = ContextUser.UserId;
                    emailTemplateViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = emailTemplateBL.AddEditEmailTemplate(emailTemplateViewModel);
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
        public JsonResult GetEmailTemplateTypeList()
        {
            EmailTemplateBL emailTemplateBL = new EmailTemplateBL();
            
            List<EmailTemplateTypeViewModel> emailTemplateList = new List<EmailTemplateTypeViewModel>();
            try
            {
                emailTemplateList = emailTemplateBL.GetEmailTemplateTypeList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(emailTemplateList, JsonRequestBehavior.AllowGet);
        }

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Email_Template, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListEmailTemplate()
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
        public JsonResult GetEmailTemplateDetail(int emailTemplateId)
        {
            EmailTemplateBL emailTemplateBL = new EmailTemplateBL();
            EmailTemplateViewModel emailTemplate = new EmailTemplateViewModel();
            try
            {
                emailTemplate = emailTemplateBL.GetEmailTemplateDetail(emailTemplateId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(emailTemplate, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetEmailTemplateList(string emailTemplateSubject, int emailTemplateTypeId,string status,int companyBranchId)
        {
            List<EmailTemplateViewModel> emailTemplates = new List<EmailTemplateViewModel>();
            EmailTemplateBL emailTemplateBL = new EmailTemplateBL();
            try
            {
               
                    emailTemplates = emailTemplateBL.GetEmailTemplateList(emailTemplateSubject, emailTemplateTypeId,ContextUser.CompanyId,status, companyBranchId);
                

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(emailTemplates);
        }






        #endregion

    }
}
