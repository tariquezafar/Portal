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
    public class RoleController : BaseController
    {
        //
        // GET: /Role/
        #region Role
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Role_CP, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditRole(int roleId = 0, int accessMode = 3)
        {

            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
                if (roleId != 0)
                {
                    ViewData["roleId"] = roleId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["roleId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Role_CP, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditRole(RoleViewModel roleViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            RoleBL roleBL = new RoleBL();
            try
            {
                if (roleViewModel != null)
                {
                    roleViewModel.CompanyId = ContextUser.CompanyId;
                    responseOut = roleBL.AddEditRole(roleViewModel);

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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Role_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListRole()
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
        public PartialViewResult GetRoleList(string roleName = "", string roleDesc = "", string isAdmin = "", string Status = "",int companyBranchId=0)
        {
            List<RoleViewModel> roles = new List<RoleViewModel>();
            RoleBL roleBL = new RoleBL();
            try
            {
                roles = roleBL.GetRoleList(roleName, roleDesc, isAdmin, Status, ContextUser.CompanyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(roles);
        }

        [HttpGet]
        public JsonResult GetRoleDetail(int roleId)
        {
            RoleBL roleBL = new RoleBL();
            RoleViewModel role = new RoleViewModel();
            try
            {
                role = roleBL.GetRoleDetail(roleId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(role, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Role UI Mapping
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Role_UI_Mapping_Admin, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditRoleUIMapping(int accessMode = 3)
        {

            try
            {
                ViewData["accessMode"] = accessMode;
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }

        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_Role_UI_Mapping_Admin, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditRoleUIMapping(List<RoleUIMappingViewModel> roleUIMappingList)
        {
            ResponseOut responseOut = new ResponseOut();
            RoleUIMappingBL roleUIMappingBL = new RoleUIMappingBL();
            try
            {
                if (roleUIMappingList != null && roleUIMappingList.Count>0)
                {
                 responseOut = roleUIMappingBL.AddEditRoleUIMapping(roleUIMappingList);
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
        public PartialViewResult GetRoleUIMapping(string interfaceType = "", string interfaceSubType = "", int roleId = 0)
        {
            List<RoleUIMappingViewModel> roles = new List<RoleUIMappingViewModel>();
            RoleUIMappingBL roleBL = new RoleUIMappingBL();
            try
            {
                roles = roleBL.GetRoleUIActionMappingDetail(interfaceType,interfaceSubType,roleId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(roles);
        }


        [HttpGet]
        public JsonResult CheckMasterPermission(int roleId = 1, int interfaceId = 1, int accessMode = 1)
        {
            RoleUIMappingBL roleBL = new RoleUIMappingBL();
            bool isAuthorized = false;
            try
            {
                isAuthorized = roleBL.CheckMasterPermission(roleId, interfaceId, accessMode);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(isAuthorized, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}
