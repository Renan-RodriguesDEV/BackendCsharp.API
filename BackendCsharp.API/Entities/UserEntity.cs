namespace BackendCsharp.API.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserEntity(string Username, string Password)
        {
            this.Id = Guid.NewGuid();
            this.Username = Username;
            this.Password = Password;
            this.CreatedAt = DateTime.UtcNow;
        }
    }
}
