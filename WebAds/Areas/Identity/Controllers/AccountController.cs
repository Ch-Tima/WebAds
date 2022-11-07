using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAds.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserServices _userServices;        
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserServices userServices, SignInManager<User> manager,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userServices = userServices;
            _signInManager = manager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Register() => View();
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            
            if (result.Succeeded)
            {
                //await _userManager.AddToRoleAsync(_userServices.Fin, "user");

                return View("Login");
            }
            else
                return View((object)result.Errors.First().Description);
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user, bool isPersistent)
        {
            try
            {
                if (user.UserName == null || user.PasswordHash == null)
                    throw new Exception("UserName or Password equals null!");

                var result = await _signInManager.PasswordSignInAsync(user.UserName, user.PasswordHash, isPersistent, lockoutOnFailure: true);

                if (result != null && result.Succeeded)
                    return Redirect("~/Home");
                else
                    throw new Exception("Can't find the user!");
            }
            catch (Exception ex)
            {
                return View(viewName: nameof(Login), model: ex.Message);
            }
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
