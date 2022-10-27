using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;
        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpGet]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userServices.GetAllAsync();
        }
    }
}
