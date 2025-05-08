using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Nosql.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Cinema.Nosql
{
    public class MongoDbInitializer
    {
        private readonly IMongoDatabase _database;

        public MongoDbInitializer(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoRepository<T> GetRepository<T>(string collectionName)
        {
            return new MongoRepository<T>(_database, collectionName);
        }
    }
}