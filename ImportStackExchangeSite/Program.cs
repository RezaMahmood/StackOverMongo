using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using StackOverMongo.Core.MongoModel;
using MappingConfiguration = StackOverMongo.Import.Mapping.Configuration;


namespace StackOverMongo.Import
{
    class Program
    {
        static void Main()
        {
            ImportToMongo();
        }

        private static void ImportToMongo()
        {
            var sw = new Stopwatch();
            sw.Start();

            MappingConfiguration.Configure();

            var mongoDbName = ConfigurationManager.AppSettings["mongoDbName"];
            var connectionString = ConfigurationManager.AppSettings["mongoConnectionString"];
            var mongoDatabase = GetMongoDatabase(mongoDbName, connectionString);
            var xmlDirectory = ConfigurationManager.AppSettings["xmlDirectory"];

            var xmlPosts = GetAllXmlPosts(xmlDirectory);
            var xmlUsers = GetAllXmlUsers(xmlDirectory);
            var xmlComments = GetAllXmlComments(xmlDirectory);

            InsertUsers(mongoDatabase, xmlUsers);
            InsertPosts(mongoDatabase, xmlPosts);
            InsertComments(mongoDatabase, xmlComments);
            
            sw.Stop();
            var result = sw.Elapsed;
            Console.WriteLine("Time to import: {0}:{1}:{2}", result.Hours, result.Minutes, result.Seconds);
            Console.ReadLine();
        }

        private static void InsertComments(MongoDatabase mongoDatabase, IEnumerable<commentsRow> xmlcomments)
        {
            var commentsCollection = mongoDatabase.GetCollection<Comment>("comments");
            foreach (var xmlcomment in xmlcomments)
            {
                Console.WriteLine("Adding comment: {0}", xmlcomment.Text);
                var comment = AutoMapper.Mapper.Map<commentsRow, Comment>(xmlcomment);
                comment.MongoId = ObjectId.GenerateNewId();
                commentsCollection.Insert(comment);
            }
        }

        private static void InsertPosts(MongoDatabase mongoDatabase, IEnumerable<postsRow> xmlposts)
        {
            var postCollection = mongoDatabase.GetCollection<Post>("posts");
            foreach (var xmlpost in xmlposts)
            {
                Console.WriteLine("Adding post: {0}", xmlpost.Title);
                var post = AutoMapper.Mapper.Map<postsRow, Post>(xmlpost);
                post.TagList = GetPostTagList(post);
                post.MongoId = ObjectId.GenerateNewId();
                postCollection.Insert(post);
            }
        }

        private static List<string> GetPostTagList(Post post)
        {
            var tags = post.Tags;
            var tagList = new List<string>();

            if (!string.IsNullOrEmpty(tags))
            {
                var regex = new Regex(@"<([^<]+?)>", RegexOptions.IgnoreCase);
                var matches = regex.Matches(tags);

                if (matches.Count > 0)
                {

                    foreach (Match match in matches)
                    {
                        tagList.Add(match.Value);
                    }
                }
            }

            return tagList;
        }

        private static void InsertUsers(MongoDatabase mongoDatabase, IEnumerable<usersRow> xmlusers)
        {
            var userCollection = mongoDatabase.GetCollection<User>("users");
            foreach (var xmluser in xmlusers)
            {
                Console.WriteLine("Adding User: {0}", xmluser.DisplayName);
                var user = AutoMapper.Mapper.Map<usersRow, User>(xmluser);
                user.MongoId = ObjectId.GenerateNewId();
                userCollection.Insert(user);
            }
        }

        private static MongoDatabase GetMongoDatabase(string mongoDbName, string connectionstring)
        {
            
            var client = new MongoClient(connectionstring);
            var server = client.GetServer();
            var database = server.GetDatabase(mongoDbName);

            return database;
        }
        
