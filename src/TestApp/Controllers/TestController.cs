﻿using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Web.Http;
using TokenAuth;

namespace TestApp.Controllers
{
    [TokenAutorize]
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

        [TokenAutorize(Roles = "admin")]
        [Route("admin")]
        public IEnumerable<string> Admin()
        {
            return new List<string>
                   {
                       UserData.Name
                   };
        } 
    }
}
