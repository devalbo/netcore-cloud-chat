using ChatBackend.Middleware.Auth;

namespace ChatBackend.Auth
{
    public interface ILogoutOrchestrator
    {
        void DoLogout(string userName);
    }

    public class LogoutOrchestrator: ILogoutOrchestrator
    {
        private readonly IUserAccessAuthorizationProvider _userAccessAuthorizationProvider;

        public LogoutOrchestrator(IUserAccessAuthorizationProvider userAccessAuthorizationProvider)
        {
            _userAccessAuthorizationProvider = userAccessAuthorizationProvider;
        }


        public void DoLogout(string userName)
        {
            _userAccessAuthorizationProvider.RemoveUser(userName);
        }
    }
}
