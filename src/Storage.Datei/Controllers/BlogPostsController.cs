using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using Storage.Common.Services;
using Storage.Datei.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Controllers
{
    [ApiExplorerSettings(GroupName = "BlogPosts")]
    [Route("api/v1/[controller]")]
    public class BlogPostsController : RestController<BlogPost>
    {
        private readonly IBlogPostRepository _repository;

        public BlogPostsController(ILoggerFactory logFactory, IBlogPostRepository repository)
            : base(logFactory, repository)
        {
            _repository = repository;
        }

        [HttpGet("latest")]
        public Task<List<BlogPost>> Latest(int? count, int? skip)
        {
            if (count == null) count = 20;
            if (skip == null) skip = 0;
            return _repository.GetLatestAsync(count, skip);
        }

        [HttpGet("link/{link}")]
        public Task<BlogPost> Link(string link)
        {
            return _repository.GetWithPermLinkAsync(link);
        }
    }
}