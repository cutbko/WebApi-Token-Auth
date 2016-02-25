using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.Http;

namespace TokenAuth
{
    public class TokenAuthApiController : ApiController
    {
        public dynamic UserData { get; set; }

        protected string LoginToken(Action<dynamic> setUserData, IEnumerable<string> roles = null)
        {
            var tokenData = new TokenData
                            {
                                Roles = roles,
                                UserData = new DynamicUserData()
                            };

            setUserData(tokenData.UserData);

            string token = GenerateToken();
            tokenData.UserData.Token = token;

            TokenStorage.Instance.AddTokenWithData(token, tokenData);

            return token;
        }

        protected string SingleTimeTokenForSitePart(string token, string sitePart)
        {
            return TokenStorage.Instance.CreateSingleTimeTokenForSitePart(token, sitePart);
        }

        protected void Logout()
        {
            if (UserData == null || UserData.Token == null)
            {
                throw new InvalidOperationException("Logout action must be authorized");
            }

            TokenStorage.Instance.RemoveTokenAndData(UserData.Token);
        }

        protected void Kick(Func<dynamic, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            TokenStorage.Instance.RemoveTokenAndData(predicate);
        }

        protected bool IsOnline(Func<dynamic, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            return TokenStorage.Instance.Contains(predicate);
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