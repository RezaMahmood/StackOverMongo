using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StackOverMongo.Core.MongoModel
{
    public class Comment
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        [BsonElement("CommentId")]
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
        public string UserDisplayName { get; set; }
    }
}