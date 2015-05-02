using Microsoft.Framework.ConfigurationModel;

namespace Storage.Common.Services
{
    public class ShortUrlGenerator
    {
        private const string CONFIGURATION_SHORTURL_LENGTH = "Configuration:ShortUrlLength";
        private const int DEFAULT_LENGTH = 10;

        private readonly IConfiguration _configuration;
        private readonly RandomStringGenerator _randomStringGenerator;

        public ShortUrlGenerator(RandomStringGenerator randomStringGenerator, IConfiguration configuration)
        {
            _randomStringGenerator = randomStringGenerator;
            _configuration = configuration;
        }

        public string Generate()
        {
            var length = _configuration.Get<int>(CONFIGURATION_SHORTURL_LENGTH);
            if (length == 0) length = DEFAULT_LENGTH;
            return Generate(length);
        }

        public string Generate(int length)
        {
            return _randomStringGenerator.GenerateBase64Url(length);
        }
    }
}