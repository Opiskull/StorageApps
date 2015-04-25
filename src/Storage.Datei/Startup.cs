using System;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Versioning;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.RequestContainer;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Logging.Console;
using MongoDB.Driver;
using Newtonsoft.Json;
using Storage.Common.Interfaces;
using Storage.Common.Services;
using Storage.Datei.Converter;
using Storage.Datei.Models;
using Storage.Datei.Repositories;
using Storage.Datei.Services;

namespace Storage.Datei
{
    public class Startup
    {
		private IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
	        Configuration = new Configuration()
				.AddJsonFile("config.json");
        }

	    private IMongoDatabase OpenDatabase()
	    {
			var mongoUrl = MongoUrl.Create(Configuration.Get("Database:MongoServerUrl"));
			var client = new MongoClient(mongoUrl);
		    return client.GetDatabase(mongoUrl.DatabaseName);
	    }

	    // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
	        services.AddLogging();
            services.AddMvc();
			services.AddSingleton<IStorageFileRepository, StorageFileRepository>();
	        services.AddSingleton<ILoggerFactory, LoggerFactory>();
	        services.AddSingleton<FileManager>();
	        services.AddSingleton<RandomStringGenerator>();
	        services.AddSingleton<IConverter<IFormFile, StorageFile>, StorageFileConverter>();
			services.AddInstance(typeof (IConfiguration), Configuration);
	        services.AddInstance(typeof (IMongoDatabase), OpenDatabase());
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
	        loggerFactory.AddConsole();


			//middlewares: https://gist.github.com/maartenba/77ca6f9cfef50efa96ec

			// http://www.reddit.com/r/programming/comments/2c1dns/aspnet_mvc_6_vnext/



			app.UseErrorHandler(errorApp =>
	        {
		        errorApp.Run(async context =>
		        {
			        var feature = context.GetFeature<IErrorHandlerFeature>();
			        context.Response.StatusCode = 500;
			        context.Response.ContentType = "application/json";
			        await context.Response.WriteAsync(JsonConvert.SerializeObject(new {Error = feature.Error, Message = feature.Error.Message}));
		        });
	        });
	        app.Use(async (ctx, next) =>
	        {
		        var requestLogger = loggerFactory.Create("Request");
		        var startTime = DateTime.UtcNow;
				requestLogger.WriteInformation("Start {0} [{1}]", ctx.Request.Method, ctx.Request.Path.Value);
		        await next();
		        var duration = DateTime.UtcNow - startTime;
				requestLogger.WriteInformation("End {0} [{1}] [{2}ms]", ctx.Request.Method,ctx.Request.Path.Value,duration.TotalMilliseconds);
	        });

			//app.UseMiddleware<ContainerMiddleware>();
            //app.UseStaticFiles();
            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
					defaults: new {  });
            });
		}
    }
}
