using System;
using System.Data.SqlTypes;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.ConfigurationModel;

namespace Storage.Datei.Services
{
    public class FileManager
    {
	    private readonly string _fileFolder;

	    public FileManager(IConfiguration configuration)
	    {
			_fileFolder = configuration.Get("Configuration:FileFolder");
	    }

	    private string GetFilePath(string id)
	    {
		    return Path.Combine(_fileFolder, id);
		}

	    public async Task StoreFile(string id, IFormFile formFile)
	    {
			var filePath = GetFilePath(id);
			if (File.Exists(filePath))
			{
				throw new Exception("File already exists");
			}
		    await formFile.SaveAsAsync(filePath);
	    }

	    public string GetFile(string id)
	    {
		    var filePath = GetFilePath(id);
		    if (!File.Exists(filePath))
		    {
				throw new FileNotFoundException("File was not found!");
			}
		    return filePath;
	    }
    }
}