using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using test_fuse.Models;
using test_fuse.Services.DataUpdateService;
using test_fuse.Services.TimerService;

namespace test_fuse.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataUpdateService _dataUpdateService;
        private readonly IDataUpdaterSingletonTimer _dataUpdaterSingletonMiddleware;

        public HomeController(IDataUpdateService dataUpdateService, IDataUpdaterSingletonTimer dataUpdaterSingletonMiddleware)
        {
            _dataUpdateService = dataUpdateService;
            _dataUpdaterSingletonMiddleware = dataUpdaterSingletonMiddleware;
            dataUpdaterSingletonMiddleware.Start();
        }

        public IActionResult Index(int page = 1, string sort = "")
        {
            page -= 1;

            if (page < 0)
            {
                page = 0;
            }

            var homePageModel = new HomePageModel();
            homePageModel.LastUpdate = _dataUpdaterSingletonMiddleware.LastUpdate.ToString();
            homePageModel.CryptoCurrencies = _dataUpdateService.Get(page * 100, (page + 1) * 100, sort);

            return View(homePageModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
