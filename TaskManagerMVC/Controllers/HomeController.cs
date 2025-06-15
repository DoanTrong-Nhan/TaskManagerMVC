using Microsoft.AspNetCore.Mvc;
using System;

namespace TaskManagerMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error(int statusCode, string message)
        {
            ViewData["StatusCode"] = statusCode;
            ViewData["Message"] = message;
            return View();
        }
    }
}