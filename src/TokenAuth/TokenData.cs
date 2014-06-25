using System.Collections;
using System.Collections.Generic;

namespace TokenAuth
{
    internal class TokenData
    {
        public IEnumerable<string> Roles { get; set; }

        public dynamic UserData { get; set; }
    }
}