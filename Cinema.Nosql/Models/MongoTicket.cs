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

        public Guid OriginalId { get; set; }
        public string MovieId { get; set; } 
        public DateTime ShowTime { get; set; }
        public double Price { get; set; }
        public string CustomerId { get; set; }
    }
}