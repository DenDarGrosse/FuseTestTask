using System.Collections.Generic;
using test_fuse.Models;

namespace test_fuse.Services.LogoService
{
    public interface ILogoService
    {
        public void FetchLogos(List<CryptoCurrency> currencies);
    }
}
