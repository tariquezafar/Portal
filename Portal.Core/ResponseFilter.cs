using Portal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace Portal.Core
{
    public class ResponseFilter : FilterAttribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
           // CookieHelper.DeleteCookie(CookieKey.CityId);
           // CookieHelper.DeleteCookie(CookieKey.CityName);
            
        }

    }


    public class BuyCouponFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                //BaseController c = filterContext.Controller as BaseController;
                //if (c != null)
                //{
                //    UserModel currentuser = c.ContextUser;
                //    if (currentuser == null)
                //    {
                //        HttpContextBase request = filterContext.HttpContext;
                //        if (filterContext.HttpContext.Request.QueryString["IsGiftStay"] != null && Convert.ToBoolean(request.Request.QueryString["IsGiftStay"]))
                //        {
                //            // filterContext.Result = new RedirectResult("~/user-registration?IsGetstarted=true&IsGiftStay=true");
                //        }
                //        else
                //        {
                //            filterContext.Result = new RedirectResult("~/user-registration?IsGetstarted=true");
                //        }

                //    }
                //    else
                //    {
                //        if (currentuser.RoleId != Convert.ToInt32(Role.AgencyUser))
                //        {
                //            filterContext.Result = new RedirectResult("~/ResponseMessage/AutherisationFailed");
                //            // TO do: Set it access denied.
                //        }
                //    }
                //}
                //else
                //    filterContext.Result = new RedirectResult("~/home");
            }
            catch (Exception ex)
            {
                Common.Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {

        }


    }
    public class SearchStayFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                //BaseController c = filterContext.Controller as BaseController;
                //if (c != null)
                //{
                //    UserModel currentuser = c.ContextUser;
                //    if (currentuser != null && currentuser.RoleId == Convert.ToInt32(Role.AgencyUser))
                //    {
                //        ModelServices model = new ModelServices();

                //        int userCouponCount = model.GetUserCouponCount(currentuser.UserId);
                //        if (userCouponCount <= 0)
                //        {
                //            filterContext.Result = new RedirectResult("~/buy-vouchers");

                //        }
                //        else if (userCouponCount > 0)
                //        {
                //            List<CouponDetail> couponList = new List<CouponDetail>();
                //            couponList = model.GetUserValidCouponsList(currentuser.UserId, DateTime.Now);
                //            if (couponList != null & couponList.Count <= 0)
                //            {
                //                filterContext.Result = new RedirectResult("~/buy-vouchers");
                //            }

                //        }

                     
                //    }
                //    else
                //    {

                //        filterContext.Result = new RedirectResult("~/ResponseMessage/AutherisationFailed");

                //    }
                //}
                //else
                //{
                //    filterContext.Result = new RedirectResult("~/home");
                //}
            }
            catch (Exception ex)
            {
                Common.Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {

        }


    }
}
