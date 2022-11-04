using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace WebAds.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private AdsServices _adsServices;

        public ProfileController(UserManager<User> userManager,
            AdsServices adsServices)
        {
            _userManager = userManager;
            _adsServices = adsServices;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userManager.GetUserAsync(User));
        }
        public async Task<IActionResult> AddNewAd()
        {
            return View(await _userManager.GetUserAsync(User));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAd(int idAd)
        {
            var user = await _userManager.GetUserAsync(User);

            var ad = (List<Ad>)await _adsServices.Find(x => x.Id == idAd && x.UserId == user.Id, true);
            if(ad != null && ad.Count() > 0)
                return View(ad[0]);

            return Redirect("~/Profile");
            
        }
    }
}
