using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cinema.Nosql.Models
{
    public class MongoMovie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Guid OriginalId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
    }
}
