using Final_Project.Models;
using Final_Project.Repositories.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using IAuthenticationService = Final_Project.Services.Interface.IAuthenticationService;

namespace Final_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepo _userRepo;

        public HomeController(IAuthenticationService authenticationService, IUserRepo userRepo)
        {
            _authenticationService = authenticationService;
            _userRepo = userRepo;
        }

        [Authorize]
        public IActionResult Index()
        {
            // AUTHORIZE is not working when the app is opened for the fist time and did not logout last time
            var username = HttpContext.Session.GetString("UserName");
            if (username == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string account, string password, string returnUrl)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                ViewBag.errMsg = "Please input account or password";
                return View("Login");
            }

            if (!_authenticationService.IsAuthentication(account, password))
            {
                ViewBag.errMsg = "Your account or password is wrong";
                return View("Login");
            }

            await SetAuthentication(account);
            SetLoginSession(account);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index");
        }

        public IActionResult GetUserName()
        {
            var username = HttpContext.Session.GetString("UserName");
            return Json(username);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task SetAuthentication(string account)
        {
            var claims = new[] { new Claim("UserName", account) };
            var claimsIdentity =
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(principal,
                           new AuthenticationProperties()
                           {
                               IsPersistent = false,
                           });
        }

        private void SetLoginSession(string account)
        {
            var uid = _userRepo.GetUser(account).UID;
            HttpContext.Session.SetString("UserName", account);
            HttpContext.Session.SetString("UID", uid);
        }
    }
}
