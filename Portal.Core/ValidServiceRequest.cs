using Portal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Portal.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ValidServiceRequest : AuthorizationFilterAttribute
    {
        private readonly bool _isActive = true;

        public ValidServiceRequest()
        {
        }
        public ValidServiceRequest(bool isActive)
        {
            _isActive = isActive;
        }

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            try
            {
                if (!_isActive) return;
                if (filterContext.Request.Headers.Authorization != null)
                {
                    var authorisationHeader = filterContext.Request.Headers.Authorization.Parameter;
                    if (authorisationHeader != null)
                    {
                        var authHeaderValue = Encoding.Default.GetString(Convert.FromBase64String(authorisationHeader));
                        String userName = authHeaderValue.Split(':').FirstOrDefault();
                        String password = authHeaderValue.Split(':').LastOrDefault();
                        //ModelServices objDBModel = new ModelServices();
                        //if (objDBModel.IsAuthentication(userName, password))
                        //{
                        //    return;
                        //}
                    }
                }

                filterContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                base.OnAuthorization(filterContext);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
            }
        }


    }

}
