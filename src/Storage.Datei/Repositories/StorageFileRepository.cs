using MongoDB.Driver;
using Storage.Common.Services;
using Storage.Datei.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Repositories
{
    public class StorageFileRepository : MongoDbShortUrlRepositoryBase<StorageFile>, IStorageFileRepository
    {
        public StorageFileRepository(IMongoDatabase db, ShortUrlGenerator shortUrlGenerator)
            : base(db, shortUrlGenerator)
        {
        }
    }
}