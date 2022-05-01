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
    public class LanguageController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Language, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditLanguage(int languageId = 0, int accessMode = 3)
        {

            try
            {
                if (languageId != 0)
                {
                    ViewData["languageId"] = languageId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["languageId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Language, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditLanguage(HR_LanguageViewModel languageViewModel)
        {
            ResponseOut responseOut = new ResponseOut();                             
            LanguageBL languageBL = new LanguageBL();
            try
            {
                if (languageViewModel != null)
                {
                    responseOut = languageBL.AddEditLanguage(languageViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Language, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListLanguage()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult GetLanguageList(string languageName = "", string languageStatus = "")
        {
            List<HR_LanguageViewModel> languageViewModel = new List<HR_LanguageViewModel>();

            LanguageBL languageBL = new LanguageBL();
            try
            {
                languageViewModel = languageBL.GetLanguageList(languageName, languageStatus);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(languageViewModel);
        }     
        [HttpGet]
        public JsonResult GetLanguageDetail(int languageId)
        {
       
            LanguageBL languageBL = new LanguageBL();
            HR_LanguageViewModel hR_LanguageViewModel = new HR_LanguageViewModel();
         
            try
            {
                hR_LanguageViewModel = languageBL.GetLanguageDetail(languageId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(hR_LanguageViewModel, JsonRequestBehavior.AllowGet);
        }

    }
}
