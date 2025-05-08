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

        [BsonRepresentation(BsonType.String)]
        public Guid OriginalId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("genre")]
        public string Genre { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("rating")]
        public double Rating { get; set; }
    }
}