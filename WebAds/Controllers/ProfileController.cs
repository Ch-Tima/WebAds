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
        private readonly AdsServices _adsServices;
        private readonly UserServices _userServices;

        public ProfileController(UserManager<User> userManager,
            AdsServices adsServices,
            UserServices userServices)
        {
            _userManager = userManager;
            _adsServices = adsServices;
            _userServices = userServices;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.IsPublicProfile = false;

            var user = await _userManager.GetUserAsync(User);

            return View(await _userManager.GetUserAsync(User));
        }

        /// <summary>
        /// Public user profile
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Profile/Index/{userId}")]
        public async Task<IActionResult> Index(string userId)
        {
            ViewBag.IsPublicProfile = true;
            return View(await _userServices.GetAsync(userId));
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
        /// <summary>
        /// Developing!
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UpdateProfile()
        {
            return View(await _userManager.GetUserAsync(User));
        }

    }
}
