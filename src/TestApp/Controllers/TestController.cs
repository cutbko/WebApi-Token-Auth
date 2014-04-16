using System.Collections.Generic;
using System.Web.Http;
using TokenAuth;

namespace TestApp.Controllers
{
    public class TestApiController : TokenAuthApiController
    {
        [TokenAutorize]
        public IEnumerable<string> Get()
        {
            return new List<string>
                   {
                       UserData.Name
                   };
        } 
    }
}
