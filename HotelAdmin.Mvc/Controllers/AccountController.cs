using HotelAdmin.Mvc.Interfaces;
using HotelAdmin.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelAdmin.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApiService _apiService;

        public AccountController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel() 
            {
                UserName = "admin", // default username for testing
                Password = "password" // default password for testing
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var loginRequest = new
            {
                username = model.UserName, // map email → username
                password = model.Password
            };

            var response = await _apiService.PostAsync<AuthResponse>("auth/login", loginRequest);

            if (!string.IsNullOrEmpty(response.Token))
            {
                HttpContext.Session.SetString("JWToken", response.Token);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid login";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
