using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Storage.Common.Interfaces;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Storage.Common.Extensions;

namespace Storage.Common.Services
{
	public class RepositoryBase<T> : IRepository<T> where T : class, IEntity
	{
		private readonly IMongoCollection<T> _collection;
		
		public RepositoryBase(IMongoDatabase db)
		{
			_collection = db.GetCollection<T>(typeof(T).ForceCustomAttribute<CollectionNameAttribute>().CollectionName);
		} 
		public RepositoryBase(IMongoDatabase db, string collectionName)
		{
			_collection = db.GetCollection<T>(collectionName);
		} 

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _collection.Find(filter => filter.Id != ObjectId.Empty).ToListAsync();
		}

		public Task<T> GetAsync(ObjectId id)
		{
			return _collection.Find(filter => filter.Id == id).FirstOrDefaultAsync();
		}

		public async Task<T> CreateAsync(T item)
		{
			await _collection.InsertOneAsync(item);
			return item;
		}

		public Task UpdateAsync(ObjectId id, T item)
		{
			return _collection.ReplaceOneAsync(filter => filter.Id == id, item);
		}

		public Task DeleteAsync(ObjectId id)
		{
			return _collection.DeleteOneAsync(item => item.Id == id);
		}
	}
}