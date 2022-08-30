using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
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
        
        public async Task<ActionResult<Response>> RegisterUser(User newUser)
        {
            if (!ModelState.IsValid)
            {
                return new Response
                {
                    StatusCode = 400,
                    ResponseMessage = "Form details are invalid"
                };
            }

            Password passwordAndSalt = _passwordService.SaltAndHashPassword(newUser.Password);

            UserConfig userConfig = new UserConfig
            {
                Email = newUser.Email,
                PasswordHash = passwordAndSalt.HashedPassword,
                Salt = passwordAndSalt.Salt
            };

            try
            {
                HttpResponseMessage response = await _apiService.Post("user", userConfig);
                response.EnsureSuccessStatusCode();
                string jsonResponseBody = await response.Content.ReadAsStringAsync();

                Response endpointResponse = JsonSerializer.Deserialize<Response>(jsonResponseBody);

                return endpointResponse;
            } catch (Exception e)
            {
                Debug.WriteLine($"Failed when trying to create user, with exception {e}");
                return new Response
                {
                    StatusCode = 400,
                    ResponseMessage = "Failed to create user"
                };
            }
        }
    }
}
