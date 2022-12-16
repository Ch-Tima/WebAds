using DLL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class AdsServices
    {
        private readonly AdRepository _adRepository;
        private readonly CategoryRepository _categoryRepository;
        public AdsServices(AdRepository adRepository, 
            CategoryRepository categoryRepository)
        {
            _adRepository = adRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task AddAsync(Ad entity, string cityName, int categoryId)
        {
            entity.CityName = cityName;
            entity.CategotyId = categoryId;

            await _adRepository.AddAsync(entity);
        }
        public async Task<IEnumerable<Ad>> GetAllAsync()
        {
            return await _adRepository.GetAllAsync();
        }
        public async Task<IEnumerable<Ad>> Find(Expression<Func<Ad, bool>> expression)
        {
            return await _adRepository.Find(expression);
        }
        public async Task<IEnumerable<Ad>> FindOnly(Expression<Func<Ad, bool>> expression)
        {
            return await _adRepository.FindOnly(expression);
        }

        public async Task<Ad> GetAsync(int id)
        {
            return await _adRepository.GetAsync(id);
        }
        public async Task<Ad> GetOnlyAsync(int id)
        {
            return await _adRepository.GetOnlyAsync(id);
        }
        public async Task<bool> RemoveAsync(int id)
        {
            return await _adRepository.RemoveAsync(id);
        }

        public async Task UpdateAsync(Ad entity)
        {
            await _adRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="idCategory"></param>
        /// <returns>Get IEnumerable&lt;Ad&gt;</returns>
        public async Task<IEnumerable<Ad>> FilterAd(string? cityName, int idCategory = -1)
        {
            var res = new List<Ad>();

            if (idCategory <= 0 && cityName == null)
                res.AddRange(await this.Find(x => x.IsVerified));
            else
            {

                if (cityName != null && idCategory <= 0)
                    res.AddRange(await this.Find(x => x.CityName == cityName && x.IsVerified));
                else
                {
                    var category = await _categoryRepository.GetAsync(idCategory);
                    if (category != null)
                    {
                        res.AddRange(await UploadAdsAsync(category.Categories.ToList()));
                        res.AddRange(category.Ads.ToList().FindAll(x => x.IsVerified));

                        category.Ads.Clear();
                        category.Categories.Clear();
                    }

                    if (cityName != null) 
                    {
                        foreach (var item in new List<Ad>(res).Where(item => item.CityName != cityName))
                            res.Remove(item);
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Downloading subcategories and their ads.
        /// </summary>
        /// <param name="categories"></param>
        /// <returns>Get IEnumerable&lt;Ad&gt;</returns>
        private async Task<IEnumerable<Ad>> UploadAdsAsync(List<Category> categories)
        {
            var resultAds = new List<Ad>();
            foreach (var item in categories)
            {
                var category = await _categoryRepository.GetAsync(item.Id);

                if (category?.Ads?.Count > 0)
                    resultAds.AddRange(item.Ads.ToList().FindAll(x => x.IsVerified));

                if (category?.Categories?.Count > 0)
                    resultAds.AddRange(await UploadAdsAsync(item.Categories.ToList()));

                item.Categories.Clear();
                item.Ads.Clear();
            }
            return resultAds;
        }
    }
}
