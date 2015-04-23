using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using MongoDB.Bson;
using Storage.Common.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Storage.Common.Services
{
	public class RestController<T> : Controller where T : class, IEntity
	{
		protected readonly ILogger _logger;
		protected readonly IRepository<T> _repository;

		public RestController(ILoggerFactory logFactory, IRepository<T> repository)
		{
			_logger = logFactory.Create("RestController");
            this._repository = repository;
		}

		// GET: api/controller
		[HttpGet]
        public virtual async Task<JsonResult> GetAll()
        {
			_logger.WriteVerbose("GetAll");
			var items = await _repository.GetAllAsync();

			return Json(items);
        }

		// GET api/controller/5
		[HttpGet("{id}")]
        public virtual async Task<JsonResult> Get(string id)
        {
			_logger.WriteVerbose("Get");
			var item = await _repository.GetAsync(ObjectId.Parse(id));
			return Json(item);
        }

		// POST api/controller
		//[HttpPost]
  //      public virtual async Task<JsonResult> Create([FromBody]T value)
  //      {
		//	_logger.WriteVerbose("Create");
		//	var item = await _repository.CreateAsync(value);
		//	return Json(item);
  //      }

		// PUT api/controller/5
		[HttpPut("{id}")]
        public virtual async Task Update(string id, [FromBody]T value)
        {
			_logger.WriteVerbose("Update");
			await _repository.UpdateAsync(ObjectId.Parse(id),value);
        }

		// DELETE api/controller/5
		[HttpDelete("{id}")]
        public virtual async void Delete(string id)
        {
			_logger.WriteVerbose("Delete");
			await _repository.DeleteAsync(ObjectId.Parse(id));
        }
    }
}
