using System.Collections.Generic;
using System.Threading.Tasks;
using web_app.Models;

namespace web_app.Services
{
    public interface IUsersService
    {
        public Task<List<User>> GetAsync();
        public void addUser(UserDTO newUser);
    }
}
