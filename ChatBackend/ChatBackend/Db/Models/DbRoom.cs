using ServiceStack.DataAnnotations;

namespace ChatBackend.Db.Models
{
    public class DbRoom
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTimeOffset CreationDateTime { get; set; }
    }
}
