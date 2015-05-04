using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using MongoDB.Bson;
using Storage.Common.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Storage.Common.Services
{
    public class RestController<T> : Controller where T : class, IMongoDbEntity
    {
        protected readonly ILogger _logger;
        protected readonly IMongoDbRepository<T> Repository;

        public RestController(ILoggerFactory logFactory, IMongoDbRepository<T> repository)
        {
            _logger = logFactory.CreateLogger<RestController<T>>();
            Repository = repository;
        }

        // GET: api/controller
        [HttpGet]
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            _logger.LogVerbose("GetAll");
            var items = await Repository.GetAllAsync();

            return items;
        }

        // GET api/controller/5
        [HttpGet("{id}")]
        public virtual async Task<T> Get(string id)
        {
            _logger.LogVerbose("Get");
            var item = await Repository.GetAsync(ObjectId.Parse(id));
            return item;
        }

        // POST api/controller
        [HttpPost]
        public virtual async Task<T> Create([FromBody] T item)
        {
            _logger.LogVerbose("Create");
            var createdItem = await Repository.CreateAsync(item);
            return createdItem;
        }

        // PUT api/controller/5
        [HttpPut("{id}")]
        public virtual async Task<T> Update(string id, [FromBody] T item)
        {
            _logger.LogVerbose("Update");
            item.Id = ObjectId.Parse(id);
            await Repository.UpdateAsync(item.Id, item);
            return item;
        }

        // DELETE api/controller/5
        [HttpDelete("{id}")]
        public virtual async void Delete(string id)
        {
            _logger.LogVerbose("Delete");
            await Repository.DeleteAsync(ObjectId.Parse(id));
        }
    }
}