using System;
using System.Web.Http;

namespace TokenAuth
{
    public class TokenAuthController : ApiController
    {
        public dynamic UserData { get; set; }

        protected string LoginToken(dynamic userData)
        {
            string token = GenerateToken();
            
            TokenStorage.Instance.AddTokenWithData(token, userData);

            return token;
        }

        private string GenerateToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}