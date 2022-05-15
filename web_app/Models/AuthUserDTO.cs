namespace web_app.Models
{
    public class AuthUserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }

        public AuthUserDTO()
        {
            Id = 0;
            Name = "";
            Token = "";
        }

        public AuthUserDTO(AuthUser user)
        {
            Id = 1;
            Name = user.Username;
            Token = "JWT.Token";
        }
    }
}
