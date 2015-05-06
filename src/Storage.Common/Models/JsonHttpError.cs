using System;
using System.Runtime.Serialization;

namespace Storage.Common.Models
{
    [DataContract]
    public class JsonHttpError
    {
        public JsonHttpError()
        {
            Message = "Internal Server Error";
            Error = null;
            ContentType = "application/json";
            StatusCode = 500;
        }

        [DataMember(Name = "error")]
        public Exception Error { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [IgnoreDataMember]
        public string ContentType { get; set; }

        [IgnoreDataMember]
        public int StatusCode { get; set; }
    }
}