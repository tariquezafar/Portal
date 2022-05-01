using Portal.Common;
using Portal.Core;
using Portal.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class UserEmailSettingController : BaseController
    {
        #region UserEmailSetting
        [ValidateRequest(true, UserInterfaceHelper.AddEdit_UserEmailSetting, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditUserEmailSetting(int userEmailSettingId = 0, int accessMode = 3)
        {
          try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserId"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (userEmailSettingId != 0)
                {
                    ViewData["userEmailSettingId"] = userEmailSettingId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["userEmailSettingId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.AddEdit_UserEmailSetting, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditUserEmailSetting(UserEmailSettingViewModel userEmailSettingViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            UserEmailSettingBL userEmailSettingBL = new UserEmailSettingBL();
            try
            {
                if (userEmailSettingViewModel != null)
                {
                    userEmailSettingViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = userEmailSettingBL.AddEditUserEmailSetting(userEmailSettingViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.AddEdit_UserEmailSetting, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListUserEmailSetting()
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
        public PartialViewResult GetUserEmailSettingList(string fullName="",string userName = "",int companyBranchId=0)
        {
            List<UserEmailSettingViewModel> users = new List<UserEmailSettingViewModel>();
            UserEmailSettingBL userEmailSettingBL = new UserEmailSettingBL();
            try
            {
                users = userEmailSettingBL.GetUserEmailSettingList(fullName, userName, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(users);
        }

        [HttpGet]
        public JsonResult GetUserEmailSettingDetail(int settingId)
        {
            UserEmailSettingBL userEmailSettingBL = new UserEmailSettingBL();
            UserEmailSettingViewModel userEmailSetting = new UserEmailSettingViewModel();
            try
            {
                userEmailSetting = userEmailSettingBL.GetUserEmailSettingDetail(settingId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(userEmailSetting, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetUserEmailAutoCompleteList(string term)
        {
            UserEmailSettingBL userEmailSettingBL = new UserEmailSettingBL();
            List<UserViewModel> userList = new List<UserViewModel>();
            try
            {
                userList = userEmailSettingBL.GetUserEmailAutoCompleteList(term);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(userList, JsonRequestBehavior.AllowGet);
        }

#endregion
    }
}
