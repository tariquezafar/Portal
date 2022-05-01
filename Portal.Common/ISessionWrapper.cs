using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Portal.Common
{
    public interface ISessionWrapper
    {
        T GetFromSession<T>(string key);
        void SetInSession(string key, object value);
    }
    public class SessionWrapper : ISessionWrapper
    {
        public T GetFromSession<T>(string key)
        {
            return (T)HttpContext.Current.Session[key];
        }

        public void SetInSession(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }
       
    }
}
    