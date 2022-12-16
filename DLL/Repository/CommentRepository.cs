using DLL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DLL.Repository
{
    public class CommentRepository : IGenericRepository<Comment, int>
    {
        private readonly AdDbContext _dbContext;
        public CommentRepository(AdDbContext context)
        {
            _dbContext = context;
        }
        public async Task AddAsync(Comment entity)
        {
            await _dbContext.Comments.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> FindOnly(Expression<Func<Comment, bool>> expression)
        {
            return await _dbContext.Comments.Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<Comment>> Find(Expression<Func<Comment, bool>> expression)
        {
            return await _dbContext.Comments.Select(x => new Comment()
            {
                UserId = x.UserId,
                User = new User()
                {
                    UserName = x.User.UserName,
                    Surname = x.User.Surname
                },
                AdId = x.AdId,
                Content = x.Content,
                DateCreate = x.DateCreate,
                Id = x.Id
            }).Where(expression).ToListAsync(); ;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _dbContext.Comments.ToListAsync();
        }

        public async Task<Comment> GetAsync(int id)
        {
            return await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (comment != null)
            {
                _dbContext.Comments.Remove(comment);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task UpdateAsync(Comment entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}