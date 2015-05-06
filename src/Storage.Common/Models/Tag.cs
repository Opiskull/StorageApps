using System.Runtime.Serialization;

namespace Storage.Common.Models
{
    [DataContract]
    public class Tag
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "count")]
        public int Count { get; set; }
    }
}