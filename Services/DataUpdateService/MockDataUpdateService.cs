using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using test_fuse.Models;
using test_fuse.Services.LogoService;
using test_fuse.Repositories;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace test_fuse.Services.DataUpdateService
{
    public class MockDataUpdateService : IDataUpdateService
    {
        private static readonly List<CryptoCurrency> cryptoCurrencies = new List<CryptoCurrency>();
        private readonly ILogoService _logoService;
        private readonly ILogoRepository _logoRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public MockDataUpdateService(
            ILogoService logoService, 
            ILogoRepository logoRepository,
            IConfiguration configuration,
            ILogger<MockDataUpdateService> logger)
        {
            _logoService = logoService;
            _logoRepository = logoRepository;
            _configuration = configuration;
            _logger = logger;
        }

        private int DeserializeData(string str)
        {
            dynamic stuff = JObject.Parse(str);
            dynamic data = stuff.data;
            int count = 0;

            foreach (var currency in data)
            {
                dynamic quote = currency.quote;
                dynamic quoteUSD = quote.USD;

                CryptoCurrency cryptoCurrency = new CryptoCurrency(
                        (int)currency.id,
                        (string)currency.name,
                        (string)currency.symbol,
                        _logoRepository.GetLogoURLByCurrencyId((int)currency.id),
                        Math.Round((double)quoteUSD.price, 2),
                        Math.Round((double)quoteUSD.percent_change_1h, 2),
                        Math.Round((double)quoteUSD.percent_change_24h, 2),
                        Math.Round((double)quoteUSD.market_cap, 2));

                cryptoCurrencies.Add(cryptoCurrency);
                count++;
            }

            return count;
        }

        public List<CryptoCurrency> Get(int start, int end, string sort = "")
        {
            if (start > cryptoCurrencies.Count)
            {
                return new List<CryptoCurrency>();
            }

            if (end > cryptoCurrencies.Count)
            {
                end = cryptoCurrencies.Count;
            }

            List<CryptoCurrency> sorted = sort switch
            {
                "name" => cryptoCurrencies.OrderBy(t => t.name).ToList(),
                "price" => cryptoCurrencies.OrderBy(t => t.priceUSD).ToList(),
                "change1h" => cryptoCurrencies.OrderBy(t => t.priceChange1h).ToList(),
                "change24h" => cryptoCurrencies.OrderBy(t => t.priceChange24h).ToList(),
                "mcap" => cryptoCurrencies.OrderBy(t => t.marketCup).ToList(),
                _ => cryptoCurrencies,
            };
            return sorted.GetRange(start, end - start);
        }

        public void UpdateData()
        {
            _logger.LogDebug("Mock UpdateData");
            string data = System.IO.File.ReadAllText(_configuration["Mock:Data"]);
            DeserializeData(data);
            _logoService.FetchLogos(cryptoCurrencies);
        }
    }
}
