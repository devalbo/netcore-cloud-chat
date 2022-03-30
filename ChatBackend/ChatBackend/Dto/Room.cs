namespace ChatBackend.Dto
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Message? MostRecentMessage { get; set; }
    }
}
