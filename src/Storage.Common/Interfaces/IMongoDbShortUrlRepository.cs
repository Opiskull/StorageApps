using System.Threading.Tasks;

namespace Storage.Common.Interfaces
{
    public interface IMongoDbShortUrlRepository<T>
    {
        Task<T> GetWithShortUrlAsync(string shortUrl);
    }
}