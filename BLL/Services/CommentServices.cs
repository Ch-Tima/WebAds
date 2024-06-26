﻿using DLL.Repository;
using Domain.Models;
using System.Linq.Expressions;

namespace BLL.Services
{
    public class CommentServices
    {
        private readonly CommentRepository _commentRepository;
        public CommentServices(CommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task AddAsync(Comment entity, int AdId)
        {
            entity.AdId = AdId;

            await _commentRepository.AddAsync(entity);
        }
        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _commentRepository.GetAllAsync();
        }
        public async Task<IEnumerable<Comment>> Find(Expression<Func<Comment, bool>> expression)
        {
            return await _commentRepository.Find(expression);
        }

        public async Task<Comment> GetAsync(int id)
        {
            return await _commentRepository.GetAsync(id);
        }
        public async Task<bool> RemoveAsync(int id)
        {
            return await _commentRepository.RemoveAsync(id);
        }

        public async Task UpdateAsync(Comment entity)
        {
            await _commentRepository.UpdateAsync(entity);
        }
    }
}
