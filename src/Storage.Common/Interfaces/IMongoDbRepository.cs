using MongoDB.Driver;

namespace Storage.Common.Interfaces
{
    public interface IMongoDbRepository<T> where T : class, IMongoDbEntity
    {
        IMongoCollection<T> Collection { get; set; }
    }
}