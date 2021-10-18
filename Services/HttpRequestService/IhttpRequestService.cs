using System.Collections.Generic;

namespace test_fuse.Services.HttpRequestService
{
    public interface IHttpRequestService
    {
        public int FreeCoins { get; set; }

        public string Send(string url, Dictionary<string, string> parameters);
    }
}
