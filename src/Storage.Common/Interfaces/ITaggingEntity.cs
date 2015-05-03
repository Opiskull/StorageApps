using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Storage.Common.Interfaces
{
    public interface ITaggingEntity
    {
        string[] Tags { get; set; }
    }
}
