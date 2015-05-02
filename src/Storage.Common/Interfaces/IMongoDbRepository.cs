using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Storage.Common.Interfaces
{
    public interface IMongoDbRepository<T> where T : class, IMongoDbEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(ObjectId id);
        Task<T> CreateAsync(T item);
        Task UpdateAsync(ObjectId id, T item);
        Task DeleteAsync(ObjectId id);
    }
}