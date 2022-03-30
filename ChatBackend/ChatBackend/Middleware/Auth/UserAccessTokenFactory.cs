using ChatBackend.Dto;
using Microsoft.Extensions.Options;

namespace ChatBackend.Middleware.Auth
{
    public interface IUserAccessTokenFactory
    {
        //string CreateAuthTokenForUser(UserCredentials userCredentials);
        string CreateTokenForUser(UserCredentials userCredentials);
    }

    public class UserAccessTokenFactory : IUserAccessTokenFactory
    {
        private readonly AppSettings _appSettings;

        public UserAccessTokenFactory(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        
        public string CreateTokenForUser(UserCredentials userCredentials)
        {
            var guid = Guid.NewGuid();
            return guid.ToString();
        }

        //public string CreateTokenForUser(UserCredentials userCredentials)
        //{
        //    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        //    var userNameClaim = new Claim("username", userCredentials.UserName);
        //    var signingCredentials =
        //        new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        //    // generate token that is valid for 7 days
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[] { userNameClaim }),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        SigningCredentials = signingCredentials
        //    };

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var token = tokenHandler.CreateToken(tokenDescriptor);

        //    return tokenHandler.WriteToken(token);
        //}
    }
}
