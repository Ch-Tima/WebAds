using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAd.Models;

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

            //await _cityServices.AddAsync(new Domain.Models.City()
            //{
            //    Name = "Name_0A",
            //    Region = "Region_A"
            //});
            //await _cityServices.AddAsync(new Domain.Models.City()
            //{
            //    Name = "Name_1A",
            //    Region = "Region_A"
            //});
            //await _cityServices.AddAsync(new Domain.Models.City()
            //{
            //    Name = "Name_0B",
            //    Region = "Region_B"
            //});
            //await _cityServices.AddAsync(new Domain.Models.City()
            //{
            //    Name = "Name_1B",
            //    Region = "Region_B"
            //});
            //await _cityServices.AddAsync(new Domain.Models.City()
            //{
            //    Name = "Name_99X",
            //    Region = "Region_X"
            //});

            //await _categoryServices.AddAsync(new Domain.Models.Category()
            //{
            //    Name = "Category_EE",

            //});
            //await _categoryServices.AddAsync(new Domain.Models.Category()
            //{
            //    Name = "Category_CC",
            //});
            //await _categoryServices.AddAsync(new Domain.Models.Category()
            //{
            //    Name = "Category_BB",
            //});
            //await _categoryServices.AddAsync(new Domain.Models.Category()
            //{
            //    Name = "Category_XX",
            //});


            //await _categoryServices.AddAsync(new Domain.Models.Category()
            //{
            //    Name = "Category_E1",
            //    CategoryId = 1
            //});
            //await _categoryServices.AddAsync(new Domain.Models.Category()
            //{
            //    Name = "Category_E2",
            //    CategoryId = 1
            //});

            //await _categoryServices.AddAsync(new Domain.Models.Category()
            //{
            //    Name = "Category_B1",
            //    CategoryId = 3
            //});

            //await _categoryServices.AddAsync(new Domain.Models.Category()
            //{
            //    Name = "Category_B1_1",
            //    CategoryId = 6
            //});
            //await _categoryServices.AddAsync(new Domain.Models.Category()
            //{
            //    Name = "Category_B1_2",
            //    CategoryId = 6
            //});
            //await _categoryServices.AddAsync(new Domain.Models.Category()
            //{
            //    Name = "Category_B1_3",
            //    CategoryId = 6
            //});

            //await _categoryServices.AddAsync(new Domain.Models.Category()
            //{
            //    Name = "Category_B1_2_A",
            //    CategoryId = 8
            //});
            //await _categoryServices.AddAsync(new Domain.Models.Category()
            //{
            //    Name = "Category_B1_2_B",
            //    CategoryId = 8
            //});
            //await _categoryServices.AddAsync(new Domain.Models.Category()
            //{
            //    Name = "Category_B1_2_C",
            //    CategoryId = 8
            //});

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