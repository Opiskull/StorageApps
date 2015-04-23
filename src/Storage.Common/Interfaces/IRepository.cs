using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Storage.Common.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
		Task<IEnumerable<T>> GetAllAsync();
		Task<T> GetAsync(ObjectId id);
		Task<T> CreateAsync(T item);
		Task UpdateAsync(ObjectId id, T item);
		Task DeleteAsync(ObjectId id);
    }
}