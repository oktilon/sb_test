using web_app.Models;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace web_app.Services
{
    public class UsersService : IUsersService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UsersService(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(
                configuration["MongoDB:ConnectionString"]);

            var mongoDatabase = mongoClient.GetDatabase(
                configuration["MongoDB:DatabaseName"]);

            _usersCollection = mongoDatabase.GetCollection<User>(
                configuration["MongoDB:UsersTable"]);
        }

        public async Task<List<User>> GetAsync() =>
            await _usersCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(string id) =>
            await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await _usersCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, User updatedUser) =>
            await _usersCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _usersCollection.DeleteOneAsync(x => x.Id == id);
    }
}
