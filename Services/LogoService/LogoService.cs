using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using test_fuse.Models;
using test_fuse.Repositories;
using test_fuse.Services.HttpRequestService;

namespace test_fuse.Services.LogoService
{
    public class LogoService : ILogoService
    {
        private readonly ILogoRepository _logoRepository;
        private readonly IHttpRequestService _httpRequestService;
        private readonly IConfiguration _configuration;

        public LogoService(
            ILogoRepository logoRepository, 
            IHttpRequestService httpRequestService, 
            IConfiguration configuration)
        {
            _logoRepository = logoRepository;
            _httpRequestService = httpRequestService;
            _configuration = configuration;
        }

        //TODO make async
        public void FetchLogos(List<CryptoCurrency> currencies)
        {
            var idsToFetch =
                currencies
                .Where(t => t.logo == null)
                .Select(t => t.id)
                .ToList();
            
            if (idsToFetch.Count == 0)
            {
                return;
            }

            for (int i = 0; i < idsToFetch.Count; i += 1000)
            {
                var idsToSend = idsToFetch.GetRange(i, (idsToFetch.Count - i*1000 > 1000) ? 1000 : idsToFetch.Count - i*1000);

                var parameters = new Dictionary<string, string>
                {
                    { "id", string.Join(',', idsToSend) },
                    { "aux", "logo" }
                };

                var response = _httpRequestService.Send(_configuration["urls:logo"], parameters);
                _httpRequestService.FreeCoins -= (int)Math.Ceiling(((double)idsToSend.Count) / 100);

                dynamic stuff = JObject.Parse(response);
                dynamic data = stuff.data;

                foreach (dynamic logoObj in data)
                {
                    int id = Convert.ToInt32((string)logoObj.Value.id);
                    string logo = (string)logoObj.Value.logo;
                    currencies.Find(t => t.id == id).logo = logo;
                    _logoRepository.Save(new Models.Domains.Logo(id, logo));
                }
            }
        }
    }
}
