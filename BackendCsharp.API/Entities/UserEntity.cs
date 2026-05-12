namespace BackendCsharp.API.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public UserEntity(string Email, string Password)
        {
            this.Id = Guid.NewGuid();
            this.Email = Email;
            this.Password = Password;
            this.CreatedAt = DateTime.UtcNow;
        }
    }
}
