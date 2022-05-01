using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.IO;
namespace Portal.Controllers
{
    [CheckSessionBeforeControllerExecuteAttribute(Order = 1)]
    public class UserController : BaseController
    {
        //
        // GET: /User/
        #region Primary User
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_User_CP, (int)AccessMode.AddAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditPrimaryUser(int userId = 0, int accessMode = 3)
        {

            try
            {
                if (userId != 0)
                {
                    ViewData["userId"] = userId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["userId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_User_CP, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditPrimaryUser(UserViewModel userViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            UserBL userBL = new UserBL();
            try
            {
                if (userViewModel != null)
                {
                    userViewModel.RoleId = (int)Roles.Admin;
                    userViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = userBL.AddEditUser(userViewModel);
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_User_CP, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListPrimaryUser()
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
        public JsonResult GetCompanyList()
        {
            CompanyBL companyBL = new CompanyBL();
            List<CompanyViewModel> companyList = new List<CompanyViewModel>();
            try
            {
                companyList = companyBL.GetCompanyList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(companyList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetPrimaryUserList(string userName = "", string phoneNo = "", string fullName = "", string email = "", int companyId = 0)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            UserBL userBL = new UserBL();
            try
            {
                users = userBL.GetUserList(userName,companyId,(int)Roles.Admin,fullName,phoneNo,email, ContextUser.RoleId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(users);
        }

        [HttpGet]
        public JsonResult GetUserDetail(int userId)
        {
            UserBL userBL = new UserBL();
            UserViewModel user = new UserViewModel();
            try
            {
                user = userBL.GetUserDetail(userId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetDashboardUser()
        {
            List<UserViewModel> users = new List<UserViewModel>();
            UserBL userBL = new UserBL();
            try
            {
                users = userBL.GetDashboardUser();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(users);
        }
        #endregion

        #region Normal User
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_User_ADMIN, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult AddEditUser(int userId = 0, int accessMode = 3)
        {
            try
            {
                ViewData["CompanyBranchId"] = Session[SessionKey.CompanyBranchId] != null ? ((UserViewModel)Session[SessionKey.CompanyBranchId]).CompanyBranchId : 0;
                ViewData["UserIdCom"] = Session[SessionKey.UserId] != null ? ((UserViewModel)Session[SessionKey.UserId]).UserId : 0;

                if (userId != 0)
                {
                    ViewData["userId"] = userId;
                    ViewData["accessMode"] = accessMode;
                }
                else
                {
                    ViewData["userId"] = 0;
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
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_User_ADMIN, (int)AccessMode.AddAccess, (int)RequestMode.Ajax)]
        public ActionResult AddEditUser(UserViewModel userViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            UserBL userBL = new UserBL();
            try
            {
                if (userViewModel != null)
                {
                    userViewModel.CompanyId = ContextUser.CompanyId;
                    userViewModel.CreatedBy = ContextUser.UserId;
                    responseOut = userBL.AddEditUser(userViewModel);
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

        [HttpPost]
        public ActionResult UpdateUserPic()
        {
            ResponseOut responseOut = new ResponseOut();
            UserBL userBL = new UserBL();
            HttpFileCollectionBase files = Request.Files;
            UserViewModel userViewModel = new UserViewModel();
            try
            {
               userViewModel.UserId= Convert.ToInt32(Request["userId"]);
                //  Get all files from Request object  
                if (files != null && files.Count > 0 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    HttpPostedFileBase file = files[0];
                    string fname;
                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        userViewModel.UserPicName = userViewModel.UserId.ToString()+ "_" + fileName ;
                        var path = Path.Combine(Server.MapPath("~/Images/UserImages"), userViewModel.UserPicName);
                        file.SaveAs(path);
                        userViewModel.UserPicPath = path;

                        //queryDetail.QueryAttachment = new byte[file.ContentLength];
                        //file.InputStream.Read(queryDetail.QueryAttachment, 0, file.ContentLength);
                    }
                }

                if (userViewModel != null && !string.IsNullOrEmpty(userViewModel.UserPicPath) )
                {
                    responseOut = userBL.UpdateUserPic(userViewModel);
                }
                else
                {
                    responseOut.message = "";
                    responseOut.status = ActionStatus.Success;
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

        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_User_ADMIN, (int)AccessMode.ViewAccess, (int)RequestMode.GetPost)]
        public ActionResult ListUser()
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
        public PartialViewResult GetUserList(string userName = "", string phoneNo = "", string fullName = "", string email = "", int roleId = 0,int companyBranchId=0)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            UserBL userBL = new UserBL();
            try
            {
                users = userBL.GetUserList(userName, ContextUser.CompanyId , roleId, fullName, phoneNo, email, ContextUser.RoleId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return PartialView(users);
        }

        [HttpGet]
        public JsonResult GetRoleList()
        {
            RoleBL roleBL = new RoleBL();
            List<RoleViewModel> roleList = new List<RoleViewModel>();
            try
            {
                roleList = roleBL.GetRoleList(ContextUser.CompanyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(roleList, JsonRequestBehavior.AllowGet);
        }
          
        [HttpPost]
        [ValidateRequest(true, UserInterfaceHelper.Add_Edit_User_ADMIN, (int)AccessMode.EditAccess, (int)RequestMode.Ajax)]
        public JsonResult RemoveImage(long userId)
        {
            ResponseOut responseOut = new ResponseOut();
            UserBL userBL = new UserBL();
            try
            {
                if (userId != null)
                {
                    responseOut = userBL.RemoveImage(userId);
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




        #endregion
    }
}
