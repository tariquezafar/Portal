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
    public class FormTypeController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_FormType_ADMIN, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditFormType(int formTypeId = 0, int accessMode = 3)
        {

            try
            {
                if (formTypeId != 0)
                {
                    ViewData["FormTypeId"] = formTypeId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["FormTypeId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_FormType_ADMIN, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditFormType(FormTypeViewModel formTypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();          
            FormTypeBL formTypeBL = new FormTypeBL();
            try
            {
                if (formTypeViewModel != null)
                {                  
                    formTypeViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = formTypeBL.AddEditFormType(formTypeViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_FormType_ADMIN, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListFormType()
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
        public PartialViewResult GetFormTypeList(string formTypeDesc = "", string status = "")
        {
            List<FormTypeViewModel> formTypeViewModel = new List<FormTypeViewModel>();
           
            FormTypeBL formTypeBL = new FormTypeBL();
            try
            {
                formTypeViewModel = formTypeBL.GetFormTypeList(formTypeDesc, ContextUser.CompanyId, status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(formTypeViewModel);
        }


        [HttpGet]
        public JsonResult GetFormTypeDetail(int formTypeId)
        {
            FormTypeBL formTypeBL = new FormTypeBL();
            FormTypeViewModel formTypeViewModel = new FormTypeViewModel();         
            try
            {
                formTypeViewModel = formTypeBL.GetScheduleDetail(formTypeId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(formTypeViewModel, JsonRequestBehavior.AllowGet);
        }

    }
}
