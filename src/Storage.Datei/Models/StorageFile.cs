using MongoDB.Bson;
using Storage.Common.Interfaces;
using Storage.Common.Services;

namespace Storage.Datei.Models
{
    [CollectionName("storageFile")]
    public class StorageFile : IMongoDbEntity
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public string ShortUrl { get; set; }
        public ObjectId Id { get; set; }
    }
}