using System;
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
    [Route("api/[controller]")]
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

        [HttpGet("{shortUrl}/info")]
        public async Task<JsonResult> Info(string shortUrl)
        {
            _logger.LogDebug("Info");
            var storageFile = await _storageFileRepository.GetWithShortUrlAsync(shortUrl);
            if (storageFile == null) return new ItemNotFoundJsonResult();
            return Json(storageFile);
        }

        [HttpGet("{shortUrl}")]
        public async Task<ActionResult> Download(string shortUrl)
        {
            _logger.LogDebug("Download");
            var storageFile = await _storageFileRepository.GetWithShortUrlAsync(shortUrl);
            if (storageFile == null) return new ItemNotFoundJsonResult();
            var filePath = _fileManager.GetFile(storageFile.Id.ToString());
            return File(filePath, storageFile.ContentType, storageFile.FileName);
        }

        [HttpGet]
        public async Task<JsonResult> All()
        {
            _logger.LogDebug("All");
            var files = await _storageFileRepository.GetAllAsync();
            return Json(files);
        }

        [HttpPut("{shortUrl}")]
        public async Task<JsonResult> Update(string shortUrl, string item, IFormFile file)
        {
            _logger.LogDebug("Update");
            if (string.IsNullOrEmpty(item) && file == null)
            {
                item.ThrowIfArgumentNullOrEmpty(nameof(item), "No item provided!");
                file.ThrowIfArgumentNull(nameof(file), "No file provided!");
            }
            var storageFile = await _storageFileRepository.GetWithShortUrlAsync(shortUrl);
            if (storageFile == null) return new ItemNotFoundJsonResult();
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
            return Json(storageFile);
        }

        [HttpPost]
        public async Task<JsonResult> Create(string item, IFormFile file)
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
            return Json(storageFile);
        }
    }
}