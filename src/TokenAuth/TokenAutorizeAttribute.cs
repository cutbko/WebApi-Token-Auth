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
        private string[] _roles;

        public string Roles
        {
            get { return string.Join(",", _roles); }

            set
            {
                _roles = !string.IsNullOrWhiteSpace(value) ? value.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries)
                                                                  .Select(role => role.Trim())
                                                                  .ToArray()
                                                           : null;
            }
        }

        public override void OnAuthorization(HttpActionContext context)
        {
            if (!TryAuthorize(context))
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "You are no authorized to access this resource.");
            }
        }

        private bool TryAuthorize(HttpActionContext context)
        {
            var controller = context.ControllerContext.Controller as TokenAuthApiController;
            if (controller == null)
            {
                throw new InvalidOperationException("controller must be derived from TokenAuthController");
            }

            IEnumerable<string> values;
            if (context.Request.Headers.TryGetValues("auth-token", out values))
            {
                string token = values.FirstOrDefault();

                TokenData data;
                if (TokenStorage.Instance.TryGetTokenData(token, out data))
                {
                    if (_roles != null)
                    {
                        if (data.Roles == null || !data.Roles.Intersect(_roles).Any())
                        {
                            return false;
                        }
                    }

                    controller.UserData = data.UserData;
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}