using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAds.Helpers;

namespace WebAd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApiAdsController : ControllerBase
    {
        private readonly AdsServices _AdServices;
        private readonly CategoryServices _categoryServices;

        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;


        public ApiAdsController(AdsServices AdServices,
            CategoryServices categoryServices,
            UserManager<User> userManager,
            IWebHostEnvironment appEnvironment)
        {
            _AdServices = AdServices;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
            _categoryServices = categoryServices;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Ad>> GetAdUser(string userId)
        {
            return await _AdServices.Find(x => x.UserId == userId);
        }

        /// <summary>
        /// Ad Filter
        /// </summary>
        /// <param name="idCategory">Category ID</param>
        /// <param name="cityName">City ID</param>
        /// <returns>Get IEnumerable&lt;Ad&gt;</returns>
        [AllowAnonymous]
        [HttpGet("{idCategory}/{cityName?}")]
        public async Task<IEnumerable<Ad>> FilterAd(string? cityName, int idCategory = -1)
        {
            var res = new List<Ad>();

            if (idCategory <= 0 && cityName == null)
                res.AddRange(await _AdServices.GetAllAsync());
            else
            {

                if (cityName != null && idCategory <= 0)
                    res.AddRange(await _AdServices.Find(x => x.CityName == cityName));
                else
                {
                    var t = await _categoryServices.GetAsync(idCategory);
                    if (t != null)
                    {
                        res.AddRange(await UploadAdsAsync(t.Categories));
                        res.AddRange(t.Ads);
                        foreach (var item in res)
                        {
                            if (item.Categoty != null)
                                item.Categoty = null;
                        }
                    }
                    if (cityName != null)
                    {
                        foreach (var item in new List<Ad>(res))
                        {
                            if (item.CityName != cityName)
                                res.Remove(item);
                        }
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// Downloading subcategories and their ads.
        /// </summary>
        /// <param name="category"></param>
        /// <returns>Get IEnumerable&lt;Ad&gt;</returns>
        private async Task<IEnumerable<Ad>> UploadAdsAsync(List<Category> category)
        {
            var res = new List<Ad>();
            foreach (var item in category)
            {
                var t = await _categoryServices.GetAsync(item.Id);
                if (t.Ads != null)
                {
                    res.AddRange(item.Ads);
                }
                if (t.Categories != null)
                {
                    res.AddRange(await UploadAdsAsync(item.Categories));
                }
            }
            return res;
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromForm] Ad ad, IFormFile upload)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (ad.CategotyId <= 0 || ad == null || upload == null)
                    throw new Exception("Invalid parameter!");

                ad.UserId = user.Id;//Set user ID

                //Save Img
                string filePathDb = "/FilesDb/" + FilesHelper.RandomName() + ".png";
                if (await upload.SaveFile(_appEnvironment.WebRootPath + filePathDb))
                {
                    ad.PathImg = filePathDb;
                    //Save Ad to db
                    await _AdServices.AddAsync(ad, ad.CityName, ad.CategotyId);
                }

                return Ok("Ad has been added!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{idAd}")]
        public async Task<IActionResult> Remove(int idAd)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (idAd < 1)
                        throw new Exception("Incorrect idAd");

                var ads = await _AdServices.Find(x => x.UserId == user.Id && x.Id == idAd);

                if(ads != null && ads.Count() == 1)
                {
                    if (await _AdServices.RemoveAsync(idAd))
                        return Ok("The delete operation was succcessful!");
                    else
                        throw new Exception($"Failed to delete Ad(Id:{idAd})!");
                }
                else
                    throw new Exception("Not found Ad!");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm]Ad ad, IFormFile? upload)
        {
            try
            {
                if(upload != null)
                {
                    if (ad.PathImg != null)
                        FilesHelper.DeleteFile(_appEnvironment.WebRootPath + ad.PathImg);

                    //Save Img
                    string filePathDb = "/FilesDb/" + FilesHelper.RandomName() + ".png";
                    if (await upload.SaveFile(_appEnvironment.WebRootPath + filePathDb))
                    {
                        FilesHelper.DeleteFile(ad.PathImg);
                        ad.PathImg = filePathDb;
                    }
                }

                //Save Ad to db
                await _AdServices.UpdateAsync(ad);

                return Ok("The update operation was successful!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
