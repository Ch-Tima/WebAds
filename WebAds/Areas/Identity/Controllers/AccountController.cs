using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace WebAds.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserServices _userServices;
        
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;


        public AccountController(UserServices userServices, 
            SignInManager<User> manager,
            UserManager<User> userManager)
        {
            _userServices = userServices;
            _signInManager = manager;
            _userManager = userManager;


        }

        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(User user, IFormFile? file)
        {

            user.IconPath = "img/defUser.png";
            var result = await _userManager.CreateAsync(user, user.PasswordHash);

            if (result.Succeeded)
            {
                return View();
            }
            else
            {
                return View("Login");
            }
        }

        [HttpPost]//logologi417@gmail.com
        public async Task<IActionResult> Login(User user, bool isPersistent)
        {
            try
            {

                var result = await _signInManager.PasswordSignInAsync(user.UserName, user.PasswordHash, isPersistent, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var t = 1;
                }
                else
                {
                    var t = 0;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Redirect("~/Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            //delete local cookie
            await _signInManager.SignOutAsync();

            return Redirect("~/Home");
        }

    }
}