        private static IEnumerable<commentsRow> GetAllXmlComments(string stackDirectory)
        {
            var commentFile = Path.Combine(stackDirectory, "comments.xml");
            var allXmlComments = StreamTypeFromXml<commentsRow>(commentFile, "row", element => new commentsRow
                {
                    CreationDate = element.GetAttributeStringValue("CreationDate"),
                    Id = element.GetAttributeStringValue("Id"),
                    PostId = element.GetAttributeStringValue("PostId"),
                    Score = element.GetAttributeStringValue("Score"),
                    Text = element.GetAttributeStringValue("Text"),
                    UserDisplayName = element.GetAttributeStringValue("UserDisplayName"),
                    UserId = element.GetAttributeStringValue("UserId")
                });

            return allXmlComments;
        }

        private static IEnumerable<usersRow> GetAllXmlUsers(string stackDirectory)
        {
            var userFile = Path.Combine(stackDirectory, "Users.xml");
            var allXmlUsers = StreamTypeFromXml<usersRow>(userFile, "row", element => new usersRow
                {
                    AboutMe = element.GetAttributeStringValue("AboutMe"),
                    CreationDate = element.GetAttributeStringValue("CreationDate"),
                    Age = element.GetAttributeStringValue("Age"),
                    DisplayName = element.GetAttributeStringValue("DisplayName"),
                    DownVotes = element.GetAttributeStringValue("DownVotes"),
                    Id = element.GetAttributeStringValue("Id"),
                    LastAccessDate = element.GetAttributeStringValue("LastAccessDate"),
                    Location = element.GetAttributeStringValue("Location"),
                    ProfileImageUrl = element.GetAttributeStringValue("ProfileImageUrl"),
                    Reputation = element.GetAttributeStringValue("Reputation"),
                    UpVotes = element.GetAttributeStringValue("UpVotes"),
                    Views = element.GetAttributeStringValue("Views"),
                    WebsiteUrl = element.GetAttributeStringValue("WebsiteUrl")
                });

            return allXmlUsers;
        }

        private static IEnumerable<postsRow> GetAllXmlPosts(string stackDirectory)
        {
            var postFile = Path.Combine(stackDirectory, "Posts.xml");

            var allXmlPosts = StreamTypeFromXml<postsRow>(postFile, "row", element => new postsRow
                {
                    AnswerCount = element.GetAttributeStringValue("AnswerCount"),
                    Body = element.GetAttributeStringValue("Body"),
                    CreationDate = element.GetAttributeStringValue("CreationDate"),
                    FavoriteCount = element.GetAttributeStringValue("FavoriteCount"),
                    LastEditDate = element.GetAttributeStringValue("LastEditDate"),
                    OwnerUserId = element.GetAttributeStringValue("OwnerUserId"),
                    Title = element.GetAttributeStringValue("Title"),
                    Tags = element.GetAttributeStringValue("Tags"),
                    Score = element.GetAttributeStringValue("Score"),
                    Id = element.GetAttributeStringValue("Id"),
                    AcceptedAnswerId = element.GetAttributeStringValue("AcceptedAnswerId"),
                    ClosedDate = element.GetAttributeStringValue("ClosedDate"),
                    CommentCount = element.GetAttributeStringValue("CommentCount"),
                    CommunityOwnedDate = element.GetAttributeStringValue("CommunityOwnedDate"),
                    LastActivityDate = element.GetAttributeStringValue("LastActivityDate"),
                    LastEditorDisplayName = element.GetAttributeStringValue("LastEditorDisplayName"),
                    LastEditorUserId = element.GetAttributeStringValue("LastEditorUserId"),
                    OwnerDisplayName = element.GetAttributeStringValue("OwnerDisplayName"),
                    ParentId = element.GetAttributeStringValue("ParentId"),
                    PostTypeId = element.GetAttributeStringValue("PostTypeId"),
                    ViewCount = element.GetAttributeStringValue("ViewCount")
                });

            return allXmlPosts;
        }
        
        private static IEnumerable<T> StreamTypeFromXml<T>(string stackDirectory, string elementName, Func<XElement, T> converter) where T : class
        {
            using (XmlReader reader = XmlReader.Create(stackDirectory))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == elementName)
                        {
                            var element = XElement.ReadFrom(reader) as XElement;
                            
                            if (element != null)
                            {
                                yield return converter.Invoke(element);
                            }
                        }
                    }
                }
                reader.Close();
            }
        }
        
    }
}
