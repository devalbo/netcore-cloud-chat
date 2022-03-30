namespace ChatBackend.Dto
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string LoggedInUsername { get; set; }
        public string LoggedInScreenName { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
    }
}
