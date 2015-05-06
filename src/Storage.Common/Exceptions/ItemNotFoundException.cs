using System;
using System.Runtime.Serialization;

namespace Storage.Common.Exceptions
{
    [DataContract]
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException() : base("Item not found!")
        {
        }
    }
}