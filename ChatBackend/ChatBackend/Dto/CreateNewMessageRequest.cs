namespace ChatBackend.Dto
{
    public class CreateNewMessageRequest
    {
        public int RoomId { get; set; }
        public string Contents { get; set; }
    }
}
