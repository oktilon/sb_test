using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace web_app.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("CreatedDate")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedDate { get; set; }
    }
}
