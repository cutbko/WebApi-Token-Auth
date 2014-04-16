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
    }
}