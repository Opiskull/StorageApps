using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Loader.IIS.Util;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.WebUtilities;
using Microsoft.Framework.Logging;
using Microsoft.Net.Http.Headers;
using MongoDB.Bson;
using Newtonsoft.Json;
using Storage.Common.Extensions;
using Storage.Common.Interfaces;
using Storage.Common.Services;
using Storage.Datei.Models;
using Storage.Datei.Repositories;
using Storage.Datei.Services;

namespace Storage.Datei.Controllers
{
	[Route("api/[controller]")]
	public class FilesController : Controller
	{
		private readonly FileManager _fileManager;
		private readonly IConverter<IFormFile, StorageFile> _storageFileConverter;
		private readonly IStorageFileRepository _storageFileRepository;
		private readonly ILogger _logger;

		public FilesController(ILoggerFactory logFactory, IStorageFileRepository storageFileRepository, FileManager fileManager, IConverter<IFormFile,StorageFile> storageFileConverter )
		{
			_logger = logFactory.Create<FilesController>();
			_storageFileRepository = storageFileRepository;
			_fileManager = fileManager;
			_storageFileConverter = storageFileConverter;
		}

		[HttpGet("{shortId}/info")]
		public async Task<JsonResult> Info(string shortId)
		{
			var storageFile = await _storageFileRepository.GetWithShortUrlAsync(shortId);
			return Json(storageFile);
		}

		[HttpGet("{shortId}")]
		public async Task<FileResult> Download(string shortId)
		{
			var storageFile = await _storageFileRepository.GetWithShortUrlAsync(shortId);
			var filePath = _fileManager.GetFile(storageFile.Id.ToString());
			return File(filePath, storageFile.ContentType, storageFile.FileName);
		}

		[HttpGet]
		public async Task<JsonResult> All()
		{
			var files = await _storageFileRepository.GetAllAsync();
			return Json(files);
		}

		[HttpPut("{shortId}")]
		public async Task<JsonResult> Update(string shortId, string item, IFormFile file)
		{
			if (string.IsNullOrEmpty(item) && file == null)
			{
				item.ThrowIfArgumentNullOrEmpty(nameof(item), "No item provided!");
				file.ThrowIfArgumentNull(nameof(file), "No file provided!");
			}
			StorageFile storageFile = await _storageFileRepository.GetWithShortUrlAsync(shortId);
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
			item.ThrowIfArgumentNullOrEmpty(nameof(item), "No item provided!");
			file.ThrowIfArgumentNull(nameof(file), "No file provided!");

			var storage = _storageFileConverter.Convert(file);
			item.UrlDecode().PopulateObject(storage);
			var storageFile = await _storageFileRepository.CreateAsync(storage);
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
