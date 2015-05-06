using System.Collections.Generic;
using Storage.Common.Models;

namespace Storage.Common.Interfaces
{
    public interface ITaggingEntity
    {
        IEnumerable<Tag> Tags { get; set; }
    }
}