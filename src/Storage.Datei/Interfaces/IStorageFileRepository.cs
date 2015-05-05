using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Storage.Common.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Interfaces
{
    public interface IStorageFileRepository : IMongoDbShortUrlRepository<StorageFile>
    {
        Task<IEnumerable<StorageFile>> GetAllAsync();
        Task<StorageFile> GetAsync(ObjectId id);
        Task UpdateAsync(ObjectId id, StorageFile item);
        Task DeleteAsync(ObjectId id);
    }
}