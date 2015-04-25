using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Storage.Common.Interfaces;
using Storage.Common.Services;

namespace Storage.Datei.Models
{
	[CollectionName("storageFile")]
	public class StorageFile : IEntity
	{
		[BsonElement("id")]
		public ObjectId Id { get; set; }
		[BsonElement("fileName")]
		public string FileName { get; set; }
		[BsonElement("contentType")]
		public string ContentType { get; set; }
		[BsonElement("size")]
		public long Size { get; set; }
		[BsonElement("shortUrl")]
		public string ShortUrl { get; set; }
	}
}