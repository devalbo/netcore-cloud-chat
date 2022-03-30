using ChatBackend.Auth;

namespace ChatBackend.Middleware.Auth;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenMiddleware> _logger;

    public TokenMiddleware(RequestDelegate next,
        ILogger<TokenMiddleware> logger
        )
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context, ILoggedInUserProvider loggedInUserProvider, IUserAccessAuthorizationProvider userAccessAuthorizationProvider)
    {
        var token = context.Request.Headers["Chat-Auth"].FirstOrDefault();
        if (token == null)
        {
            token = context.Request.Cookies["Chat-Auth"];
        }

        if (token != null)
        {
            var userForToken = await GetUserForToken(loggedInUserProvider, userAccessAuthorizationProvider, token);
            if (userForToken != null)
            {
                var userIsCurrentlyAuthorized = userAccessAuthorizationProvider.IsUserNameCurrentlyAuthorized(userForToken.Username);
                if (userIsCurrentlyAuthorized)
                {
                    AttachUserToRequestContext(context, userForToken);
                }
            }
        }

        await _next(context);
    }

    private async Task<LoggedInUser?> GetUserForToken(ILoggedInUserProvider loggedInUserProvider, 
        IUserAccessAuthorizationProvider userAccessAuthorizationProvider, string token)
    {
        var userName = userAccessAuthorizationProvider.GetUserNameForToken(token);
        if (userName == null)
        {
            _logger.LogDebug($"Invalid token: {token}");
            return null;
        }

        var loggedInUser = await loggedInUserProvider.GetByUsername(userName);
        return loggedInUser;
    }

    //private async Task<LoggedInUser?> GetUserForToken(ILoggedInUserProvider loggedInUserProvider, string token)
    //{
    //    try
    //    {
    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
    //        tokenHandler.ValidateToken(token, new TokenValidationParameters
    //        {
    //            ValidateIssuerSigningKey = true,
    //            IssuerSigningKey = new SymmetricSecurityKey(key),
    //            ValidateIssuer = false,
    //            ValidateAudience = false,
    //            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
    //            ClockSkew = TimeSpan.Zero
    //        }, out SecurityToken validatedToken);

    //        var jwtToken = (JwtSecurityToken)validatedToken;
    //        var userName = jwtToken.Claims.First(x => x.Type == "username").Value;

    //        // attach user to context on successful jwt validation
    //        var loggedInUser = await loggedInUserProvider.GetByUsername(userName);
    //        return loggedInUser;
    //    }
    //    catch
    //    {
    //        _logger.LogDebug($"Invalid token: {token}");
    //        // do nothing if jwt validation fails
    //        // user is not attached to context so request won't have access to secure routes
    //    }

    //    return null;
    //}

    private void AttachUserToRequestContext(HttpContext context, LoggedInUser loggedInUser)
    {
        context.Items["LoggedInUser"] = loggedInUser;
    }
}