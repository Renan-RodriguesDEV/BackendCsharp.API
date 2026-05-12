namespace BackendCsharp.API.DTOs
{
    public class UserRequests
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public UserRequests()
        {
            
        }
        public UserRequests(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;
        }
    }
}
