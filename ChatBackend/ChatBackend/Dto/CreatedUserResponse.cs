namespace ChatBackend.Dto
{
    public class CreatedUserResponse
    {
        public int Id { get; set; }
        public bool Success { get; set; }
        public string FinalUserName { get; set; }
    }
}
