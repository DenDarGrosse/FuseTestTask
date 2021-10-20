using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_fuse.Models;

namespace test_fuse.Services.LogoService
{
    public class MockLogoService : ILogoService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public MockLogoService(IConfiguration configuration, ILogger<MockLogoService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void FetchLogos(List<CryptoCurrency> currencies)
        {
            _logger.LogDebug("Mock FetchLogos");
            foreach (var currency in currencies)
            {
                if (currency.logo == null)
                {
                    currency.logo = _configuration["MockData:Logo"];
                }
            }
        }
    }
}
