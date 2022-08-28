using System;
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
        
        public async Task<IActionResult> RegisterUser(User newUser)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();

                Password passwordAndSalt = _passwordService.SaltAndHashPassword(newUser.Password);

                UserConfig userConfig = new UserConfig
                {
                    Email = newUser.Email,
                    PasswordHash = passwordAndSalt.HashedPassword,
                    Salt = passwordAndSalt.Salt
                };

                try
                {
                    await _apiService.Post("user", userConfig);
                } catch (Exception e)
                {
                    Debug.WriteLine("ERROR");
                }

                return PartialView("UserRegistered");
            }
            else
            {
                return View();
            }
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
