using System;

namespace test_fuse.Services.TimerService
{
    public interface IDataUpdaterSingletonTimer
    { 
        public DateTime LastUpdate { get; set; }

        public void Start();
    }
}