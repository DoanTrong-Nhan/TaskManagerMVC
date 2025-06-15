using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerMVC.Dto.Auth;
using TaskManagerMVC.Helpers; // ✅ THÊM using này
using TaskManagerMVC.Services.Interfaces;

namespace TaskManagerMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Task", "ListTask");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _authService.ValidateUserAsync(model);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            var principal = ClaimsPrincipalFactory.CreatePrincipal(user);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("ListTask", "Task");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
