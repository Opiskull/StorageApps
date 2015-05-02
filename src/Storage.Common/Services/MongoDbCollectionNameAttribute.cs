using System;

namespace Storage.Common.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MongoDbCollectionNameAttribute : Attribute
    {
        public MongoDbCollectionNameAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }

        public string CollectionName { get; set; }
    }
}