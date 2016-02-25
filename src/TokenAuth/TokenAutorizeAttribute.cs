using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
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
        private TokenSource _tokenSource = TokenSource.RequestHeader;

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

        public string SitePart { get; set; }

        public TokenSource TokenSource
        {
            get { return _tokenSource; }
            set { _tokenSource = value; }
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

            string token = null;

            string tempToken = null;

            if (TokenSource == TokenSource.RequestHeader)
            {
                IEnumerable<string> values;
                if (context.Request.Headers.TryGetValues("auth-token", out values))
                {
                    token = values.FirstOrDefault();
                }

                if (context.Request.Headers.TryGetValues("temp-auth-token", out values))
                {
                    token = values.FirstOrDefault();
                }
            }
            else if (TokenSource == TokenSource.QueryString)
            {
                NameValueCollection queryString = HttpContext.Current.Request.QueryString;

                if (queryString.HasKeys())
                {
                    for (int i = 0; i < queryString.Count; i++)
                    {
                        if (queryString.Keys[i].ToLower() == "temp-auth-token")
                        {
                            tempToken = queryString[queryString.Keys[i]];
                        }
                        
                        if (queryString.Keys[i].ToLower() == "auth-token")
                        {
                            token = queryString[queryString.Keys[i]];
                        }
                    }
                }
            }

            if (token == null && tempToken == null)
            {
                return false;
            }

            TokenData data;
            if (token != null && TokenStorage.Instance.TryGetTokenData(token, out data)
             || tempToken != null && TokenStorage.Instance.TryGetTokenDataBySingleTimeTokenAndSitePart(tempToken, SitePart, out data))
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
    }

    public enum TokenSource
    {
        RequestHeader,
        QueryString
    }
}