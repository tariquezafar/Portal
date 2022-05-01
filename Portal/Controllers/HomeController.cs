using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Core;
using Portal.Core.ViewModel;
using Portal.Common;
using System.Reflection;
using System.Text;
using System.Data;

namespace Portal.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            try
            {
                return View();
            }
            catch(Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                return View();
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string userName, string password)
        {
            UserBL userBL = new UserBL();
            UserViewModel userViewModel = new UserViewModel();

            try
            {
                //var encryptPassword= CommonHelper.EncryptString(password, "secure");
                //var decryptPassword = CommonHelper.DecryptString(encryptPassword, "secure");

                userViewModel = userBL.AuthenticateUser(userName, password);
                Session["Test"] = "Test Value";
            }
            catch (Exception ex)
            {
                userViewModel.status = ActionStatus.Fail;
                userViewModel.message = ActionMessage.ApplicationException;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(userViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetEmployeeIdByEmailId(string emailId)
        {
            EmployeeBL employeeBL = new EmployeeBL();
            EmployeeViewModel empViewModel = new EmployeeViewModel();

            try
            {
                //var encryptPassword= CommonHelper.EncryptString(password, "secure");
                //var decryptPassword = CommonHelper.DecryptString(encryptPassword, "secure");

                empViewModel.EmployeeId = employeeBL.GetEmployeeIdByEmailId(emailId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(empViewModel, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult SendPasswordMail(string userEmail)
        {
            ResponseOut responseOut = new ResponseOut(); 
            UserBL useBL = new UserBL(); 
            try
            {
                UserViewModel userViewModel = new UserViewModel();
                userViewModel = useBL.GetUserFromEmail(userEmail); 

                if (!string.IsNullOrEmpty(userViewModel.FullName))
                {
                    StringBuilder mailBody = new StringBuilder(" ");
                    SendMail sendMail = new SendMail();
                    mailBody.Append("<html><head></head><body>");
                    //mailBody.Append("<img src='" + Convert.ToString(ConfigurationManager.AppSettings["Logo_Path"]) + "' alt='ICS Logo' />");
                    //mailBody.Append("<hr/><br/>");
                    mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Dear " + userViewModel.FullName + " </p><br/>");
                    mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Please find below password</p><br/>");
                    mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Password: <b>"+ EnDecrypt.Decrypt(userViewModel.Password) +"</b></p><br/>");
                    mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Regards,</p><br/>");
                    mailBody.Append("<p style='font-family:Cambria;font-size:12px;margin: 0px 0px 0px 0px;'>Support Team</p><br/>");
                    mailBody.Append("</body></html>"); 

                    bool sendMailStatus = sendMail.SendEmail("", userEmail, "Password Recovery Mail", mailBody.ToString());
                    if (sendMailStatus)
                    {
                        responseOut.message = "Mail Sent Successfully";
                        responseOut.status = ActionStatus.Success;


                    }
                    else
                    {
                        responseOut.message = "Problem in Sending Mail!!!";
                        responseOut.status = ActionStatus.Fail;

                    }
                }
                else
                {
                    responseOut.message = "Entered Email not exists!!!";
                    responseOut.status = ActionStatus.Fail;

                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }
        

        [AllowAnonymous]
        public ActionResult ChangePassword()
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


        [HttpPost] 
        public ActionResult ChangePassword(UserViewModel userViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            UserBL userBL = new UserBL();
            try
            {
                if (userViewModel != null)
                {
                    userViewModel.CreatedBy = ContextUser.UserId;
                    userViewModel.UserId = ContextUser.UserId;
                    responseOut = userBL.ChangePassword(userViewModel);
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


        [AllowAnonymous]
        public ActionResult SignOut()
        {
            try
            {
                Session.Abandon();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return RedirectToAction("Login", "Home");
        }
        [AllowAnonymous]
        public ActionResult SetSession()
        {
            Session["Test"] = "Test Value";
            return View();
        }
        [AllowAnonymous]
        public ActionResult Keepalive()
        {
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult AccessDenied(string redirectURL="")
        {
            try
            {
                ViewBag.RedirectURL = redirectURL;
                if (string.IsNullOrEmpty(redirectURL))
                { 
                ViewBag.ErrorMessage = ActionMessage.AccessDenied;
                }
                else if (redirectURL == "SA")
                {
                    string errMessage = "<p style='font-weight:bold;font-size:15px;text-align:center;line-height:25px;margin-top:28px;'>Access to the requested page has been denied</p>";
                    //errMessage += "<p style='font-size:15px;text-align:center;'>Please contact to administrator.<a href='../Company/SuperAdminDashboard\")' class='btn btn-success'>Click Here</a> to go back to Dashboard</p>";
                    ViewBag.ErrorMessage = errMessage;
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
            return View();
        }
    }
}
