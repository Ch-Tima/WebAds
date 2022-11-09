using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using WebAds.Models;

namespace WebAds.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {   
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IEmailSender _emailService;

        public AccountController(SignInManager<User> manager, UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, IEmailSender emailService)
        {
            _signInManager = manager;
            _userManager = userManager;
            _roleManager = roleManager;

            _emailService = emailService;
        }

        public IActionResult Register() => View();
        public IActionResult Login() => View();

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new User()
            {
                Email = model.Email,
                UserName = model.Name,
                Surname = model.Surname,
                PhoneNumber = model.PhoneNumber,
                CityName = model.CityName,
                IsMailing = model.IsMailing,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if(await _roleManager.FindByNameAsync("user") == null)
                    await _roleManager.CreateAsync(new IdentityRole("user"));

                await _userManager.AddToRoleAsync(user, "user");
                await _signInManager.SignInAsync(user, isPersistent: false);

                var t1 = Request.Scheme;
                var t2 = Request.Host.Value;

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmLink = Url.Action("Confirm", "EmailConfirm", new { token = token, userEmail = user.Email }, Request.Scheme, Request.Host.Value + "/Identity");

                var link = $"<lable>Please click the link for confirm Email address:</lable><a href='{confirmLink}'>Confirm Email</a>";

                await _emailService.SendEmailAsync(user.Email, "Confirmation Email(WebAd)", link);

                return RedirectToAction("Index", "Home");
            }
            
            return View(nameof(Register), result.Errors.First().Description);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.IsPersistent, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(nameof(Login), "User not found!");
                }
            }
            return View(nameof(Login), "Error!");
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
