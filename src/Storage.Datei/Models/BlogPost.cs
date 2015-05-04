using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Storage.Common.Interfaces;
using Storage.Common.Services;

namespace Storage.Datei.Models
{
    [MongoDbCollectionName("blogPost")]
    [BsonIgnoreExtraElements]
    [DataContract]
    public class BlogPost : IMongoDbEntity
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "body")]
        public string Body { get; set; }

        [DataMember(Name = "id")]
        public ObjectId Id { get; set; }

        [DataMember(Name="permLink")]
        public string PermLink { get; set; }

        [DataMember(Name = "created")]
        public DateTime? Created { get; set; }
    }
}