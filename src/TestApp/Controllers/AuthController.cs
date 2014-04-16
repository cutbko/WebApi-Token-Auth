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
        public HttpResponseMessage PostLogin()
        {
            string loginToken = LoginToken(userData => userData.Name = "name");

            return Request.CreateResponse(HttpStatusCode.OK, loginToken);
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
