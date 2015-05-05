using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Storage.Common.Extensions;
using Storage.Common.Interfaces;

namespace Storage.Common.Services
{
    public class MongoDbRepositoryBase<T> : IMongoDbRepository<T> where T : class, IMongoDbEntity
    {
        public IMongoCollection<T> Collection { get; set; }

        public MongoDbRepositoryBase(IMongoDatabase db)
        {
            Collection =
                db.GetCollection<T>(typeof (T).ForceCustomAttribute<MongoDbCollectionNameAttribute>().CollectionName);
        }
    }
}