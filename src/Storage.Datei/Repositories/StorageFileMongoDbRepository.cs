using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Internal.DecisionTree;
using Microsoft.Framework.ConfigurationModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Storage.Common.Services;
using Storage.Datei.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Repositories
{
	public class StorageFileMongoDbRepository : MongoDbRepositoryBase<StorageFile>, IStorageFileRepository
	{
		private readonly RandomStringGenerator _randomStringGenerator;
		private readonly IConfiguration _configuration;

		public StorageFileMongoDbRepository(IMongoDatabase db, RandomStringGenerator randomStringGenerator, IConfiguration configuration):base(db)
		{
			_randomStringGenerator = randomStringGenerator;
			_configuration = configuration;
			CreateIndexes();
		}

		private void CreateIndexes()
		{
			Collection.Indexes.CreateOneAsync(
				new BsonDocumentIndexKeysDefinition<StorageFile>(BsonDocument.Parse("{ShortUrl:1}")),
				new CreateIndexOptions {Unique = true});
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

		private IFindFluent<StorageFile,StorageFile> FindWithShortUrl(string shortUrl)
		{
			return Collection.Find(item => item.ShortUrl == shortUrl);
		}

		private async Task<string> GenerateUniqueShortUrl()
		{
			var shortUrl = _randomStringGenerator.GenerateBase64Url(_configuration.Get<int>("Configuration:ShortUrlLength"));
			if (await DoesShortUrlExistAsync(shortUrl))
			{
				return await GenerateUniqueShortUrl();
			}
			return shortUrl;
		}
	}
}