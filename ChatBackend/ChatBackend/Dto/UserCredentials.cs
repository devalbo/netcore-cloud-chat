namespace ChatBackend.Dto
{
    public record UserCredentials
    {
        public UserCredentials(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; set; }
    }
}
