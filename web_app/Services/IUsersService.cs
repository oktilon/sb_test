using System.Collections.Generic;
using System.Threading.Tasks;
using web_app.Models;

namespace web_app.Services
{
    public interface IUsersService
    {
        public Task<List<User>> GetAsync();

        public Task<User?> GetAsync(string id);

        public Task CreateAsync(User newUser);

        public Task UpdateAsync(string id, User updatedUser);

        public Task RemoveAsync(string id);
    }
}
