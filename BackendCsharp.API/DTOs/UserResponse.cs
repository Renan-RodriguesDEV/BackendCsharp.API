namespace BackendCsharp.API.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public UserResponse()
        {
            
        }
        public UserResponse(Guid Id, string Email,DateTime CreatedAt)
        {
            this.Id = Id;
            this.Email = Email;
            this.CreatedAt = CreatedAt;
        }
    }
}
