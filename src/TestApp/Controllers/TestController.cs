using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Web.Http;
using TokenAuth;

namespace TestApp.Controllers
{
    [TokenAutorize]
    public class TestApiController : TokenAuthApiController
    {
        [Route("test")]
        [TokenAutorize]
        public IEnumerable<string> Get()
        {
            return new List<string>
                   {
                       UserData.Name
                   };
        }

        [TokenAutorize(Roles = "admin")]
        [Route("admin")]
        public IEnumerable<string> Admin()
        {
            return new List<string>
                   {
                       UserData.Name
                   };
        }

        [Route("kickAll")]
        public void KickAll()
        {
            Kick(tokenData => tokenData.Name == "user");
        } 
    }
}
