﻿using System.Threading.Tasks;
using Storage.Common.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Repositories
{
	public interface IStorageFileRepository : IRepository<StorageFile>
	{
		Task<StorageFile> GetWithShortUrlAsync(string shortUrl);
		Task<bool> DoesShortUrlExistAsync(string shortUrl);
	}
}