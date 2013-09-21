using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StackOverMongo.Core.MongoModel
{
    public class Tag
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        public string Name { get; set; }
        public long Count { get; set; }
    }
}