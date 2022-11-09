using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAds.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class EmailConfirmController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly IEmailSender _emailService;

        public EmailConfirmController(UserManager<User> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailService = emailSender;
        }
        [HttpGet]
        public async Task<IActionResult> Confirm(string token, string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if(result.Succeeded)
                {
                    await _emailService.SendEmailAsync(userEmail, "Confirmation Email(WebAd)", "<p>Your email has been verified!</p>");
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }
    }
}
