namespace BackendCsharp.API.DTOs
{
    public class UserRequests
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public UserRequests()
        {
            
        }
        public UserRequests(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
    }
}
