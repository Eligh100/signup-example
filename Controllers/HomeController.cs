using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using signup_example.Models;
using signup_example.Services;

namespace signup_example.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPasswordService _passwordService;
        private readonly IApiService _apiService;

        public HomeController(ILogger<HomeController> logger, IPasswordService passwordService, IApiService apiService)
        {
            _logger = logger;
            _passwordService = passwordService;
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult RegisterUser(string email, string password, string confirmPassword)
        {
            ViewBag.loading = true;

            System.Threading.Thread.Sleep(2000);

            return PartialView("UserRegistered");

            //Password passwordAndSalt = _passwordService.SaltAndHashPassword(password);

            //UserConfig userConfig = new UserConfig
            //{
            //    Email = email,
            //    PasswordHash = passwordAndSalt.HashedPassword,
            //    Salt = passwordAndSalt.Salt
            //};

            //_apiService.Post("user", userConfig).ContinueWith((task) =>
            //{
            //    ViewBag.loading = false;
            //}, TaskScheduler.FromCurrentSynchronizationContext());
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
    }
}
