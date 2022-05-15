using System.Collections.Generic;
using System.Threading.Tasks;
using web_app.Models;

namespace web_app.Services
{
    public interface IUsersService
    {
        public List<User> GetUsers();
        public void addUser(UserDTO newUser);
    }
}
