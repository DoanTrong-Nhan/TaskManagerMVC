using Microsoft.AspNetCore.Mvc;
using System;

namespace TaskManagerMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error(int statusCode, string message)
        {
            ViewBag.StatusCode = statusCode;
            ViewBag.Message = string.IsNullOrEmpty(message) ? "Có lỗi xảy ra. Vui lòng thử lại." : message;
            return View();
        }
    }
}