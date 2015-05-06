using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using Storage.Common.Exceptions;
using Storage.Common.Extensions;
using Storage.Common.Interfaces;
using Storage.Datei.Interfaces;
using Storage.Datei.Models;
using Storage.Datei.Services;

namespace Storage.Datei.Controllers
{
    [ApiExplorerSettings(GroupName = "Files")]
    [Route("api/v1/[controller]")]
    public class FilesController : Controller
    {
        private readonly FileManager _fileManager;
        private readonly ILogger _logger;
        private readonly IConverter<IFormFile, StorageFile> _storageFileConverter;
        private readonly IStorageFileRepository _storageFileRepository;

        public FilesController(ILoggerFactory logFactory, IStorageFileRepository storageFileRepository,
            FileManager fileManager, IConverter<IFormFile, StorageFile> storageFileConverter)
        {
            _logger = logFactory.CreateLogger<FilesController>();
            _storageFileRepository = storageFileRepository;
            _fileManager = fileManager;
            _storageFileConverter = storageFileConverter;
        }

        private async Task<StorageFile> GetFileWithShortUrlAsync(string shortUrl)
        {
            var storageFile = await _storageFileRepository.GetWithShortUrlAsync(shortUrl);
            if (storageFile == null) throw new ItemNotFoundException();
            return storageFile;
        }

        [HttpGet("{shortUrl}/info")]
        public async Task<StorageFile> Info(string shortUrl)
        {
            _logger.LogDebug("Info");
            var storageFile = await GetFileWithShortUrlAsync(shortUrl);
            return storageFile;
        }

        [HttpGet("{shortUrl}/download")]
        public async Task<IActionResult> Download(string shortUrl)
        {
            _logger.LogDebug("Download");
            var storageFile = await GetFileWithShortUrlAsync(shortUrl);
            var filePath = _fileManager.GetFile(storageFile.Id.ToString());
            return File(filePath, storageFile.ContentType, storageFile.FileName);
        }

        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> ViewFile(string shortUrl)
        {
            _logger.LogDebug("Download");
            var storageFile = await GetFileWithShortUrlAsync(shortUrl);
            var filePath = _fileManager.GetFile(storageFile.Id.ToString());
            return File(filePath, storageFile.ContentType);
        }

        [HttpGet]
        public async Task<StorageFile[]> All()
        {
            _logger.LogDebug("All");
            var files = await _storageFileRepository.GetAllAsync();
            return files.ToArray();
        }

        [HttpPut("{shortUrl}")]
        public async Task<StorageFile> Update(string shortUrl, string item, IFormFile file)
        {
            _logger.LogDebug("Update");
            if (string.IsNullOrEmpty(item) && file == null)
            {
                item.ThrowIfArgumentNullOrEmpty(nameof(item), "No item provided!");
                file.ThrowIfArgumentNull(nameof(file), "No file provided!");
            }
            var storageFile = await GetFileWithShortUrlAsync(shortUrl);
            if (file != null)
            {
                storageFile = _storageFileConverter.Convert(file);
                await _fileManager.StoreFileAsync(storageFile.Id.ToString(), file);
            }
            if (!string.IsNullOrEmpty(item))
            {
                item.UrlDecode().PopulateObject(storageFile);
            }

            await _storageFileRepository.UpdateAsync(storageFile.Id, storageFile);
            return storageFile;
        }

        [HttpPost]
        public async Task<StorageFile> Create(string item, IFormFile file)
        {
            _logger.LogDebug("Create");
            item.ThrowIfArgumentNullOrEmpty(nameof(item), "No item provided!");
            file.ThrowIfArgumentNull(nameof(file), "No file provided!");

            var storage = _storageFileConverter.Convert(file);
            item.UrlDecode().PopulateObject(storage);
            var storageFile = await _storageFileRepository.CreateWithShortUrlAsync(storage);
            try
            {
                await _fileManager.StoreFileAsync(storageFile.Id.ToString(), file);
            }
            catch (Exception)
            {
                await _storageFileRepository.DeleteAsync(storage.Id);
                throw;
            }
            return storageFile;
        }
    }
}