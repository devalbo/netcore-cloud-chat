using ChatBackend.Auth;
using ChatBackend.Dto;
using ChatBackend.Middleware.Auth;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ChatBackend.Controllers
{
    
    [ApiController]
    [Route("auth")]
    [EnableCors]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ILoginOrchestrator _loginOrchestrator;
        private readonly ILogoutOrchestrator _logoutOrchestrator;
        private readonly ILoggedInUserProvider _loggedInUserProvider;
        private readonly IUserNameSanitizer _userNameSanitizer;
        private readonly IUserCreator _userCreator;
        private readonly ICreatedUserResponseMapper _createdUserResponseMapper;

        public AuthController(
            ILogger<AuthController> logger, 
            IUserCreator userCreator, 
            ICreatedUserResponseMapper createdUserResponseMapper,
            IUserNameSanitizer userNameSanitizer,
            ILoginOrchestrator loginOrchestrator,
            ILogoutOrchestrator logoutOrchestrator, 
            ILoggedInUserProvider loggedInUserProvider)
        {
            _logger = logger;
            _userCreator = userCreator;
            _createdUserResponseMapper = createdUserResponseMapper;
            _userNameSanitizer = userNameSanitizer;
            _loginOrchestrator = loginOrchestrator;
            _logoutOrchestrator = logoutOrchestrator;
            _loggedInUserProvider = loggedInUserProvider;
        }

        [HttpGet]
        [Authorize]
        [Route("data")]
        public IActionResult DoCheck()
        {
            return Ok("here's some data");
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<LoginResponse> DoLogin([FromBody] UserCredentials rawUserCredentials)
        {
            var sanitizedUserName = _userNameSanitizer.Sanitize(rawUserCredentials.UserName);
            var userCredentials = new UserCredentials(sanitizedUserName);

            var loginResult = await _loginOrchestrator.DoLogin(userCredentials);

            Response.Cookies.Append("Chat-Auth", loginResult.Token);

            if (loginResult.UserLoginState != UserLoginState.JustAuthorized &&
                loginResult.UserLoginState != UserLoginState.WasAlreadyAuthorized)
            {
                return new LoginResponse()
                {
                    Success = false
                };
            }

            var loginResponse = new LoginResponse()
            {
                Id = loginResult.UserId,
                LoggedInUsername = sanitizedUserName,
                LoggedInScreenName = loginResult.LoggedInScreenName,
                Success = true,
                Token = loginResult.Token
            };

            return loginResponse;
        }

        [HttpPost]
        [Authorize]
        [Route("logout")]
        public IActionResult DoLogout()
        {
            var loggedInUser = _loggedInUserProvider.GetUserForCurrentRequest(this);
            var loggedInUserName = loggedInUser.Username;
            _logoutOrchestrator.DoLogout(loggedInUserName);

            return Ok();
        }

        [HttpPost]
        [Route("create-user")]
        public async Task<CreatedUserResponse> CreateUser([FromBody] CreateUserInput user)
        {
            _logger.LogInformation($"User creation request for username {user.UserName} [{user.ScreenName}]");

            var createUserResponse = await _userCreator.CreateUser(user);
            if (!createUserResponse.UserCreated)
            {
                throw new ApplicationException(createUserResponse.UserNotCreatedReason);
            }

            var createdUserResponse = _createdUserResponseMapper.Map(createUserResponse);

            return createdUserResponse;
        }
    }
}
