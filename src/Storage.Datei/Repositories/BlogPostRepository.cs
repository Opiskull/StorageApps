using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Internal.DecisionTree;
using MongoDB.Bson;
using MongoDB.Driver;
using Storage.Common.Services;
using Storage.Datei.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Repositories
{
    public class BlogPostRepository : MongoDbRepositoryBase<BlogPost>, IBlogPostRepository
    {
        private readonly ShortUrlGenerator _shortUrlGenerator;

        public BlogPostRepository(IMongoDatabase db, ShortUrlGenerator shortUrlGenerator)
            : base(db)
        {
            _shortUrlGenerator = shortUrlGenerator;
        }

        public override Task<BlogPost> CreateAsync(BlogPost item)
        {
            item.Created = DateTime.Now;
            item.PermLink = _shortUrlGenerator.Generate(5);
            return base.CreateAsync(item);
        }

        public Task<List<BlogPost>> GetLatestAsync(int? count, int? skip)
        {
            return Collection
                .Find(new BsonDocument())
                .SortBy(post => post.Created)
                .Skip(skip)
                .Limit(count)
                .ToListAsync();
        }

        public Task<BlogPost> GetWithPermLinkAsync(string permlink)
        {
            return Collection
                .Find(post => post.PermLink == permlink)
                .SingleOrDefaultAsync();
        }
    }
}