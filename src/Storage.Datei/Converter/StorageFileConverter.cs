using Microsoft.AspNet.Http;
using Storage.Common.Extensions;
using Storage.Common.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Converter
{
	public class StorageFileConverter : IConverter<IFormFile,StorageFile>
	{
		public StorageFile Convert(IFormFile from)
		{
			var storageFile = new StorageFile
			{
				ContentType = @from.ContentType,
				FileName = @from.GetFileName(),
				Size = @from.GetSize()
			};
			return storageFile;
		}
	}
}