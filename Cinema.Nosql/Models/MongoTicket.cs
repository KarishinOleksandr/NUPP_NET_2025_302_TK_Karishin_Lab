using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cinema.Nosql.Models
{
    public class MongoTicket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid OriginalId { get; set; }

        [BsonElement("movieId")]
        public string MovieId { get; set; }

        [BsonElement("showTime")]
        public DateTime ShowTime { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("customerId")]
        public string CustomerId { get; set; }
    }
}