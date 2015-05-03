using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Description;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Storage.Common.Controllers
{
    [Route("docs")]
    public class ApiExplorerController : Controller
    {
        private readonly IApiDescriptionGroupCollectionProvider _provider;

        public ApiExplorerController(IApiDescriptionGroupCollectionProvider provider)
        {
            _provider = provider;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var descriptions = _provider.ApiDescriptionGroups.Items;
            return Json(descriptions);
        }
    }
}