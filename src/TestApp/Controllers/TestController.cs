﻿using System.Collections.Generic;
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
            IsOnline(tokenData => tokenData.Name == "user");
        }

        [Route("testQueryAuth")]
        [TokenAutorize(TokenSource = TokenSource.QueryString)]
        public string QueryAuth()
        {
            return UserData.Name;
        }

        [Route("testQueryAuthWithSite")]
        [HttpGet]
        [TokenAutorize(TokenSource = TokenSource.QueryString, SitePart = "testQueryAuthWithSite")]
        public string QueryAuthWithSite()
        {
            return UserData.Name;
        }
    }
}
