using System;

namespace Storage.Common.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException() : base("Item not found!")
        {
        }
    }
}