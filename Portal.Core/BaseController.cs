using Newtonsoft.Json;
using Portal.Common;
using Portal.Core.ViewModel;
using System;
using System.Net;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace Portal.Core
{
    [AfterActionExecuteAttribute]
    public class BaseController : System.Web.Mvc.Controller
    {
        //public ModelServices objDBModel = new ModelServices();
        private UserViewModel _user = null;
        public ISessionWrapper SessionWrapper { get; set; }
      
        protected override void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;
            //if (Logger.IsErrorEnabled)
            //{
            string controllerName = (filterContext.RouteData.Values["controller"]).ToString() + " Controller";
            string actionName = (filterContext.RouteData.Values["action"]).ToString() + "()" + " Action";
            //String exceptionMessage = "Some exception occurs in method "+actionName+" of controller "+controllerName+" : " + ex.Message;
            //Logger.SaveLog(exceptionMessage);

            Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);

            //filterContext.Result = new RedirectResult("~/home/home");
            //}   
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            try
            {

                //if (Session["IsAuthenticated"] == null || Convert.ToBoolean(Session["IsAuthenticated"]) == false)
                //{
                //    Session.Abandon();
                //    Response.Redirect("~/Home/Login");
                //}
               

                //UserTracking userTracking = new UserTracking();
                //userTracking.SessionID = System.Web.HttpContext.Current.Session.SessionID;
                //userTracking.ControllerName = requestContext.RouteData.Values["Controller"].ToString();
                //userTracking.ActionName = requestContext.RouteData.Values["action"].ToString();
                //if (Session[SessionKey.CurrentUser] != null)
                //{
                //    UserModel currentuser = SessionWrapper.GetFromSession<UserModel>("CurrentUser") as UserModel;
                //    userTracking.UserID = currentuser.UserId;
                //}
                //userTracking.UserIP = GetIPAddress();
                //int pin;
                //userTracking.UserLocation = GetIPLocation(userTracking.UserIP, out pin);
                //userTracking.CreatedDate = DateTime.Now;
                //userTracking.PinCode = pin;
                //ModelServices service = new ModelServices();
                //service.SaveUserHittingDetails(userTracking);
            }
            catch (Exception ex)
            {
                if (Logger.IsErrorEnabled)
                {
                    Logger.WriteErrorLog(ex);
                }
            }
        }

        public UserViewModel ContextUser
        {
            get
            {
                if (_user == null)
                {
                    _user = SessionWrapper.GetFromSession<UserViewModel>("CurrentUser") as UserViewModel;
                }
                return _user;
            }
        }

        public BaseController()
        {
            SessionWrapper = new SessionWrapper();
            ViewBag.CurrentUser = this.ContextUser;

            

            //if (this.DashBoardUrl != null)
            //    TempData["DashBoardUrl"] = this.DashBoardUrl;

        }

   
  
    }
}
