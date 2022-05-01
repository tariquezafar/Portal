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
    public class GLMainGroupController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_GLMainGroup_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditGLMainGroup(int glmaingroupId = 0, int accessMode = 3)
        {

            try
            {
                if (glmaingroupId != 0)
                {
                    ViewData["glmaingroupId"] = glmaingroupId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["glmaingroupId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_GLMainGroup_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditGLMainGroup(GLMainGroupViewModel glmaingroupViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            GLMainGroupBL glmaingroupBL = new GLMainGroupBL();
            try
            {
                if (glmaingroupViewModel != null)
                {
                    glmaingroupViewModel.CreatedBy = ContextUser.UserId;
                    glmaingroupViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = glmaingroupBL.AddEditGLMainGroup(glmaingroupViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_GLMainGroup_ACCOUNT, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListGLMainGroup()
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
        public PartialViewResult GetGLMainGroupList(string glmaingroupName = "", string glType = "", int sequenceNo = 0, string status = "")
        {
            List<GLMainGroupViewModel> glmaingroup = new List<GLMainGroupViewModel>();
            GLMainGroupBL glmaingroupBL = new GLMainGroupBL();
            try
            {
                glmaingroup = glmaingroupBL.GetGLMainGroupList(glmaingroupName, glType,  sequenceNo, ContextUser.CompanyId, status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(glmaingroup);
        }


        [HttpGet]
        public JsonResult GetGLMainGroupDetail(int glmaingroupId)
        {
            GLMainGroupBL glmaingroupBL = new GLMainGroupBL();
            GLMainGroupViewModel glmaingroup = new GLMainGroupViewModel();
            try
            {
                glmaingroup = glmaingroupBL.GetGLMainGroupDetail(glmaingroupId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(glmaingroup, JsonRequestBehavior.AllowGet);
        }

 
         
    }
}
