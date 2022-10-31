using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace WebAd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly AdsServices _AdServices;

        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;
        public AdsController(AdsServices AdServices, 
            UserManager<User> userManager, 
            IWebHostEnvironment appEnvironment)
        {
            _AdServices = AdServices;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }
        [HttpGet]
        public async Task<IEnumerable<Ad>> Get(string userId)
        {
            return await _AdServices.Find(userId);
        }

        //[Authorize]
        [HttpPut]
        public async Task<IActionResult> Add([FromForm] Ad ad, IFormFile upload)
        {
            try
            {
                if (ad == null || upload == null)
                    throw new Exception("Ad Equals null");


                if(ad.CategotyId <= 0 || ad.CityId <= 0)
                    throw new Exception("Incorrect parameter!");

                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                    throw new Exception("Please LogIn to your account!");



                //Save Img
                var nameFile = HashTime() + ".png";
                await SaveFile(upload, nameFile);
                ad.PathImg = "/FilesDb/" + nameFile;

                //def
                ad.DateCreate = DateTime.Now;
                ad.IsVerified = false;
                ad.IsTop = false;
                ad.UserId = user.Id;

                await _AdServices.AddAsync(ad, ad.CityId, ad.CategotyId);

                return Ok("Ad has been added!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private async Task<bool> SaveFile(IFormFile file, string newName)
        {
            try
            {
                if (file == null)
                    return false;

                string path = "/FilesDb/" + newName;

                using (var fs = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string HashTime()
        {
            try
            {
                using (var sha = SHA256.Create())
                {
                    var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
                    var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                    return hash;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
