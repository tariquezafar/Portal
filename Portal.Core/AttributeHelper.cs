using System.Data;
using System.Reflection;
using System.Web.Caching;
using System.Web.Http.Controllers;
using System.Threading;
using System;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web;
using Portal.Common;
using Portal.Core.ViewModel;
using Portal.DAL;

namespace Portal.Core
{
    /// <summary>
    /// Attribute Class to Validate User
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ValidateRequest : System.Web.Mvc.FilterAttribute, System.Web.Mvc.IAuthorizationFilter
    {
        private  bool _isActive = true;
        private  int _userInterfaceId = 0;
        private  int _accessMode = 3;
        private  int _requestMode = 1;
        public ValidateRequest()
        {
            Order = 1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive">To validate or not request </param>
        /// <param name="userInterfaceId">user interface id to be authorized</param>
        /// <param name="accessId">Access permission to be checked </param>
        /// <param name="requestMode">Request mode whether POST/GET or Ajax</param>
        public ValidateRequest(bool isActive, int userInterfaceId, int accessMode=3, int requestMode=1)
        {
            _isActive = isActive;
            _userInterfaceId = userInterfaceId;
            _accessMode = accessMode;
            _requestMode = requestMode;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            _accessMode = (int)AccessMode.ViewAccess;
            if (!_isActive)
                return;
          
            ResponseOut responseOut = new ResponseOut();
            BaseController c = filterContext.Controller as BaseController;
            if (c != null)
            {
                String controllerName = filterContext.RequestContext.RouteData.Values["Controller"].ToString();
                String methodName = filterContext.RequestContext.RouteData.Values["action"].ToString();
                string requestAccessMode = filterContext.HttpContext.Request.QueryString["AccessMode"];
                if (!string.IsNullOrEmpty(requestAccessMode))
                {
                    _accessMode =Convert.ToInt16(requestAccessMode);
                }
                //else
                //{
                //    if (_accessMode==0)
                //    { 
                //    _accessMode = (int)AccessMode.ViewAccess;
                //    }
                //}


                UserViewModel currentuser = c.ContextUser;
                if (currentuser == null)
                {
                    if (_requestMode == (int)RequestMode.Ajax)
                    {
                        JsonResult jsonResult = new JsonResult();
                        responseOut.status = ActionStatus.Fail;
                        responseOut.message = ActionMessage.SessionException;
                        jsonResult.Data = responseOut;
                        jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                        filterContext.Result = jsonResult;
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/Home/Login");
                    }
                }
                else
                {
                    bool flag = false;
                    if (HttpContext.Current.Session[SessionKey.IsAuthenticated] != null)
                    {
                        flag = (bool)HttpContext.Current.Session[SessionKey.IsAuthenticated];
                    }
                    if (flag)
                    {
                        if (!IsUserAuthorised(currentuser.RoleId ,_userInterfaceId,_accessMode))
                        {
                            if (_requestMode == (int)RequestMode.Ajax)
                            {
                                JsonResult jsonResult = new JsonResult();
                                responseOut.status = ActionStatus.Fail;
                                responseOut.message = ActionMessage.AccessDenied;
                                jsonResult.Data = responseOut;
                                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                                filterContext.Result = jsonResult;
                            }
                            else
                            {
                                filterContext.Result = new RedirectResult("~/Home/AccessDenied?redirectURL=SA");
                            }
                            //filterContext.Result = new RedirectResult("~/ResponseMessage/AutherisationFailed");
                            // TO do: Set it access denied.
                        }
                    }
                    else
                    {
                        if (_requestMode == (int)RequestMode.Ajax)
                        {
                            JsonResult jsonResult = new JsonResult();
                            responseOut.status = ActionStatus.Fail;
                            responseOut.message = ActionMessage.AuthenticationException;
                            jsonResult.Data = responseOut;
                            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                            filterContext.Result = jsonResult;
                        }
                        else
                        {
                            filterContext.Result = new RedirectResult("~/Home/Login");
                        }

                    }
                }
            }
            else
            {
                if (_requestMode == (int)RequestMode.Ajax)
                {
                    JsonResult jsonResult = new JsonResult();
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                    jsonResult.Data = responseOut;
                    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    filterContext.Result = jsonResult;
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Home/Login");
                }
            }

        }

        #region HelperMethod
        private Boolean IsUserAuthorised(int roleId,int interfaceId,int accessMode)
        {
            DBInterface dbInterface = new DBInterface();
            if (dbInterface.AuthorizeUser(roleId,interfaceId,accessMode))
            {
                return true;
            }
            else
            {
                return false;
            }


          

        }
        #endregion
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true), System.Runtime.InteropServices.GuidAttribute("2CCF89FA-8C29-4BDA-B091-9BB12EB74FC4")]
    public class AfterActionExecuteAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    HttpContext ctx = HttpContext.Current;

        //    // check if session is supported
        //    if (ctx.Session != null)
        //    {

        //        // check if a new session id was generated
        //        if (ctx.Session.IsNewSession)
        //        {

        //            // If it says it is a new session, but an existing cookie exists, then it must
        //            // have timed out
        //            string sessionCookie = ctx.Request.Headers["Cookie"];
        //            if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
        //            {
        //                ctx.Response.Redirect("~/home/Login");
        //            }
        //        }
        //    }

        //    base.OnActionExecuting(filterContext);
        //}
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var ctr = filterContext.Controller as BaseController;
            if (ctr != null)
            {
                filterContext.Controller.ViewBag.CurrentUser = ctr.ContextUser;
            }
        }
      
      
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true), System.Runtime.InteropServices.GuidAttribute("2CCF89FA-8C29-4BDA-B091-9BB12EB74FC4")]
    public class CheckSessionBeforeControllerExecuteAttribute : System.Web.Mvc.ActionFilterAttribute
    {
       
        //
        // Summary:
        //     Called by the ASP.NET MVC framework before the action method executes.
        //
        // Parameters:
        //   filterContext:
        //     The filter context.
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool flag = false;
            if (HttpContext.Current.Session[SessionKey.IsAuthenticated] != null)
            {
                flag = (bool)HttpContext.Current.Session[SessionKey.IsAuthenticated];
            }
            if (!flag)
            {
                filterContext.Result = new RedirectResult("~/Home/Login");
            }
        }
    }
}
