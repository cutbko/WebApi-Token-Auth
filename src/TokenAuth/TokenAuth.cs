using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace TokenAuth
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class TokenAutorize : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //actionContext.Request.Headers.TryGetValues()
        }
    }
}