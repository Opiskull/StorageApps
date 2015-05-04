using System;
using MongoDB.Bson;

namespace Storage.Common.Interfaces
{
    public interface IMongoDbEntity
    {
        ObjectId Id { get; set; }
    }
}