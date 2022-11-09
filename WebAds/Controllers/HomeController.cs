using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAd.Controllers
{
    public class HomeController : Controller
    {
        private readonly CityServices _cityServices;
        private readonly CategoryServices _categoryServices;
        private readonly AdsServices _adsServices;
        public HomeController(AdsServices adsServices,
            CityServices cityServices, CategoryServices categoryServices)
        {
            _adsServices = adsServices;

            _cityServices = cityServices;
            _categoryServices = categoryServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 0)
        {
            return View(await _adsServices.GetAllAsync());
        }
        [HttpGet]
        public async Task<IActionResult> AdDetails(int id)
        {
            var ad = await _adsServices.GetAsync(id, true);

            if (ad == null)
                return Redirect("~/");
            else
                return View(ad);
        }
    }
}