using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.Framework.ConfigurationModel;
using Storage.Common.Extensions;

namespace Storage.Datei.Services
{
    public class FileManager
    {
        private readonly string _fileFolder;

        public FileManager(IConfiguration configuration)
        {
            _fileFolder = configuration.Get("Configuration:FilesStoragePath");
            _fileFolder.ThrowIfArgumentNullOrEmpty("Configuration:FilesStoragePath", "No FilesStoragePath provided!");
        }

        private string GetAbsoluteFilePath(string filename)
        {
            return Path.Combine(_fileFolder, filename);
        }

        public async Task StoreFileAsync(string fileName, IFormFile formFile, bool overwrite = false)
        {
            var filePath = GetAbsoluteFilePath(fileName);
            if (File.Exists(filePath) && !overwrite)
            {
                throw new Exception("File already exists");
            }
            await formFile.SaveAsAsync(filePath);
        }

        public string GetFile(string fileName)
        {
            var filePath = GetAbsoluteFilePath(fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File was not found!");
            }
            return filePath;
        }
    }
}