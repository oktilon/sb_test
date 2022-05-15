using web_app.Models;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using RabbitMQ.Client;
using System.Security.Claims;
using System;

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

        public List<User> GetUsers()
        {
            return _usersCollection.Find(_ => true).ToList();
        }

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

        public string GenerateJwtToken(AuthUserDTO user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSecret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                     new Claim("id", user.Id.ToString()),
                     new Claim("name", user.Name)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public AuthUserDTO loginUser(AuthUser authUser)
        {
            if (authUser.Username == _configuration["AdminLogin"] && authUser.Password == _configuration["AdminPass"])
            {
                var user = new AuthUserDTO(authUser);
                user.Token = GenerateJwtToken(user);
                return user;
            }
            return null;
        }
    }
}
