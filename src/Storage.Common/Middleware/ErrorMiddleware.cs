using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Logging;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace Storage.Common.Middleware
{
    public class ErrorMiddleware
    {
		private readonly ILogger _logger;

		public ErrorMiddleware(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.Create("Error");
		}
	    private async Task Invoke(HttpContext context)
	    {
		    var feature = context.GetFeature<IErrorHandlerFeature>();
		    context.Response.StatusCode = 500;
		    context.Response.ContentType = "application/json";
			_logger.WriteError("Error in Application",feature.Error);
		    await context.Response.WriteAsync(new
		    {
			    Error = feature.Error,
			    Message = feature.Error.Message
		    }.ToJson());
	    }

	    public void ErrorHandler(IApplicationBuilder errorApp)
	    {
		    errorApp.Run(Invoke);
	    }
    }
}