using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAds.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApiFavouritesAdController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly FavouritesAdService _favouritesAdService;

        public ApiFavouritesAdController(UserManager<User> userManager,
            FavouritesAdService favouritesAdService)
        {
            _userManager = userManager;
            _favouritesAdService = favouritesAdService;
        }

        [HttpGet]
        public async Task<IEnumerable<FavouritesAd>> GetFavouriteAds()
        {
            var user = await _userManager.GetUserAsync(User);
            return await _favouritesAdService.Find(x => x.UserId == user.Id && x.Ad.IsVerified);
        }

        [HttpPut]
        public async Task<IActionResult> Add([FromBody] int adId)
        {
            try
            {
                var result = await _favouritesAdService.AddAsync(_userManager.GetUserId(User), adId);

                if (result)
                    return Ok();
                else
                    return BadRequest("Failed to add!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{adId:int}")]
        public async Task<IActionResult> Remove(int adId)
        {
            try
            {
                var result = await _favouritesAdService.RemoveAsync(_userManager.GetUserId(User), adId);

                if (result)
                    return Ok();
                else
                    return BadRequest("Failed to delete!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
