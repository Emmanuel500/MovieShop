using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            if (userLogedIn != null)
            {
                // create an authentication cookie and store some claims information in the cookie
                // user related information
                // Driving Licence
                // First Name, Last Name, Date Of Birth, Location

                // create claims object to store user claims information

                var claims = new List<Claim> {
                    new Claim( ClaimTypes.Email, userLogedIn.Email ),
                    new Claim( ClaimTypes.NameIdentifier, userLogedIn.Id.ToString() ),
                    new Claim(  ClaimTypes.GivenName, userLogedIn.FirstName ),
                    new Claim(  ClaimTypes.Surname, userLogedIn.LastName ),
                    new Claim(  ClaimTypes.DateOfBirth, userLogedIn.DateOfBirth.ToShortDateString() ),
                    new Claim ("FullName", userLogedIn.FirstName + "," + userLogedIn.LastName),
                    new Claim ("Language", "en")

                };

                // identity object
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // create the cookie
                // SignInAsync

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return LocalRedirect("~/");
            }
            else
            {
                return View();
            }
        }
    }
}
