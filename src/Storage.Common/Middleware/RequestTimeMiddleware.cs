using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace Storage.Common.Middleware
{
    public class RequestTimeMiddleware
    {
	    private readonly RequestDelegate _next;
	    private readonly ILogger _logger;

	    public RequestTimeMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
	    {
		    _next = next;
		    _logger = loggerFactory.Create("Request");
	    }

	    public async Task Invoke(HttpContext ctx)
	    {
		    using (_logger.BeginScope("Request"))
		    {
				var watch = new Stopwatch();
				watch.Start();
				await _next(ctx);
				_logger.WriteInformation($"{ctx.Request.Method} [{ctx.Request.Path.Value}] [{watch.ElapsedMilliseconds}ms]");
			}
	    }
    }
}