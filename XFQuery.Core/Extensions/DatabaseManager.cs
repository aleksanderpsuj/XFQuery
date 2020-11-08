using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using XFQuery.Core.Configuration;

namespace XFQuery.Core.Extensions
{
    public class DatabaseManager
    {
        public DatabaseManager(XfQueryConfig config)
        {
            string GetConfig()
            {
                return config.Database.ConnectionString == string.Empty
                    ? $"mongodb+srv://{config.Database.Login}:{config.Database.Password}@{config.Database.Host}:{config.Database.Port}/{config.Database.Database}?authSource=admin&readPreference=primary&appname=XFQuery"
                    : config.Database.ConnectionString;
            }

            var mongoClient = new MongoClient(GetConfig());
            mongoClient.StartSession();
            var mongoDatabase = mongoClient.GetDatabase($"{config.Database.Database}");
            MongoDatabase = mongoDatabase;
        }

        private static IMongoDatabase MongoDatabase { get; set; }

        public static List<BsonDocument> GetAllFromDb(string collection)
        {
            var mongoCollection = MongoDatabase.GetCollection<BsonDocument>(collection);
            var documents = mongoCollection.Find(new BsonDocument()).ToList();

            return documents;
        }

        public static BsonDocument GetFromDb(string collection, string filterfield, string valuefield)
        {
            var mongoCollection = MongoDatabase.GetCollection<BsonDocument>(collection);
            var filter = Builders<BsonDocument>.Filter.Eq(filterfield, valuefield);
            var document = mongoCollection.Find(filter).FirstOrDefault();

            return document;
        }

        public static bool InsertIntoDb(string collection, BsonDocument data)
        {
            var mongoCollection = MongoDatabase.GetCollection<BsonDocument>(collection);
            var insertstatIsCompleted = mongoCollection.InsertOneAsync(data).IsCompletedSuccessfully;

            return insertstatIsCompleted;
        }

        public static long DeleteFromDb(string collection, string filterfield, string valuefield)
        {
            var mongoCollection = MongoDatabase.GetCollection<BsonDocument>(collection);
            var filter = Builders<BsonDocument>.Filter.Eq(filterfield, valuefield);
            var deletedCount = mongoCollection.DeleteOneAsync(filter).Result.DeletedCount;

            return deletedCount;
        }

        public static bool UpdateFromDb(string collection, string filterfield, string valuefield, string updatefield,
            string updatevalue)
        {
            var mongoCollection = MongoDatabase.GetCollection<BsonDocument>(collection);
            var filter = Builders<BsonDocument>.Filter.Eq(filterfield, valuefield);
            var update = Builders<BsonDocument>.Update.Set(updatefield, updatevalue);
            var isAcknowledged = mongoCollection.UpdateOneAsync(filter, update).Result.IsAcknowledged;

            return isAcknowledged;
        }
    }
}