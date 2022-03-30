using ServiceStack.DataAnnotations;

namespace ChatBackend.Db.Models
{
    public class DbRoomLatestActivity
    {
        [AutoIncrement]
        public int Id { get; set; }
        public int DbRoomId { get; set; }
        public int MessageId { get; set; }
        public DateTimeOffset LatestActivityDateTime { get; set; }
    }
}
