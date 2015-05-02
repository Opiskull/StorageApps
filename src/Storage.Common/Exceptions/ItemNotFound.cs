using Microsoft.AspNet.Mvc;

namespace Storage.Common.Exceptions
{
    public class ItemNotFoundJsonResult : JsonResult
    {
        public ItemNotFoundJsonResult() : base(new {})
        {
            StatusCode = 404;
            Value = new {Error = "Item not found!"};
        }
    }
}