using Cinema.Nosql.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Cinema.Nosql
{
    public class MongoDbInitializer
    {
        private readonly IMongoDatabase _database;

        public MongoDbInitializer(IOptions<MongoDbSettings> settings)
        {
            SetupSerializers();

            var clientSettings = MongoClientSettings.FromConnectionString(settings.Value.ConnectionString);
            clientSettings.ConnectTimeout = TimeSpan.FromSeconds(30);
            clientSettings.ServerSelectionTimeout = TimeSpan.FromSeconds(30);

            var client = new MongoClient(clientSettings);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        private static void SetupSerializers()
        {
            if (BsonSerializer.LookupSerializer<Guid>() is GuidSerializer)
                return;

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        }

        public IMongoRepository<T> GetRepository<T>(string collectionName)
        {
            return new MongoRepository<T>(_database, collectionName);
        }
    }
}