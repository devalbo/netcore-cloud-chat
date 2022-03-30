using ChatBackend.Db.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ChatBackend.Auth
{
    public interface ILoggedInUserProvider
    {
        Task<LoggedInUser?> GetByUsername(string userName);
        LoggedInUser GetUserForCurrentRequest(ControllerBase controller);
    }

    public class LoggedInUserProvider: ILoggedInUserProvider
    {
        private readonly IUserRepository _userRepository;

        public LoggedInUserProvider(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoggedInUser?> GetByUsername(string userName)
        {
            var dbUser = await _userRepository.GetDbUserForUserName(userName);
            if (dbUser == null)
            {
                throw new ApplicationException($"No user found: {userName}");
            }

            var userDbId = dbUser.Id;
            return new LoggedInUser()
            {
                Username = userName,
                UserDbId = userDbId
            };
        }

        public LoggedInUser GetUserForCurrentRequest(ControllerBase controller)
        {
            var loggedInUser = (LoggedInUser?)controller.HttpContext.Items["LoggedInUser"];
            if (loggedInUser == null)
            {
                throw new ApplicationException("Unable to get user for request");
            }

            return loggedInUser;
        }
    }
}
