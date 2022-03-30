namespace ChatBackend.Dto
{
    public class CreateNewRoomResponse
    {
        public int Id { get; set; }
        public bool Success { get; set; }
        public string FailureReason { get; set; }
    }
}
