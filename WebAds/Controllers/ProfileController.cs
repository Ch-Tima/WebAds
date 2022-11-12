using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAds.Helpers;

namespace WebAds.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AdsServices _adsServices;
        private readonly UserServices _userServices;

        private readonly IWebHostEnvironment _appEnvironment;

        public ProfileController(UserManager<User> userManager,
            AdsServices adsServices,
            UserServices userServices,
            IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _adsServices = adsServices;
            _userServices = userServices;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.IsPublicProfile = false;
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
            if (ad != null && ad.Count() > 0)
                return View(ad[0]);

            return Redirect("~/Profile");
        }

        public async Task<IActionResult> UpdateProfile()
        {
            return View(await _userManager.GetUserAsync(User));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(User model, IFormFile? file)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                user.UserName = model.UserName;
                user.Surname = model.Surname;
                user.IsMailing = model.IsMailing;

                if (file != null)
                {
                    string filePathDb = "/FilesDb/" + FilesHelper.RandomName() + ".png";
                    if (await file.SaveFile( _appEnvironment.WebRootPath + filePathDb))
                    {
                        FilesHelper.DeleteFile(user.IconPath);
                        user.IconPath = filePathDb;
                    }
                }

                await _userManager.UpdateAsync(user);

                return Redirect(nameof(Index));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

    }
}
