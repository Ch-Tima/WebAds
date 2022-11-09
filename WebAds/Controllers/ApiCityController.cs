using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAds.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiCityController : ControllerBase
    {
        private readonly CityServices _cityServices; 
        public ApiCityController(CityServices cityServices)
        {
            _cityServices = cityServices;
        }
        [HttpGet]
        public async Task<IEnumerable<City>> Get()
        {  
            var result = await _cityServices.GetAllAsync();
            foreach (var item in result)
                if(item.Ads.Count() > 0)
                    foreach (var ad in item.Ads)
                        ad.City = null;

            return result;
        }
    }
}