using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAds.Controllers
{
    public class HomeController : Controller
    {
        private readonly AdsServices _adsServices;
        private readonly UserManager<Domain.Models.User> _userManager;
        public HomeController(AdsServices adsServices,
            UserManager<User> userManager)
        {
            _adsServices = adsServices;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AdDetails(int id)
        {
            var ad = await _adsServices.GetAsync(id);
            if (ad != null)
            {
                var user = await _userManager.GetUserAsync(User);
                if (ad.IsVerified || (user != null && await _userManager.IsInRoleAsync(user, UserRole.Manager)))
                    return View(ad);
            }
            return Redirect("~/");
        }
    }
}