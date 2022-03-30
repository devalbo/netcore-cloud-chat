using ChatBackend.Db.Repositories;
using ChatBackend.Dto;
using ChatBackend.Middleware.Auth;

namespace ChatBackend.Auth
{
    public enum UserLoginState
    {
        NotSet,
        DoesNotExist,
        WasAlreadyAuthorized,
        JustAuthorized,
    }

    public class LoginOrchestratorResult
    {
        public UserLoginState UserLoginState { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public string LoggedInScreenName { get; set; }
    }

    public interface ILoginOrchestrator
    {
        Task<LoginOrchestratorResult> DoLogin(UserCredentials userCredentials);
    }

    public class LoginOrchestrator: ILoginOrchestrator
    {
        private readonly IUserAccessAuthorizationProvider _userAccessAuthorizationProvider;
        private readonly IUserRepository _userRepository;
        private readonly IUserAccessTokenFactory _userAccessTokenFactory;

        public LoginOrchestrator(
            IUserAccessAuthorizationProvider userAccessAuthorizationProvider,
            IUserRepository userRepository, 
            IUserAccessTokenFactory userAccessTokenFactory
            )
        {
            _userAccessAuthorizationProvider = userAccessAuthorizationProvider;
            _userRepository = userRepository;
            _userAccessTokenFactory = userAccessTokenFactory;
        }

        public async Task<LoginOrchestratorResult> DoLogin(UserCredentials userCredentials)
        {
            var user = await _userRepository.GetDbUserForUserName(userCredentials.UserName);

            if (user == null)
            {
                return new LoginOrchestratorResult()
                {
                    UserLoginState = UserLoginState.DoesNotExist
                };
            }

            if (_userAccessAuthorizationProvider.IsUserNameCurrentlyAuthorized(userCredentials.UserName, out var userToken))
            {
                return new LoginOrchestratorResult()
                {
                    UserLoginState = UserLoginState.WasAlreadyAuthorized,
                    Token = userToken,
                    LoggedInScreenName = user.ScreenName,
                };
            }

            var token = _userAccessTokenFactory.CreateTokenForUser(userCredentials);
            _userAccessAuthorizationProvider.AuthorizeUser(userCredentials, token);

            return new LoginOrchestratorResult()
            {
                UserLoginState = UserLoginState.JustAuthorized,
                Token = token,
                LoggedInScreenName = user.ScreenName,
            };
        }
    }
}
