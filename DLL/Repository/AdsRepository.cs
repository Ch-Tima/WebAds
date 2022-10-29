
using DLL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace DLL.Repository
{
    public class AdRepository : IGenericRepository<Ad>
    {
        private readonly AdDbContext _dbContext;
        public AdRepository(AdDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddAsync(Ad entity)
        {
            if(entity != null)
            {
                await _dbContext.Ads.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Ad>> Find(Expression<Func<Ad, bool>> predicate)
        {
            return await _dbContext.Ads.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Ad>> GetAllAsync()
        {
            return await _dbContext.Ads.ToListAsync();
        }

        public async Task<Ad> GetAsync(int id)
        {
            return await _dbContext.Ads
                .Include(x => x.User)
                .Include(x => x.Categoty)
                .Include(x => x.City)
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Ad> GetOnlyAsync(int id)
        {
            return await _dbContext.Ads.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var Ad = await _dbContext.Ads.FirstOrDefaultAsync(x => x.Id == id);

            if (Ad != null)
            {
                _dbContext.Ads.Remove(Ad);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task UpdateAsync(Ad entity)
        {
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
