using LinkUp.Core.Domain.Entities.Social;
using LinkUp.Core.Domain.Interface;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly LinkUpAppContext _context;

        public CommentRepository(LinkUpAppContext context) : base(context)
        {
            _context = context;
        }

        // Comentarios principales de una publicación (sin replies)
        public async Task<List<Comment>> GetByPostIdAsync(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId && c.ParentCommentId == null)
                .OrderBy(c => c.CreatedAtUtc)
                .ToListAsync();
        }

        // Replies de un comentario específico
        public async Task<List<Comment>> GetRepliesByCommentIdAsync(int parentCommentId)
        {
            return await _context.Comments
                .Where(c => c.ParentCommentId == parentCommentId)
                .OrderBy(c => c.CreatedAtUtc)
                .ToListAsync();
        }

        // Todos los comentarios y replies de una publicación (para cargarlos juntos)
        public async Task<List<Comment>> GetAllByPostIdAsync(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.CreatedAtUtc)
                .ToListAsync();
        }
    }
}