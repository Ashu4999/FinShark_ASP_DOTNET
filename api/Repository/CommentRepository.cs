using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository (ApplicationDBContext context) {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);  
            await _context.SaveChangesAsync();
            return commentModel;        
        }

        public async Task<List<Comment>> GetAllAsync() {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto updateCommentRequestDto)
        {
            var foundComment = await _context.Comments.FirstOrDefaultAsync(s => s.Id == id);

            if (foundComment == null) {
                return null;
            }

            foundComment.Title = updateCommentRequestDto.Title;
            foundComment.Content = updateCommentRequestDto.Content;

            await _context.SaveChangesAsync();
            return foundComment;
        }
    }
}