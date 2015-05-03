using System.Threading.Tasks;

namespace Storage.Common.Interfaces
{
    public interface IMongoDbShortUrlRepository<T>
    {
        Task<T> GetWithShortUrlAsync(string shortUrl);
        Task<bool> DoesShortUrlExistAsync(string shortUrl);
        Task<T> CreateWithShortUrlAsync(T item);
    }
}