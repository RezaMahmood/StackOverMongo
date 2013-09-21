using System;
using StackOverMongo.Core;
using AutoMapper;
using StackOverMongo.Core.MongoModel;

namespace StackOverMongo.Import.Mapping
{
    internal static class Configuration
    {
        internal static void Configure()
        {
            Mapper.CreateMap<usersRow, User>();
            Mapper.CreateMap<postsRow, Post>();
            Mapper.CreateMap<commentsRow, Comment>();
            Mapper.CreateMap<string, int>().ConvertUsing(new IntegerTypeConverter());
            Mapper.CreateMap<string, DateTime?>().ConvertUsing(new DateTimeTypeConverter());
        }
    }
}
