using Storage.Common.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Interfaces
{
    public interface IStorageFileRepository : IMongoDbRepository<StorageFile>, IMongoDbShortUrlRepository<StorageFile>
    {
    }
}