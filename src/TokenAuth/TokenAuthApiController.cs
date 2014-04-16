using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Web.Http;

namespace TokenAuth
{
    public class TokenAuthApiController : ApiController
    {
        public dynamic UserData { get; set; }

        protected string LoginToken(Action<dynamic> setUserData)
        {
            dynamic userData = new DynamicUserData();
            setUserData(userData);

            string token = GenerateToken();
            userData.Token = token;

            TokenStorage.Instance.AddTokenWithData(token, userData);

            return token;
        }

        protected void Logout()
        {
            if (UserData == null || UserData.Token == null)
            {
                throw new InvalidOperationException("Logout action must be authorized");
            }

            TokenStorage.Instance.RemoveTokenAndData(UserData.Token);
        }

        private string GenerateToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        private class DynamicUserData : DynamicObject
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