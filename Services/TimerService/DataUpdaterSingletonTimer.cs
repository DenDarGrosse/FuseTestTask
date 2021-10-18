using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Timers;
using test_fuse.Services.DataUpdateService;
using test_fuse.Services.HttpRequestService;

namespace test_fuse.Services.TimerService
{
    public class DataUpdaterSingletonTimer : IDataUpdaterSingletonTimer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpRequestService _httpRequestService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly Timer timer;

        private bool started = false;

        public DataUpdaterSingletonTimer(
            IServiceProvider serviceProvider, 
            IHttpRequestService httpRequestService, 
            IConfiguration configuration,
            ILogger<DataUpdaterSingletonTimer> logger)
        {
            _serviceProvider = serviceProvider;
            _httpRequestService = httpRequestService;
            _configuration = configuration;
            _logger = logger;
            timer = new Timer(1000 * 60);
        }

        public DateTime LastUpdate { get; set; }

        public void Start()
        {
            if (!started)
            {
                _logger.LogInformation($"Update timer started. Next update will be at {DateTime.Now.AddMinutes(1)}");

                started = true;

                int averageCoinsCount = Convert.ToInt32(_configuration["averageCoinsCount"]);
                int nextUpdateMinute;
                DateTime nextTime = DateTime.Now;
                timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                timer.Start();

                void OnTimedEvent(object source, ElapsedEventArgs e)
                {
                    if (DateTime.Now >= nextTime)
                    {
                        IDataUpdateService dataUpdateService = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IDataUpdateService>();
                        dataUpdateService.UpdateData();
                        nextUpdateMinute = (int)(24.0f / _httpRequestService.FreeCoins * averageCoinsCount * 60);
                        nextTime = DateTime.Now.AddMinutes(nextUpdateMinute);
                        LastUpdate = DateTime.Now;
                        _logger.LogInformation($"Successfull update. The next one will be at {nextTime}");
                    }
                }
            }
        }
    }
}
