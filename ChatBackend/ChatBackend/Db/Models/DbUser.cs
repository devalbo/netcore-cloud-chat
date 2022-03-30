using ServiceStack.DataAnnotations;

namespace ChatBackend.Db.Models
{
    public class DbUser
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ScreenName { get; set; }
        public DateTimeOffset CreationDateTime { get; set; }
    }
}
