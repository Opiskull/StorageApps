using System.Threading.Tasks;

namespace Storage.Common.Interfaces
{
    public interface IMongoDbShortUrlRepository<T> : IMongoDbRepository<T>
        where T : class, IShortUrlEntity, IMongoDbEntity
    {
        Task<T> GetWithShortUrlAsync(string shortUrl);
        Task<bool> DoesShortUrlExistAsync(string shortUrl);
        Task<T> CreateWithShortUrlAsync(T item);
    }
}