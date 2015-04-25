using System;
using MongoDB.Bson;

namespace Storage.Common.Extensions
{
    public static class ValidationExtensions
    {
	    public static void ThrowIfArgumentNull(this object validationObject, string parameter, string message)
	    {
		    if (validationObject == null)
		    {
			    throw new ArgumentNullException(parameter,message);
		    }
	    }

	    public static void ThrowIfArgumentNullOrEmpty(this string validationObject, string parameter, string message)
	    {
		    if (string.IsNullOrEmpty(validationObject))
		    {
			    throw new ArgumentNullException(parameter, message);
		    }
	    }

	    public static ObjectId TryParseIdThrowIfNot(this string validationObject, string parameter = "Id", string message = "Provided Id is not a valid ObjectId")
	    {
		    ObjectId objectId = ObjectId.Empty;
		    if (!ObjectId.TryParse(validationObject, out objectId))
		    {
			    throw new ArgumentNullException(parameter,message);
		    }
			return objectId;
	    }
    }
}