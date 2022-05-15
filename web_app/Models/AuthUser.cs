namespace web_app.Models
{
    public class AuthUser
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public AuthUser(string name, string password)
        {
            Username = name;
            Password = password;
        }
    }
}
