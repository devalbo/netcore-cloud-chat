namespace ChatBackend.Dto
{
    public class Message
    {
        public int Id { get; set; }
        public User SentBy { get; set; }
        public string Content { get; set; }
        public DateTimeOffset SentAt { get; set; }
    }
}
