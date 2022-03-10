using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // account/register => GET
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // 
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            // save the password and account info with salt
            var user = await _accountService.CreateUser(model);
            return RedirectToAction("Login");

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var userLogedIn = await _accountService.ValidateUser(model.Email, model.Password);
            if (userLogedIn)
                return LocalRedirect("~/");
            else
            {
                return View();
            }
        }
    }
}
