using System;
using MongoDB.Bson;
using Storage.Common.Interfaces;
using Storage.Common.Services;

namespace Storage.Datei.Models
{
    [MongoDbCollectionName("storageFile")]
    public class StorageFile : IMongoDbEntity, IShortUrlEntity
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public DateTime Created { get; set; }
        public ObjectId Id { get; set; }
        public string ShortUrl { get; set; }
    }
}