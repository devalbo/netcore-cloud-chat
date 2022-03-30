namespace ChatBackend.Auth
{
    public interface IUserNameSanitizer
    {
        bool Validate(string userName);
        string Sanitize(string userName);
    }

    public class UserNameSanitizer: IUserNameSanitizer
    {
        const string AllowedUserNameChars = "abcdefghijklmnopqrstuvwxyz" +
                                            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                            "0123456789";
        public bool Validate(string userName)
        {
            var trimmedUserName = userName.Trim();
            foreach (var c in trimmedUserName)
            {
                if (!AllowedUserNameChars.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }

        public string Sanitize(string userName)
        {
            var trimmedUserName = userName.Trim();
            return trimmedUserName;
        }
    }
}
