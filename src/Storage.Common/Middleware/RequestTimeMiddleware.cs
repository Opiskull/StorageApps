using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Logging;

namespace Storage.Common.Middleware
{
    public class RequestTimeMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public RequestTimeMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger("Request");
        }

        public async Task Invoke(HttpContext ctx)
        {
            using (_logger.BeginScope("Request"))
            {
                var watch = new Stopwatch();
                watch.Start();
                await _next(ctx);
                _logger.LogInformation(
                    $"{ctx.Request.Method} [{ctx.Request.Path.Value}] [{watch.ElapsedMilliseconds}ms]");
            }
        }
    }
}