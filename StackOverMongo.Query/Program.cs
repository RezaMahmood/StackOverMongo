using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using StackOverMongo.Core.MongoModel;
using QueryBuilder = MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using StackOverMongo.Core;

namespace StackOverMongo.Query
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionstring = "mongodb://mongouseradmin:mongouseradminpassword@rezamongodb1.cloudapp.net:27017";
            var client = new MongoClient(connectionstring);
            var server = client.GetServer();
            var database = server.GetDatabase("mathoverflow");
            var postsCollection = database.GetCollection<Post>("posts");
            var tagsCollection = database.GetCollection<Tag>("tags");

            var distinctTags = new List<string>();
            var tags = GetTags(postsCollection);

            foreach(var tag in tags)
            {
                var regex = new Regex(@"<([^<]+?)>", RegexOptions.IgnoreCase);
                var matches = regex.Matches(tag);
                if(matches.Count > 0)
                {
                    foreach(Match match in matches)
                    {
                        if(distinctTags.Any(x=>x==match.Value))
                        {
                            continue;
                        }
                        var tagName = match.Value;
                        IMongoQuery query = QueryBuilder.Query<Post>.EQ(q=>q.Tags, tagName);
                        var tagCount = postsCollection.Find(query).Count();
                        distinctTags.Add(match.Value);
                        Console.WriteLine("Adding {0} with count {1}", tagName, tagCount);
                        tagsCollection.Insert(new Tag{MongoId = ObjectId.GenerateNewId(), Name = tagName, Count = tagCount});
                    }
                }
            }

            Console.WriteLine("Number of Tags: {0}", distinctTags.Count());
        }

        private static IEnumerable<string> GetTags(MongoCollection<Post> postsCollection)
        {
            var allTags = postsCollection.FindAll().Select(x=>x.Tags);

            foreach(var tag in allTags)
            {
                yield return tag;
            }

        }
    }
}
