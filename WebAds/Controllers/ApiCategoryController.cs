using BLL.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
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
        public async Task<IEnumerable<Category>> Get()
        {
            var f = await _categoryServices.GetAllAsync();

            foreach (var item in f)
            {
                if (item.Categors != null)
                    item.Categors = null;
            }
            
            var temp = new List<Category>(f);

            foreach (var item in f)
            {
                if (item.Categories != null)
                {
                    foreach (var category in item.Categories)
                    {
                        category.Categors = null;
                        temp.Remove(category);//нужна рекурсия для глубины 3...
                    }
                }
            }
            return temp;
        }
    }
}
