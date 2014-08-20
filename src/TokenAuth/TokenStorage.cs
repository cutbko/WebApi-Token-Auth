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
        

        internal static ITokenStorage Instance
        {
            get { return TokenStorageLazy.Value; }
        }

        internal TokenStorage()
        {
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
    }
}