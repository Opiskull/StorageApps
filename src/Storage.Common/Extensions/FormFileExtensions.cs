using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;

namespace Storage.Common.Extensions
{
    public static class FormFileExtensions
    {
        public static string GetFileName(this IFormFile formFile)
        {
            return ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
        }
    }
}