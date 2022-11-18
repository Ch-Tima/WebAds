using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text;
using WebAds.Models;
using Domain.Models;
using WebAd.Models;

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
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
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

                if (model.Password == null || model.Password.Length < 4)
                    return BadRequest("The password is short!");


                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (await _roleManager.FindByNameAsync(UserRole.User) == null)
                        await _roleManager.CreateAsync(new IdentityRole(UserRole.User));

                    await _userManager.AddToRoleAsync(user, UserRole.User);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmLink = Url.Action("Confirm", "EmailConfirm",
                        new { token = token, userEmail = user.Email }, Request.Scheme,
                        Request.Host.Value + "/Identity");

                    var msgHtml =
                        $"<lable>Please click the link for confirm Email address:</lable><a href='{confirmLink}'>Confirm Email</a>";

                    await _emailService.SendEmailAsync(user.Email, "Confirmation Email(WebAd)", msgHtml);

                    return RedirectToAction("Index", "Home");
                }

                return View(nameof(Register), result.Errors.First().Description);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> ExternaLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternaLoginCallback", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }
        public async Task<IActionResult> ExternaLoginCallback(string? returnUrl, string? remoteError)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var viewMode = new LoginViewModel()
            {
                ExternalAuthentication = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login), viewMode);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {

                ModelState.AddModelError(string.Empty, $"Error login information: {remoteError}");
                return View(nameof(Login), viewMode);
            }

            var singInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, bypassTwoFactor: true);

            if (singInResult.Succeeded)
                return Redirect("~/"); //return LocalRedirect(returnUrl);
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new User()
                        {
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Name),
                            Surname = info.Principal.FindFirstValue(ClaimTypes.Surname)??string.Empty,
                        };
                       var res =  await _userManager.CreateAsync(user);
                       if (res.Succeeded)
                       {
                           await _userManager.AddToRoleAsync(user, UserRole.User);
                           await _userManager.AddLoginAsync(user, info);
                           await _signInManager.SignInAsync(user, false);
                           return Redirect("~/");
                       }
                    }
                    else
                    {
                        return View("Error", new ErrorViewModel()
                        {
                            RequestId = "Susch email exists"
                        });
                    }
                }
            }
            return View("Error", new ErrorViewModel()
            {
                RequestId = "Please contct support"
            });
        }


        public async Task<IActionResult> Login() => View(new LoginViewModel()
        {
            ExternalAuthentication = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        });
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.IsPersistent, false);
                if (result.Succeeded)
                    return Redirect("~/");

                ViewBag.Error = "User not found!";
                return View(nameof(Login), new LoginViewModel()
                {
                    ExternalAuthentication = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
                });
            }
            ViewBag.Error = "Error!";
            return View(nameof(Login), new LoginViewModel()
            {
                ExternalAuthentication = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            });
        }


        public IActionResult ForgotPassword() => View();
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _userManager.IsEmailConfirmedAsync(user))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var link = Url.Action("ResetPassword", "Account", new { token = token, email = email }, Request.Scheme, Request.Host.Value + "/Identity");

                await _emailService.SendEmailAsync(email, "Reset Password(WebAds)", $"<a href='{link}'>Confirm Email</a>");

                return View(model: "Please, check email!");
            }
            return View(model: "Not found user!");
        }


        public IActionResult ResetPassword(string email, string token)
        {
            return View(model: new ResetPasswordViewModel()
            {
                Email = email,
                Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token))
            });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

                if (result.Succeeded)
                    return View(nameof(Login));
            }

            return View(nameof(Register));
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
