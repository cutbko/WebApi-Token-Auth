using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TokenAuth;

namespace TestApp.Controllers
{
    public class AuthApiController : TokenAuthApiController
    {
        [Route("login")]
        public HttpResponseMessage PostLogin([FromBody]string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "username is not provided");
            }

            if (username == "user")
            {
                string loginToken = LoginToken(userData => userData.Name = "user",
                                               new[] { "user" });

                return Request.CreateResponse(HttpStatusCode.OK, loginToken);
            }

            if (username == "admin")
            {
                string loginToken = LoginToken(userData => userData.Name = "admin",
                                               new[] { "user", "admin" });

                return Request.CreateResponse(HttpStatusCode.OK, loginToken);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "incorect username");
        }

        [TokenAutorize]
        [Route("logout")]
        public HttpResponseMessage PostLogout()
        {
            Logout();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
