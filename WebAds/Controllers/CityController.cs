using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAds.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly CityServices _cityServices; 
        public CityController(CityServices cityServices)
        {
            _cityServices = cityServices;
        }
        [HttpGet]
        public async Task<IEnumerable<City>> Get()
        {
            return await _cityServices.GetAllAsync();
        }
    }
}
