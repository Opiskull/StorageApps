using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using Storage.Common.Interfaces;
using Storage.Common.Services;

namespace Storage.Datei.Models
{
    [MongoDbCollectionName("storageFile")]
    [DataContract]
    public class StorageFile : IMongoDbEntity, IShortUrlEntity, ITaggingEntity
    {
        [DataMember(Name = "fileName")]
        public string FileName { get; set; }
        [DataMember(Name = "contentType")]
        public string ContentType { get; set; }
        [DataMember(Name = "size")]
        public long Size { get; set; }
        [DataMember(Name = "created")]
        public DateTime Created { get; set; }
        [DataMember(Name = "id")]
        public ObjectId Id { get; set; }
        [DataMember(Name = "shortUrl")]
        public string ShortUrl { get; set; }
        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }
    }
}