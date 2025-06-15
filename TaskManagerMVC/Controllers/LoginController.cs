using Microsoft.AspNetCore.Mvc;
using TaskManagerMVC.Services.Interfaces;
using TaskManagerMVC.JWT;
using TaskManagerMVC.Models;
using Microsoft.AspNetCore.Http;
using TaskManagerMVC.Dto.Auth;

namespace TaskManagerMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthService _authService;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public LoginController(IAuthService authService, JwtTokenGenerator jwtTokenGenerator)
        {
            _authService = authService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Gọi Login để lấy token, nếu không hợp lệ thì trả về null hoặc chuỗi rỗng
            var token = _authService.Login(model.Username, model.Password);
            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(model);
            }

            // Lưu token vào cookie
            Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(_jwtTokenGenerator.ExpiryHours)
            });

            return RedirectToAction("ListTask", "Task");
        }

    }
}
