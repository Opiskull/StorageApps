using System.Text;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Description;
using Storage.Common.Extensions;
using Storage.Common.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Storage.Common.Controllers
{
    [Route("[controller]")]
    public class DocsController : Controller
    {
        private readonly MarkdownDocumentationGenerator _markdownDocumentationGenerator;
        private readonly IApiDescriptionGroupCollectionProvider _provider;

        public DocsController(IApiDescriptionGroupCollectionProvider provider,
            MarkdownDocumentationGenerator markdownDocumentationGenerator)
        {
            _provider = provider;
            _markdownDocumentationGenerator = markdownDocumentationGenerator;
        }

        [HttpGet("api.{format}")]
        [HttpGet("api")]
        public IActionResult Api(string format = "html")
        {
            var content = _markdownDocumentationGenerator.CreateApiDocumentation(_provider.ApiDescriptionGroups.Items);
            if (format == "md")
            {
                return Content(content, "text/plain", Encoding.UTF8);
            }
            if (format == "json")
            {
                return Content(_provider.ApiDescriptionGroups.Items.ToJson());
            }
            return Content(content.ToHtml(), "text/html", Encoding.UTF8);
        }
    }
}