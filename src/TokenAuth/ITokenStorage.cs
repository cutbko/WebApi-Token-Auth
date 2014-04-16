namespace TokenAuth
{
    public interface ITokenStorage
    {
        bool TryGetTokenData(string token, out dynamic data);

        void AddTokenWithData(string token, dynamic data);

        void RemoveTokenAndData(string token);
    }
}