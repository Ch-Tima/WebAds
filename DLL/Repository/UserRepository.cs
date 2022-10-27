﻿using DLL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DLL.Repository
{
    public class UserRepository : IGenericRepository<User>
    {
        private readonly AdDbContext _dbContext;
        public UserRepository(AdDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddAsync(User entity)
        {
            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            return await _dbContext.Users.Include(x => x.Ads).FirstOrDefaultAsync(x => x.Id == id.ToString());
        }
        public async Task<User> GetOnlyAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());
        }
        public async Task<bool> RemoveAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());

            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task UpdateAsync(User entity)
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
