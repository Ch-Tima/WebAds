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
        private readonly IEmailSender _emailSender;

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

        public async Task<IActionResult> Index() => View();

        [HttpGet]
        public async Task<IEnumerable<Ad>> Sort(bool IsVerified) => await _adsServices.GetSort(x => x.IsVerified == IsVerified);

        [HttpPost]
        public async Task<IActionResult> ControlAdVisibility(int id)
        {
            try
            {
                var ad = await _adsServices.GetAsync(id);
                if (ad == null)
                    return BadRequest("Not Found!");

                ad.IsVerified = !ad.IsVerified;
                await _adsServices.UpdateAsync(ad);

                var user = await _userManager.FindByIdAsync(ad.UserId);
                await _emailSender.SendEmailAsync(
                    user.Email,
                    $"WebAds Ad({ad.Name})",
                    $"<p>For the ad \"{ad.Name}\" has been changed, the visibility status to {(ad.IsVerified ? "\"public\"" : "\"private\"")} </p>" + 
                    $"{(ad.IsVerified ? "" : "<p>Status: \"Violations of the rules\".</p><p>For more information write to us.</p>")}");

                return Ok(ad.IsVerified ? "Activated" : "Deactivated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}