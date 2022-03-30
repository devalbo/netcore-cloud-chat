using ChatBackend.Db.Repositories;

namespace ChatBackend.Auth
{
    public class CreateUserInput
    {
        public string UserName { get; set; }
        public string ScreenName { get; set; }
    }

    public class CreateUserResponse
    {
        public int Id { get; set; }
        public bool UserCreated { get; set; }
        public string? UserNotCreatedReason { get; set; }
        public string? FinalUserName { get; set; }
    }

    public interface IUserCreator
    {
        Task<CreateUserResponse> CreateUser(CreateUserInput user);
    }

    public class UserCreator: IUserCreator
    {
        private readonly IUserNameSanitizer _userNameSanitizer;
        private readonly IUserRepository _userRepository;

        public UserCreator(IUserNameSanitizer userNameSanitizer, IUserRepository userRepository)
        {
            _userNameSanitizer = userNameSanitizer;
            _userRepository = userRepository;
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserInput user)
        {
            var userName = user.UserName;

            var isUserNameValid = _userNameSanitizer.Validate(userName);
            if (!isUserNameValid)
            {
                throw new ApplicationException("Invalid username. Only use English letters and numbers please");
            }

            var sanitizedUserName = _userNameSanitizer.Sanitize(userName);

            var dbUser = await _userRepository.GetDbUserForUserName(sanitizedUserName);
            if (dbUser != null)
            {
                return new CreateUserResponse()
                {
                    UserCreated = false,
                    UserNotCreatedReason = $"DbUser with name {sanitizedUserName} already exists"
                };
            }

            var userId = await _userRepository.AddUserToDb(sanitizedUserName, user.ScreenName);

            return new CreateUserResponse()
            {
                Id = userId,
                UserCreated = true,
                FinalUserName = sanitizedUserName
            };
        }
    }
}
