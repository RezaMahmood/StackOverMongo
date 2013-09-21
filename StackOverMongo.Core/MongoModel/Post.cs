using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StackOverMongo.Core.MongoModel
{
    public class Post
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        [BsonElement("PostId")]
        public int Id { get; set; }
        public int PostTypeId { get; set; }
        public int AcceptedAnswerId { get; set; }
        public DateTime CreationDate { get; set; }
        public int Score { get; set; }
        public int ViewCount { get; set; }
        public string Body { get; set; }
        public int OwnerUserId { get; set; }
        public int LastEditorUserId { get; set; }
        public DateTime? LastEditDate { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public int AnswerCount { get; set; }
        public int FavoriteCount { get; set; }
        public string ParentId { get; set; }
        public DateTime? CommunityOwnedDate { get; set; }
        public string LastEditorDisplayName { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string OwnerDisplayName { get; set; }
        public Comment[] Comments { get; set; }
        public List<string> TagList { get; set; }
        public List<Post> Answers { get; set; } 
    }
}
