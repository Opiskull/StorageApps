using System;
using System.Collections.Generic;
using System.Linq;

namespace Storage.Common.Services
{
	public class RandomStringGenerator
	{
		private readonly Random _random;
		private const string BASE64_URL = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";

		public RandomStringGenerator()
		{
			_random = new Random();
		}

		public string GenerateBase64Url(int count)
		{
			return string.Concat(Enumerable.Repeat(0, count).Select(i => GenerateChar()));
		}
		
		private char GenerateChar()
		{
			var index = _random.Next(0, BASE64_URL.Length);
			return BASE64_URL[index];
		}
	}
}