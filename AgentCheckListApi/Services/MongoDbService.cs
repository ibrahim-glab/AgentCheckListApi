// configure MongoDb
// c:/Users/Bebo/source/repos/AgentCheckListApi/AgentCheckListApi/Services/MongoDbService.cs
using AgentCheckListApi.Helper;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AgentCheckListApi.Services
{
    public class MongoDbService<T> where T : class
    {
        private readonly IMongoDatabase _database;
        
        public MongoDbService(IOptions<MongoDb> configuration)
        {
            var client = new MongoClient(configuration.Value.ConnectionString);
            _database = client.GetDatabase(configuration.Value.Database);

        }
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        //Get All Data
        public List<T> GetAllData()
        {
            var collection = GetCollection<T>(typeof(T).Name);
            return collection.Find(new BsonDocument()).ToList();
        }

        
    }
}