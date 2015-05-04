using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using Storage.Common.Services;
using Storage.Datei.Interfaces;
using Storage.Datei.Models;

namespace Storage.Datei.Controllers
{
    [ApiExplorerSettings(GroupName = "BlogPosts")]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class BlogPostsController : RestController<BlogPost>
    {
        public BlogPostsController(ILoggerFactory logFactory, IBlogPostRepository repository)
            : base(logFactory, repository)
        {
        }
    }
}