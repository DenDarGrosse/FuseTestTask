using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using test_fuse.Models.Domains;

namespace test_fuse.Repositories
{
    public class MockLogoRepository : ILogoRepository
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public MockLogoRepository(
            ILogger<MockLogoRepository> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public string GetLogoURLByCurrencyId(int logoId)
        {
            _logger.LogDebug($"Mock GetLogoURLByCurrencyId({logoId})");
            return _configuration["MockData:Logo"];
        }

        public void Save(Logo logo)
        {
            _logger.LogDebug($"Mock Save({logo.CurrencyId})");
        }
    }
}
