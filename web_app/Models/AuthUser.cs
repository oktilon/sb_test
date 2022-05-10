namespace web_app.Models
{
    public class AuthUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public AuthUser(string name, string password)
        {
            UserName = name;
            Password = password;
        }
    }
}
