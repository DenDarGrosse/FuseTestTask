using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using test_fuse.Models;
using test_fuse.Repositories;
using test_fuse.Services.HttpRequestService;
using test_fuse.Services.LogoService;

namespace test_fuse.Services.DataUpdateService
{
    public class DataUpdateService : IDataUpdateService
    {
        private static readonly List<CryptoCurrency> cryptoCurrencies = new List<CryptoCurrency>();
        private readonly ILogoRepository _logoRepository;
        private readonly ILogoService _logoService;
        private readonly IHttpRequestService _httpRequestService;
        private readonly IConfiguration _configuration;

        public DataUpdateService(
            ILogoRepository logoRepository, 
            ILogoService logoService, 
            IHttpRequestService httpRequestService, 
            IConfiguration configuration)
        {
            _logoRepository = logoRepository;
            _logoService = logoService;
            _httpRequestService = httpRequestService;
            _configuration = configuration;
        }

        private int DeserializeData(string str)
        {
            dynamic stuff = JObject.Parse(str);
            dynamic data = stuff.data;
            int count = 0;

            foreach(var currency in data)
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

        //TODO make async
        public void UpdateData()
        {
            cryptoCurrencies.Clear();

            int start = 1;
            bool allFetched = false;

            while (!allFetched)
            {
                var parameters = new Dictionary<string, string>
                {
                    { "start", start.ToString() },
                    { "limit", "5000" },
                    { "convert", "USD" }
                };

                var response =  _httpRequestService.Send(_configuration["urls:meta"], parameters);

                int fetched = DeserializeData(response);
                _logoService.FetchLogos(cryptoCurrencies);
                _httpRequestService.FreeCoins -= (int)Math.Ceiling(((double)fetched) / 200);

                start += fetched;
                if (fetched == 0)
                {
                    allFetched = true;
                }
            }
        }

        public List<CryptoCurrency> Get(int start, int end, string sort)
        {
            if (start > cryptoCurrencies.Count)
            {
                return new List<CryptoCurrency>();
            }

            if(end > cryptoCurrencies.Count)
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
    }
}
