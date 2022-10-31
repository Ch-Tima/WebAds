using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace WebAds.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentServices _commentServices;
        private readonly UserManager<User> _userManager;
        public CommentController(CommentServices commentServices,
            UserManager<User> userManager)
        {
            _commentServices = commentServices;
            _userManager = userManager;
        }
        [HttpGet("{id}")]
        public async Task<IEnumerable<Comment>> Get(int id)
        {
            return await _commentServices.Find(x => x.AdId == id);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Comment comment)
        {
            try
            {
                if (comment == null)
                    throw new Exception("comment equal null!");

                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                    throw new Exception("Please LogIn to your account!");

                comment.DateCreate = DateTime.Now;
                comment.UserId = user.Id;

                await _commentServices.AddAsync(comment, comment.AdId);

                return Ok("Ok");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
