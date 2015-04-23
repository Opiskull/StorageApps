using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using Storage.Common.Interfaces;
using Storage.Common.Services;

namespace Storage.Datei.Models
{
	[CollectionName("storageFile")]
	public class StorageFile : IEntity
	{
		public ObjectId Id { get; set; }
		public string FileName { get; set; }
		public string ContentType { get; set; }
		public long Size { get; set; }
	}
}