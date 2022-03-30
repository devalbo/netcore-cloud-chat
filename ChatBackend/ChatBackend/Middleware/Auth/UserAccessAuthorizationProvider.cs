using ChatBackend.Dto;

namespace ChatBackend.Middleware.Auth
{
    public interface IUserAccessAuthorizationProvider
    {
        bool IsUserNameCurrentlyAuthorized(string userName);
        bool IsUserNameCurrentlyAuthorized(string userName, out string userToken);
        void AuthorizeUser(UserCredentials userCredentials, string userToken);
        void RemoveUser(string userName);
        string? GetUserNameForToken(string token);
    }

    public class UserAccessAuthorizationProvider: IUserAccessAuthorizationProvider
    {
        private readonly Dictionary<string, string> _currentlyAuthorizedUsersAndTokens = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _tokensForUserNames = new Dictionary<string, string>();

        public bool IsUserNameCurrentlyAuthorized(string userName)
        {
            return _currentlyAuthorizedUsersAndTokens.Keys.Contains(userName);
        }

        public bool IsUserNameCurrentlyAuthorized(string userName, out string userToken)
        {
            var found = _currentlyAuthorizedUsersAndTokens.TryGetValue(userName, out var token);
            if (!found)
            {
                userToken = "";
                return false;
            }

            userToken = token!;
            return found;
        }

        public void AuthorizeUser(UserCredentials userCredentials, string userToken)
        {
            _currentlyAuthorizedUsersAndTokens[userCredentials.UserName] = userToken;
            _tokensForUserNames[userToken] = userCredentials.UserName;
        }

        public void RemoveUser(string userName)
        {
            if (_currentlyAuthorizedUsersAndTokens.ContainsKey(userName))
            {
                var token = _currentlyAuthorizedUsersAndTokens[userName];
                _currentlyAuthorizedUsersAndTokens.Remove(userName);
                _tokensForUserNames.Remove(token);
            }
        }

        public string? GetUserNameForToken(string token)
        {
            _tokensForUserNames.TryGetValue(token, out var userName);
            return userName;
        }
    }
}
