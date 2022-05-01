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
    public class GLSubGroupController : BaseController
    {
        //
        // GET: /Company/
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_GLSubGroup_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditGLSubGroup(int glsubgroupId = 0, int accessMode = 3)
        {

            try
            {
                if (glsubgroupId != 0)
                {
                    ViewData["glsubgroupId"] = glsubgroupId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["glsubgroupId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_GLSubGroup_ACCOUNT, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditGLSubGroup(GLSubGroupViewModel glsubgroupViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            GLSubGroupBL glsubgroupBL = new GLSubGroupBL();
            try
            {
                if (glsubgroupViewModel != null)
                {
                    glsubgroupViewModel.CreatedBy = ContextUser.UserId;
                    glsubgroupViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = glsubgroupBL.AddEditGLSubGroup(glsubgroupViewModel);
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


        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_GLSubGroup_ACCOUNT, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListGLSubGroup()
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
        public PartialViewResult GetGLSubGroupList(string glsubgroupName = "", int scheduleId = 0, string glmaingroupId = "", int sequenceNo = 0, string status = "")
        {
            List<GLSubGroupViewModel> glsubgroup = new List<GLSubGroupViewModel>();
            GLSubGroupBL glsubgroupBL = new GLSubGroupBL();
            try
            {
                glsubgroup = glsubgroupBL.GetGLSubGroupList(glsubgroupName, scheduleId, glmaingroupId, sequenceNo, ContextUser.CompanyId, status);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(glsubgroup);
        }


        [HttpGet]
        public JsonResult GetGLSubGroupDetail(int glsubgroupId)
        {
            GLSubGroupBL glsubgroupBL = new GLSubGroupBL();
            GLSubGroupViewModel glsubgroup = new GLSubGroupViewModel();
            try
            {
                glsubgroup = glsubgroupBL.GetGLSubGroupDetail(glsubgroupId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(glsubgroup, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetGLMainGroupList()
        {
            GLSubGroupBL glsubgroupBL = new GLSubGroupBL();
            List<GLMainGroupViewModel> glmaingroupList = new List<GLMainGroupViewModel>();
            try
            {
                glmaingroupList = glsubgroupBL.GetGLMainGroupList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(glmaingroupList, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetGLScheduleList()
        {
            ScheduleBL scheduleBL = new ScheduleBL();
            List<ScheduleViewModel> scheduleViewModelList = new List<ScheduleViewModel>();
            
            try
            {
                scheduleViewModelList = scheduleBL.GetGLScheduleList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(scheduleViewModelList, JsonRequestBehavior.AllowGet);
        }
         


        [HttpGet]
        public JsonResult GetGLMainGroupTypeList(string glType)
        {
            GLSubGroupBL glsubgroupBL = new GLSubGroupBL();
            List<GLMainGroupViewModel> glmaingroupList = new List<GLMainGroupViewModel>();
            try
            {
                glmaingroupList = glsubgroupBL.GetGLMainGroupTypeList(glType);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(glmaingroupList, JsonRequestBehavior.AllowGet);
        }

    }
}
