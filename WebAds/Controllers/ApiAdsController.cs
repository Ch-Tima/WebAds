using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAds.Helpers;

namespace WebAds.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApiAdsController : ControllerBase
    {
        private readonly AdsServices _adServices;
        private readonly CategoryServices _categoryServices;
        private readonly CityServices _cityServices;

        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;

        public ApiAdsController(AdsServices adServices,
            CategoryServices categoryServices,
            CityServices cityServices,
            UserManager<User> userManager,
            IWebHostEnvironment appEnvironment)
        {
            _adServices = adServices;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
            _categoryServices = categoryServices;
            _cityServices = cityServices;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Ad>> GetAdUser(string userId)
        {
            return await _adServices.FindOnly(x => x.UserId == userId);
        }

        /// <summary>
        /// Ad Filter
        /// </summary>
        /// <param name="idCategory">Category ID</param>
        /// <param name="cityName">City ID</param>
        /// <returns>Get IEnumerable&lt;Ad&gt;</returns>
        [AllowAnonymous]
        [HttpGet("{idCategory:int}/{cityName?}")]
        public async Task<IEnumerable<Ad>> FilterAd(string? cityName, int idCategory = -1)
        {
            return await _adServices.FilterAd(cityName, idCategory);
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromForm] Ad ad, IFormFile upload)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (!await _categoryServices.IsExists(ad.CategotyId) || !await _cityServices.IsExists(ad.CityName))
                    return BadRequest("Invalid parameter!"); //throw new Exception("Invalid parameter!");

                ad.UserId = user.Id;//Set user ID

                //Save Img
                var filePathDb = "/FilesDb/" + FilesHelper.RandomName() + ".png";
                if (await upload.SaveFile(_appEnvironment.WebRootPath + filePathDb))
                {
                    ad.PathImg = filePathDb;
                    await _adServices.AddAsync(ad, ad.CityName, ad.CategotyId);//Save 'ad' to database
                }

                return Ok("Ad has been added!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{idAd:int}")]
        public async Task<IActionResult> Remove(int idAd)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var ads = await _adServices.FindOnly(x => x.UserId == user.Id && x.Id == idAd);
                if (ads.Count() == 1)
                {
                    if (await _adServices.RemoveAsync(idAd))
                        return Ok("The delete operation was successful!");
                }
                return BadRequest("Not found Ad!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] Ad ad, IFormFile? upload)
        {
            try
            {
                if (upload != null)//Update ad icon
                {
                    if (ad.PathImg != null)//Delete old ad icon
                        FilesHelper.DeleteFile(_appEnvironment.WebRootPath + ad.PathImg);

                    //Save new ad icon
                    string filePathDb = "/FilesDb/" + FilesHelper.RandomName() + ".png";
                    if (await upload.SaveFile(_appEnvironment.WebRootPath + filePathDb))
                        ad.PathImg = filePathDb;
                }
                ad.IsVerified = false;
                //Save Ad to db
                await _adServices.UpdateAsync(ad);
                return Ok("The update operation was successful!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}