using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAds.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiCategoryController : ControllerBase
    {
        private readonly CategoryServices _categoryServices;
        public ApiCategoryController(CategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        [HttpGet]
        public async Task<IEnumerable<Category>> GetMainCategories()
        {
            return await _categoryServices.GetMainCategoriesAsync();
        }
    }
}
