using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
namespace WebAds.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = UserRole.Manager)]
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CityServices _cityServices;
        private readonly CategoryServices _categoryServices;
        private readonly AdsServices _adsServices;
        private readonly  IEmailSender _emailSender;
        public HomeController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            CityServices cityServices, CategoryServices categoryServices, AdsServices adsServices,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _cityServices = cityServices;
            _categoryServices = categoryServices;
            _adsServices = adsServices;

            _emailSender = emailSender;
            
        }

        public async Task<IActionResult> Index()
        {
            return View(await _adsServices.GetAllAsync());
        }

        public async Task<IActionResult> ActivAd(int id)
        {
            var ad = await _adsServices.GetAsync(id);
            if(ad == null)
                return BadRequest("Not Found!");

            if (!ad.IsVerified)
            {
                ad.IsVerified = true;
                await _adsServices.UpdateAsync(ad);

                var user = await _userManager.FindByIdAsync(ad.UserId);
                await _emailSender.SendEmailAsync(user.Email, $"WebAds Ad({ad.Name})", $"<h3>Ad \"{ad.Name}\" has been activated!</h3>");

                return Ok($"Ad \"ID:{ad.Id}\" has been activated!");
            }

            return BadRequest("This ad is already active.");
        }
        public async Task<IActionResult> DeactivatedAd(int id)
        {
            var ad = await _adsServices.GetAsync(id);
            if (ad == null)
                return BadRequest("Not Found!");


            if (ad.IsVerified)
            {
                ad.IsVerified = false;
                await _adsServices.UpdateAsync(ad);

                var user = await _userManager.FindByIdAsync(ad.UserId);
                await _emailSender.SendEmailAsync(user.Email, $"WebAds Ad({ad.Name})", $"<h3>Ad \"{ad.Name}\" has been deactivate!</h3>");

                return Ok($"Ad \"ID:{ad.Id}\" has been Deactivated!");
            }

            return BadRequest("This ad is already deactivate.");
        }

    }
}