namespace ChatBackend.Middleware.Auth.Models;

public class AuthenticateResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }


    public AuthenticateResponse(AuthUser authUser, string token)
    {
        Id = authUser.Id;
        FirstName = authUser.FirstName;
        LastName = authUser.LastName;
        Username = authUser.Username;
        Token = token;
    }
}