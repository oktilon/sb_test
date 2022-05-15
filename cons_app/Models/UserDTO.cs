namespace cons_app.Models
{
    public class UserDTO
    {
        public string Name { get; set; }

        public UserDTO(string name = "")
        {
            Name = name;
        }
    }
}
