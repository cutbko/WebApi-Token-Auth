using System;
using System.Collections.Concurrent;
using System.Linq;

namespace TokenAuth
{
    internal class TokenStorage : ITokenStorage
    {
        private static readonly Lazy<ITokenStorage> TokenStorageLazy =  
                            new Lazy<ITokenStorage>(() => new TokenStorage());

        private readonly ConcurrentDictionary<string, dynamic> _storage = 
                     new ConcurrentDictionary<string, dynamic>();
        

        internal static ITokenStorage Instance
        {
            get { return TokenStorageLazy.Value; }
        }

        internal TokenStorage()
        {
        }

        public bool TryGetTokenData(string token, out dynamic data)
        {
            return _storage.TryGetValue(token, out data);
        }

        public void AddTokenWithData(string token, dynamic data)
        {
            _storage.TryAdd(token, data);
        }

        public void RemoveTokenAndData(string token)
        {
            dynamic _;
            _storage.TryRemove(token, out _);
        }
    }
}