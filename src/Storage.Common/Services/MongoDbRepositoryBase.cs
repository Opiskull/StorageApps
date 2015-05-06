using MongoDB.Driver;
using Storage.Common.Extensions;
using Storage.Common.Interfaces;

namespace Storage.Common.Services
{
    public class MongoDbRepositoryBase<T> : IMongoDbRepository<T> where T : class, IMongoDbEntity
    {
        public MongoDbRepositoryBase(IMongoDatabase db)
        {
            Collection =
                db.GetCollection<T>(typeof (T).ForceCustomAttribute<MongoDbCollectionNameAttribute>().CollectionName);
        }

        public IMongoCollection<T> Collection { get; set; }
    }
}