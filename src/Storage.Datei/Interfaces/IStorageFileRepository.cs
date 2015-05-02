﻿using System.Threading.Tasks;
using Storage.Common.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Interfaces
{
    public interface IStorageFileRepository : IMongoDbRepository<StorageFile>
    {
        Task<StorageFile> GetWithShortUrlAsync(string shortUrl);
        Task<bool> DoesShortUrlExistAsync(string shortUrl);
    }
}