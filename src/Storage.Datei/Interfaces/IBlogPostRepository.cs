using System.Collections.Generic;
using System.Threading.Tasks;
using Storage.Common.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Interfaces
{
    public interface IBlogPostRepository : IMongoDbCrudRepository<BlogPost>
    {
        Task<List<BlogPost>> GetLatestAsync(int? count,int? skip);
        Task<BlogPost> GetWithPermLinkAsync(string permlink);
    }
}