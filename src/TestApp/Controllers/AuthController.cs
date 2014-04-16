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
    public class AuthController : TokenAuthController
    {
        [Route("login")]
        public HttpResponseMessage PostLogin()
        {
            dynamic userData = new UserDataClass();
            userData.Name = "name";

            string loginToken = LoginToken(userData);

            return Request.CreateResponse(HttpStatusCode.OK, loginToken);
        }

        [TokenAutorize]
        [Route("logout")]
        public HttpResponseMessage PostLogout()
        {
            Logout();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private class UserDataClass : DynamicObject
        {
            private readonly ConcurrentDictionary<string, object> _properties = 
                         new ConcurrentDictionary<string, object>();

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                return _properties.TryGetValue(binder.Name, out result);
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                _properties[binder.Name] = value;
                return true;
            }

        }
    }
}
