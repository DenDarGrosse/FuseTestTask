using System;
using System.Collections.Generic;
using test_fuse.Models;

namespace test_fuse.Services.DataUpdateService
{
    public interface IDataUpdateService
    {
        public void UpdateData();

        public List<CryptoCurrency> Get(int start, int end, string sort);
    }
}
