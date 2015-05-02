using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using Storage.Common.Middleware;

namespace Storage.Common.Extensions
{
    public static class JsonErrorMiddlewareExtensions
    {
        public static IApplicationBuilder UseJsonErrorHandler(this IApplicationBuilder app)
        {
            return app.UseErrorHandler(errorApp =>
            {
                var middleware = app.ApplicationServices.GetService<JsonErrorMiddleware>();
                errorApp.Run(middleware.Invoke);
            });
        }
    }
}