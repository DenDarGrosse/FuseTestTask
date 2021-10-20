using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace test_fuse.Services.HttpRequestService
{
    public class HttpRequestService : IHttpRequestService
    {
        private static int coinsCurrient;
        private static int coinsMaximum;
        private static bool started = false;
        private readonly string API_KEY;
        private readonly ILogger _logger;

        public int FreeCoins
        {
            get => coinsMaximum - coinsCurrient;
            set => coinsCurrient = coinsMaximum - value;
        }

        public HttpRequestService(IConfiguration configuration, ILogger<HttpRequestService> logger)
        {
            API_KEY = configuration["API_KEY"];
            _logger = logger;

            if (!started)
            {
                started = true;
                var response = Send(configuration["urls:keyInfo"], new Dictionary<string, string>());
                dynamic parsedData = JObject.Parse(response);
                var data = parsedData.data;
                var plan = data.plan;
                var usage = data.usage;
                coinsMaximum = Convert.ToInt32(plan.credit_limit_daily);
                coinsCurrient = Convert.ToInt32(usage.current_day.credits_used);
            }
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
