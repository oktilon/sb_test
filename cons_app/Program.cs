using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using cons_app.Models;

namespace cons_app
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration cfg = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var rabbit = cfg.GetSection("RabbitMQ");
            var mongo = cfg.GetSection("MongoDB");
            Console.WriteLine("Config RabbitMQ: host={0}, queue={1}", rabbit["Host"], rabbit["Queue"]);
            Console.WriteLine("Config MongoDB: host={0}, db={1}, tbl={2}", mongo["ConnectionString"], mongo["DatabaseName"], mongo["UsersTable"]);

            var mongoClient = new MongoClient(mongo["ConnectionString"]);

            var mongoDatabase = mongoClient.GetDatabase(mongo["DatabaseName"]);

            var usersCollection = mongoDatabase.GetCollection<User>(mongo["UsersTable"]);

            try
            {
                var factory = new ConnectionFactory() { HostName = rabbit["Host"] };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: rabbit["Queue"],
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);

                        var dto = JsonConvert.DeserializeObject<UserDTO>(message);
                        var user = new User(dto);
                        usersCollection.InsertOne(user);
                    };
                    channel.BasicConsume(queue: rabbit["Queue"],
                        autoAck: true,
                        consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }
        }
    }
}
