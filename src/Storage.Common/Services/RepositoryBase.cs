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
		protected readonly IMongoCollection<T> Collection;
		
		public RepositoryBase(IMongoDatabase db)
		{
			Collection = db.GetCollection<T>(typeof(T).ForceCustomAttribute<CollectionNameAttribute>().CollectionName);
		} 
		public RepositoryBase(IMongoDatabase db, string collectionName)
		{
			Collection = db.GetCollection<T>(collectionName);
		} 

		public virtual async Task<IEnumerable<T>> GetAllAsync()
		{
			return await Collection.Find(filter => filter.Id != ObjectId.Empty).ToListAsync();
		}

		public virtual Task<T> GetAsync(ObjectId id)
		{
			return Collection.Find(filter => filter.Id == id).FirstOrDefaultAsync();
		}

		public virtual async Task<T> CreateAsync(T item)
		{
			await Collection.InsertOneAsync(item);
			return item;
		}

		public virtual Task UpdateAsync(ObjectId id, T item)
		{
			return Collection.ReplaceOneAsync(filter => filter.Id == id, item);
		}

		public virtual Task DeleteAsync(ObjectId id)
		{
			return Collection.DeleteOneAsync(item => item.Id == id);
		}
	}
}