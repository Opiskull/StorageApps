﻿using System;
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
        protected readonly IMongoCollection<T> Collection;

        public MongoDbRepositoryBase(IMongoDatabase db)
        {
            Collection =
                db.GetCollection<T>(typeof (T).ForceCustomAttribute<MongoDbCollectionNameAttribute>().CollectionName);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Collection.Find(new BsonDocument()).ToListAsync();
        }

        public virtual Task<T> GetAsync(ObjectId id)
        {
            return Collection.Find(filter => filter.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<T> CreateAsync(T item)
        {
            await Collection.InsertOneAsync(item);
            return item;
        }

        public virtual Task UpdateAsync(ObjectId id, T item)
        {
            return Collection.ReplaceOneAsync(filter => filter.Id == id, item);
        }

        public virtual Task DeleteAsync(ObjectId id)
        {
            return Collection.DeleteOneAsync(item => item.Id == id);
        }
    }
}