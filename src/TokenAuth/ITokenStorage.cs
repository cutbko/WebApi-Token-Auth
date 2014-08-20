using System;

namespace TokenAuth
{
    internal interface ITokenStorage
    {
        bool TryGetTokenData(string token, out TokenData data);

        void AddTokenWithData(string token, TokenData data);

        void RemoveTokenAndData(string token);

        void RemoveTokenAndData(Func<dynamic, bool> predicate);
    }
}