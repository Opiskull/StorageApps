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
		private readonly IStorageFileRepository _storageFileRepository;
		private readonly ILogger _logger;

		public FilesController(ILoggerFactory logFactory, IStorageFileRepository storageFileRepository, FileManager fileManager)
		{
			_logger = logFactory.Create<FilesController>();
			_storageFileRepository = storageFileRepository;
			_fileManager = fileManager;
		}

		[HttpGet("{id}/info")]
		public async Task<JsonResult> GetFileInfo(string id)
		{
			var storageFile = await _storageFileRepository.GetAsync(ObjectId.Parse(id));
			return Json(storageFile);
		}

		[HttpGet("{id}")]
		public async Task<FileResult> Download(string id)
		{
			var storageFile = await _storageFileRepository.GetAsync(ObjectId.Parse(id));
			var filePath = _fileManager.GetFile(id);
			return File(filePath, storageFile.ContentType, storageFile.FileName);
		}

		[HttpGet]
		public async Task<JsonResult> GetAll()
		{
			var files = await _storageFileRepository.GetAllAsync();
			return Json(files);
		}
			
			
			[HttpPost("")]
		public async Task<JsonResult> Upload(string item, IFormFile file)
		{
			var storage = item.UrlDecode().ToObject<StorageFile>();
			storage.FileName = file.GetFileName();
			storage.ContentType = file.ContentType;
			storage.Size = file.GetSize();

			var storageFile = await _storageFileRepository.CreateAsync(storage);

			try
			{
				await _fileManager.StoreFile(storageFile.Id.ToString(), file);
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
