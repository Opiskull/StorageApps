using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Logging;
using Storage.Common.Exceptions;
using Storage.Common.Extensions;
using Storage.Common.Models;

namespace Storage.Common.Middleware
{
    public class JsonErrorMiddleware
    {
        private readonly ILogger _logger;

        public JsonErrorMiddleware(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Error");
        }

        private JsonHttpError CreateJsonError(Exception exception)
        {
            if (exception is ItemNotFoundException)
            {
                return new JsonHttpError
                {
                    Message = "Item not found!",
                    StatusCode = (int) HttpStatusCode.BadRequest
                };
            }
            return new JsonHttpError
            {
                Error = exception,
                Message = exception.Message
            };
        }

        private async Task WriteErrorResponseAsync(HttpContext context, Exception exception)
        {
            var httpError = CreateJsonError(exception);
            context.Response.StatusCode = httpError.StatusCode;
            context.Response.ContentType = httpError.ContentType;
            await context.Response.WriteAsync(httpError.ToJson());
        }

        public async Task Invoke(HttpContext context)
        {
            var feature = context.GetFeature<IErrorHandlerFeature>();
            _logger.LogError("Error in Application", feature.Error);
            await WriteErrorResponseAsync(context, feature.Error);
        }
    }
}