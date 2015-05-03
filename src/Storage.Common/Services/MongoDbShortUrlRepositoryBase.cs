using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Storage.Common.Interfaces;

namespace Storage.Common.Services
{
    public class MongoDbShortUrlRepositoryBase<T> : MongoDbRepositoryBase<T>, IMongoDbShortUrlRepository<T>
        where T : class, IMongoDbEntity, IShortUrlEntity
    {
        private const string SHORTURL_INDEX = "{ShortUrl:1}";
        private readonly ShortUrlGenerator _shortUrlGenerator;

        public MongoDbShortUrlRepositoryBase(IMongoDatabase db, ShortUrlGenerator shortUrlGenerator) : base(db)
        {
            _shortUrlGenerator = shortUrlGenerator;
            CreateIndexes();
        }

        public Task<T> GetWithShortUrlAsync(string shortUrl)
        {
            return FindWithShortUrl(shortUrl).FirstOrDefaultAsync();
        }

        public async Task<T> CreateWithShortUrlAsync(T item)
        {
            item.ShortUrl = await GenerateUniqueShortUrl();
            return await CreateAsync(item);
        }

        public async Task<bool> DoesShortUrlExistAsync(string shortUrl)
        {
            var count = await FindWithShortUrl(shortUrl).CountAsync();
            return count > 1;
        }

        private IFindFluent<T, T> FindWithShortUrl(string shortUrl)
        {
            return Collection.Find(item => item.ShortUrl == shortUrl);
        }

        private async Task<string> GenerateUniqueShortUrl()
        {
            var shortUrl = _shortUrlGenerator.Generate();
            if (await DoesShortUrlExistAsync(shortUrl))
            {
                return await GenerateUniqueShortUrl();
            }
            return shortUrl;
        }

        private void CreateIndexes()
        {
            Collection.Indexes.CreateOneAsync(
                new BsonDocumentIndexKeysDefinition<T>(BsonDocument.Parse(SHORTURL_INDEX)),
                new CreateIndexOptions {Unique = true});
        }
    }
}