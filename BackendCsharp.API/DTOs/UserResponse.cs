namespace BackendCsharp.API.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public UserResponse()
        {
            
        }
        public UserResponse(Guid Id, string Username,DateTime CreatedAt)
        {
            this.Id = Id;
            this.Username = Username;
            this.CreatedAt = CreatedAt;
        }
    }
}
