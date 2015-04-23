using System;
using System.Net;
using Newtonsoft.Json;

namespace Storage.Common.Extensions
{
    public static class ParseJsonExtensions
    {
	    public static T ToObject<T>(this string input)
	    {
		    return JsonConvert.DeserializeObject<T>(input);
	    }

	    public static string UrlDecode(this string input)
	    {
			//TODO: maybe replace with a newer version of https://github.com/aspnet/HttpAbstractions/commit/1008e1725954dd59a157722434c9f823f6160272
			return WebUtility.UrlDecode(input);
	    }
    }
}