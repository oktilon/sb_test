using web_app.Models;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace web_app.Services
{
    public class UsersService : IUsersService
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IConfiguration _configuration;

        public UsersService(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(
                configuration["MongoDB:ConnectionString"]);

            var mongoDatabase = mongoClient.GetDatabase(
                configuration["MongoDB:DatabaseName"]);

            _usersCollection = mongoDatabase.GetCollection<User>(
                configuration["MongoDB:UsersTable"]);

            _configuration = configuration;
        }

        public async Task<List<User>> GetAsync() =>
            await _usersCollection.Find(_ => true).ToListAsync();

        public void addUser(UserDTO newUser)
        {
            var json = JsonSerializer.Serialize(newUser);
            var queue = _configuration["RabbitMQ:Queue"];
            var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQ:Host"] };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                channel.BasicPublish(exchange: "",
                    routingKey: queue,
                    basicProperties: null,
                    body: Encoding.UTF8.GetBytes(json));
            }
        }

        public bool loginUser(AuthUser authUser)
        {
            return authUser.UserName == _configuration["AdminLogin"]
                   && authUser.Password == _configuration["AdminPass"];
        }
    }
}
