using System.Threading.Tasks;
using Microsoft.Framework.ConfigurationModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Storage.Common.Services;
using Storage.Datei.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Repositories
{
    public class StorageFileRepository : MongoDbRepositoryBase<StorageFile>, IStorageFileRepository
    {
        private readonly IConfiguration _configuration;
        private readonly RandomStringGenerator _randomStringGenerator;

        public StorageFileRepository(IMongoDatabase db, RandomStringGenerator randomStringGenerator,
            IConfiguration configuration) : base(db)
        {
            _randomStringGenerator = randomStringGenerator;
            _configuration = configuration;
            CreateIndexes();
        }

        public override async Task<StorageFile> CreateAsync(StorageFile item)
        {
            item.ShortUrl = await GenerateUniqueShortUrl();
            return await base.CreateAsync(item);
        }

        public Task<StorageFile> GetWithShortUrlAsync(string shortUrl)
        {
            return FindWithShortUrl(shortUrl).FirstOrDefaultAsync();
        }

        public async Task<bool> DoesShortUrlExistAsync(string shortUrl)
        {
            var count = await FindWithShortUrl(shortUrl).CountAsync();
            return count > 1;
        }

        private void CreateIndexes()
        {
            Collection.Indexes.CreateOneAsync(
                new BsonDocumentIndexKeysDefinition<StorageFile>(BsonDocument.Parse("{ShortUrl:1}")),
                new CreateIndexOptions {Unique = true});
        }

        private IFindFluent<StorageFile, StorageFile> FindWithShortUrl(string shortUrl)
        {
            return Collection.Find(item => item.ShortUrl == shortUrl);
        }

        private async Task<string> GenerateUniqueShortUrl()
        {
            var shortUrl =
                _randomStringGenerator.GenerateBase64Url(_configuration.Get<int>("Configuration:ShortUrlLength"));
            if (await DoesShortUrlExistAsync(shortUrl))
            {
                return await GenerateUniqueShortUrl();
            }
            return shortUrl;
        }
    }
}