using MongoDB.Driver;
using Storage.Common.Services;
using Storage.Datei.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Repositories
{
    public class BlogPostRepository : MongoDbRepositoryBase<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(IMongoDatabase db)
            : base(db)
        {
        }
    }
}