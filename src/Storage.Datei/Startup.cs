using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using MongoDB.Driver;
using Storage.Common.Interfaces;
using Storage.Common.Middleware;
using Storage.Common.Services;
using Storage.Datei.Converter;
using Storage.Datei.Interfaces;
using Storage.Datei.Models;
using Storage.Datei.Repositories;
using Storage.Datei.Services;

namespace Storage.Datei
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new Configuration()
                .AddJsonFile("config.json");
        }

        private IConfiguration Configuration { get; }

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
            services.AddSingleton<JsonErrorMiddleware>();
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


            var errorMiddleware = app.ApplicationServices.GetService<JsonErrorMiddleware>();

            app.UseErrorHandler(errorMiddleware.ErrorHandler);
            app.UseMiddleware<RequestTimeMiddleware>();

            //app.UseMiddleware<ContainerMiddleware>();
            //app.UseStaticFiles();
            // Add MVC to the request pipeline.
            app.UseMvc(routes => { routes.MapRoute("default", "{controller}/{action}/{id?}", new {}); });
        }
    }
}