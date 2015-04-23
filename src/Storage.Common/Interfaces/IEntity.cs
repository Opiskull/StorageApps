using System;
using MongoDB.Bson;

namespace Storage.Common.Interfaces
{
    public interface IEntity
    {
		ObjectId Id { get; set; }
    }
}