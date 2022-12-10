using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IEmailSender _emailService;

        public ProfileController(UserManager<User> userManager,
            AdsServices adsServices,
            UserServices userServices,
            IWebHostEnvironment appEnvironment, IEmailSender emailService)
        {
            _userManager = userManager;
            _adsServices = adsServices;
            _userServices = userServices;
            _appEnvironment = appEnvironment;
            _emailService = emailService;
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

            var ad = (List<Ad>)await _adsServices.Find(x => x.Id == idAd && x.UserId == user.Id);
            if (ad?.Count() > 0)
                return View(ad[0]);

            return Redirect("~/Profile");
        }

        public async Task<IActionResult> UpdateProfile()
        {
            var user = await _userServices.GetAsync(_userManager.GetUserId(User));
  
            //If email address isn't verified
            if (!user.EmailConfirmed)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmLink = Url.Action("Confirm", "EmailConfirm", 
                    values: new { area = "Identity", token = token, userEmail = user.Email }, 
                    Request.Scheme, Request.Host.Value);

                var msgHtml = $"<lable>Please click the link for confirm Email address:</lable><a href='{confirmLink}'>Confirm Email</a>";

                await _emailService.SendEmailAsync(user.Email, "Confirmation Email(WebAd)", msgHtml);

                ViewBag.Error = "Please click email and confirm your email address.";
                ViewBag.IsPublicProfile = false;
                return View(nameof(Index), user);
            }

            return View(user);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UpdateProfile(User model, IFormFile? file)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                user.CityName = model.CityName;
                user.IsMailing = model.IsMailing;
                user.TwoFactorEnabled = model.TwoFactorEnabled;

                if (file != null)
                {
                    string filePathDb = "/FilesDb/" + FilesHelper.RandomName() + ".png";
                    if (await file.SaveFile(_appEnvironment.WebRootPath + filePathDb))
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
