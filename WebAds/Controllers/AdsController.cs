using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly AdsServices _AdServices;
        public AdsController(AdsServices AdServices)
        {
            _AdServices = AdServices;
        }
        [HttpGet]
        public async Task<IEnumerable<Ad>> Get()
        {
            return await _AdServices.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<Ad> Get(int id)
        {
            return await _AdServices.GetAsync(id);
        }
    }
}
