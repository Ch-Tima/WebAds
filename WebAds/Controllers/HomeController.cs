using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAd.Models;

namespace WebAd.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(UserServices userServices, AdsServices AdServices)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}