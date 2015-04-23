using System;

namespace Storage.Common.Services
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class CollectionNameAttribute : Attribute
	{
		public CollectionNameAttribute(string collectionName)
		{
			this.CollectionName = collectionName;
		}

		public string CollectionName { get; set; }
	}
}