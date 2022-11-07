
using DLL.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Numerics;

namespace BLL.Services
{
    public class UserServices
    {
        private readonly UserRepository _userRepository;
        public UserServices(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task AddAsync(User entity)
        {
            await _userRepository.AddAsync(entity);
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetAsync(string id, bool fullLoad = false)
        {
            if (fullLoad)
                return await _userRepository.GetAsync(id);
            else
                return await _userRepository.GetOnlyAsync(id);
            
        }



        public async Task<bool> RemoveAsync(string id)
        {
            return await _userRepository.RemoveAsync(id);
        }

        public async Task UpdateAsync(User entity)
        {
            try
            {
                await _userRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

