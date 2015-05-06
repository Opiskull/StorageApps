using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Storage.Common.Interfaces;
using Storage.Common.Services;
using Storage.Datei.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Repositories
{
    public class StorageFileRepository : MongoDbShortUrlRepositoryBase<StorageFile>, IStorageFileRepository
    {
        private readonly IMongoDbCrudRepository<StorageFile> _crudRepository;

        public StorageFileRepository(IMongoDatabase db, ShortUrlGenerator shortUrlGenerator,
            IMongoDbCrudRepository<StorageFile> crudRepository)
            : base(db, shortUrlGenerator)
        {
            _crudRepository = crudRepository;
        }

        public override Task<StorageFile> CreateWithShortUrlAsync(StorageFile item)
        {
            item.Created = DateTime.Now;
            return base.CreateWithShortUrlAsync(item);
        }

        public Task<IEnumerable<StorageFile>> GetAllAsync()
        {
            return _crudRepository.GetAllAsync();
        }

        public Task<StorageFile> GetAsync(ObjectId id)
        {
            return _crudRepository.GetAsync(id);
        }

        public Task UpdateAsync(ObjectId id, StorageFile item)
        {
            return _crudRepository.UpdateAsync(id, item);
        }

        public Task DeleteAsync(ObjectId id)
        {
            return _crudRepository.DeleteAsync(id);
        }
    }
}