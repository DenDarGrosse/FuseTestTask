using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace test_fuse.Services.HttpRequestService
{
    public class HttpRequestService : IHttpRequestService
    {
        private int coinsCurrient;
        private readonly int coinsMaximum;
        private readonly string API_KEY;
        private readonly ILogger _logger;

        public int FreeCoins
        {
            get => coinsMaximum - coinsCurrient;
            set => coinsCurrient = coinsMaximum - value;
        }

        public HttpRequestService(IConfiguration configuration, ILogger<HttpRequestService> logger)
        {
            coinsMaximum = Convert.ToInt32(configuration["maxCoins"]);
            coinsCurrient = Convert.ToInt32(configuration["coinsStart"]);
            API_KEY = configuration["API_KEY"];
            _logger = logger;
        }

        public string Send(string url, Dictionary<string, string> parameters)
        {
            try
            {
                var URL = new UriBuilder(url);

                var queryString = HttpUtility.ParseQueryString(string.Empty);
                foreach (var parameter in parameters)
                {
                    queryString[parameter.Key] = parameter.Value;
                }

                URL.Query = queryString.ToString();

                var client = new WebClient();
                client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
                client.Headers.Add("Accepts", "application/json");

                return client.DownloadString(URL.ToString());
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return "";
            }
        }
    }
}
