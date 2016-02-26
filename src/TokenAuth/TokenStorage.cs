using System;
using System.Collections.Concurrent;
using System.Linq;

namespace TokenAuth
{
    internal class TokenStorage : ITokenStorage
    {
        private static readonly Lazy<ITokenStorage> TokenStorageLazy =  
                            new Lazy<ITokenStorage>(() => new TokenStorage());

        private readonly ConcurrentDictionary<string, TokenData> _storage = 
                     new ConcurrentDictionary<string, TokenData>();

        private readonly ConcurrentDictionary<Tuple<string, string>, string> _tempTokensStorage =
                     new ConcurrentDictionary<Tuple<string, string>, string>();
        

        internal static ITokenStorage Instance
        {
            get { return TokenStorageLazy.Value; }
        }

        internal static string GenerateRandomToken()
        {
            return new string((Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + Convert.ToBase64String(Guid.NewGuid().ToByteArray())).Where(char.IsLetterOrDigit).ToArray());
        }

        public bool TryGetTokenData(string token, out TokenData data)
        {
            return _storage.TryGetValue(token, out data);
        }

        public void AddTokenWithData(string token, TokenData data)
        {
            _storage.TryAdd(token, data);
        }

        public void RemoveTokenAndData(string token)
        {
            TokenData _;
            _storage.TryRemove(token, out _);
        }

        public void RemoveTokenAndData(Func<dynamic, bool> predicate)
        {
            foreach (var token in _storage.Where(item => predicate(item.Value.UserData))
                                          .Select(item => item.Key))
            {
                RemoveTokenAndData(token);
            }
        }

        public bool Contains(Func<object, bool> predicate)
        {
            return _storage.Any(item => predicate(item.Value.UserData));
        }

        public string CreateSingleTimeTokenForSitePart(string token, string sitePart)
        {
            string singleTimeToken = GenerateRandomToken();

            _tempTokensStorage.TryAdd(Tuple.Create(singleTimeToken, sitePart), token);

            return singleTimeToken;
        }

        public bool TryGetTokenDataBySingleTimeTokenAndSitePart(string singleTimeToken, string sitePart, out TokenData data)
        {
            string token;
            data = null;

            return _tempTokensStorage.TryRemove(Tuple.Create(singleTimeToken, sitePart), out token) && TryGetTokenData(token, out data);
        }
    }
}