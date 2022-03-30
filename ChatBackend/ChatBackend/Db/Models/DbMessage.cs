using ServiceStack.DataAnnotations;

namespace ChatBackend.Db.Models
{
    public class DbMessage
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string Content { get; set; }
        public int RoomId { get; set; }
        public int PostedByUserId { get; set; }
        public DateTimeOffset CreationDateTime { get; set; }
    }
}
