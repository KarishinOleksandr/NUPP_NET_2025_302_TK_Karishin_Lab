using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cinema.Nosql.Models
{
    public class MongoCustomer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Guid OriginalId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("favoriteGenre")]
        public string FavoriteGenre { get; set; }

        [BsonElement("tickets")]
        public List<string> TicketIds { get; set; } = new List<string>();

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [BsonElement("updatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
