//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using ChatBackend.Db.Repositories;
//using ChatBackend.Middleware.Auth.Models;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;

//namespace ChatBackend.Middleware.Auth;

//public interface IAuthUserService
//{
//    Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
//    //IEnumerable<AuthUser> GetAll();
//    AuthUser? GetByUsername(string username);
//}

//public class AuthUserService : IAuthUserService
//{
//    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
//    //private List<AuthUser> _users = new List<AuthUser>
//    //{
//    //    new AuthUser { Id = 1, FirstName = "Test", LastName = "AuthUser", Username = "test", Password = "test" }
//    //};

//    private readonly IUserRepository _userRepository;
//    private readonly AppSettings _appSettings;

//    public AuthUserService(
//        IOptions<AppSettings> appSettings, 
//        IUserRepository userRepository)
//    {
//        _userRepository = userRepository;
//        _appSettings = appSettings.Value;
//    }

//    public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
//    {
//        var user = await _userRepository.GetDbUserForUserName(model.Username);
//        //var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

//        // return null if authUser not found
//        if (user == null)
//        {
//            return null;
//        }

//        // authentication successful so generate jwt token
//        var token = generateJwtToken(user);

//        return new AuthenticateResponse(user, token);
//    }

//    //public IEnumerable<AuthUser> GetAll()
//    //{
//    //    return _users;
//    //}

//    //public AuthUser? GetById(int id)
//    //{
//    //    return _users.FirstOrDefault(x => x.Id == id);
//    //}


//    private string generateJwtToken(AuthUser authUser)
//    {
//        // generate token that is valid for 7 days
//        var tokenHandler = new JwtSecurityTokenHandler();
//        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
//        var tokenDescriptor = new SecurityTokenDescriptor
//        {
//            Subject = new ClaimsIdentity(new[] { new Claim("id", authUser.Id.ToString()) }),
//            Expires = DateTime.UtcNow.AddDays(7),
//            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//        };
//        var token = tokenHandler.CreateToken(tokenDescriptor);
//        return tokenHandler.WriteToken(token);
//    }
//}