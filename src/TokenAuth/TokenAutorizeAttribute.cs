using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace TokenAuth
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class TokenAutorize : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext context)
        {
            if (!TryAuthorize(context))
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "You are no authorized to access this resource.");
            }
        }

        private bool TryAuthorize(HttpActionContext context)
        {
            var controller = context.ControllerContext.Controller as TokenAuthController;
            if (controller == null)
            {
                throw new InvalidOperationException("controller must be derived from TokenAuthController");
            }

            IEnumerable<string> values;
            if (context.Request.Headers.TryGetValues("auth-token", out values))
            {
                string token = values.FirstOrDefault();

                dynamic data;
                if (TokenStorage.Instance.TryGetTokenData(token, out data))
                {
                    controller.UserData = data;
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}