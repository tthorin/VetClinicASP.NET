namespace VetClinic.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using VetClinic.Models;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            MongoDbAccess.Database.DbSeeder dbs = MongoDbAccess.Factory.GetDbSeeder();
            await dbs.SeedDB();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> About()
        {
            var db=MongoDbAccess.Factory.GetDataAccess();
            (int customers,int animals)=await db.GetDbStats();
            DbInformation dbStats = new() { Customers=customers,Animals=animals};
            return View(dbStats);
        }
    }
}