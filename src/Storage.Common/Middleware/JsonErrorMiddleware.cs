using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Logging;
using MongoDB.Bson;

namespace Storage.Common.Middleware
{
    public class JsonErrorMiddleware
    {
        private readonly ILogger _logger;

        public JsonErrorMiddleware(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Error");
        }

        private async Task Invoke(HttpContext context)
        {
            var feature = context.GetFeature<IErrorHandlerFeature>();
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            _logger.LogError("Error in Application", feature.Error);
            await context.Response.WriteAsync(new
            {
                feature.Error,
                feature.Error.Message
            }.ToJson());
        }

        public void ErrorHandler(IApplicationBuilder errorApp)
        {
            errorApp.Run(Invoke);
        }
    }
}