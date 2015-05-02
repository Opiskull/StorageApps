using Microsoft.AspNet.Http;
using Storage.Common.Extensions;
using Storage.Common.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Converter
{
    public class StorageFileConverter : IConverter<IFormFile, StorageFile>
    {
        public StorageFile Convert(IFormFile from)
        {
            var storageFile = new StorageFile
            {
                FileName = from.GetFileName(),
                ContentType = from.ContentType,
                Size = from.Length
            };
            return storageFile;
        }
    }
}