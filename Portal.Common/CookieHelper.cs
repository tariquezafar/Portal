using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Portal.Common
{ /// <summary>
  /// Class for adding and getting cookies.
  /// </summary>
    public class CookieHelper
    {
        public static object HttpContext { get; private set; }

        /// <summary>
        /// Method to set cookie.
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        public static void SetCookie(string cookieName, string cookieValue)
        {
            try
            {
                DeleteCookie(cookieName);

                HttpCookie myCookie;
                HttpCookie myCookieExpires;
                
                if (System.Web.HttpContext.Current.Response.Cookies.Get(cookieName) == null)
                {
                    myCookie = new HttpCookie(cookieName);
                    myCookieExpires = new HttpCookie(cookieName + "Expires");
                }
                else
                {
                    myCookie = System.Web.HttpContext.Current.Response.Cookies.Get(cookieName);
                    myCookieExpires = System.Web.HttpContext.Current.Response.Cookies.Get(cookieName + "Expires");
                }

                myCookie.Value = cookieValue;
                myCookie.Expires = DateTime.Now.AddMinutes(60 * 24 * 20);
                myCookieExpires.Value = DateTime.Now.AddMinutes(60 * 24 * 20).ToString();
                myCookieExpires.Expires = DateTime.Now.AddMinutes(60 * 24 * 20);

                System.Web.HttpContext.Current.Response.Cookies.Set(myCookie);
                System.Web.HttpContext.Current.Response.Cookies.Set(myCookieExpires);
            }
            catch (Exception ex)
            {
                //Logger.SaveErrorLog("CookieHelper", MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        /// <summary>
        /// Method to get cookie.
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookie(string cookieName)
        {
            try
            {
                HttpCookie myCookie = System.Web.HttpContext.Current.Request.Cookies.Get(cookieName);
                HttpCookie myCookieExpires = System.Web.HttpContext.Current.Request.Cookies.Get(cookieName + "Expires");
                return myCookie == null ? string.Empty : Convert.ToDateTime(myCookieExpires.Value) < DateTime.Now ? string.Empty : myCookie.Value;
            }
            catch (Exception ex)
            {
                //Logger.SaveErrorLog("CookieHelper", MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public static void DeleteCookie(string cookieName)
        {
            try
            {
                HttpCookie myCookie = System.Web.HttpContext.Current.Response.Cookies.Get(cookieName);
                HttpCookie myCookieExpires = System.Web.HttpContext.Current.Response.Cookies.Get(cookieName + "Expires");
                if (myCookie != null)
                {
                    myCookie.Expires = DateTime.Now.AddDays(-1D);
                    myCookieExpires.Expires = DateTime.Now.AddDays(-1D);
                    System.Web.HttpContext.Current.Response.Cookies.Set(myCookie);
                    System.Web.HttpContext.Current.Response.Cookies.Set(myCookieExpires);
                }
            }
            catch (Exception ex)
            {
               // Logger.SaveErrorLog("CookieHelper", MethodBase.GetCurrentMethod().Name, ex);
            }
        }
    }

   
}
