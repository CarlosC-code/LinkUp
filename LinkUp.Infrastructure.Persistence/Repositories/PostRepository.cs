using LinkUp.Core.Domain.Entities.Social;
using LinkUp.Core.Domain.Interface;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly LinkUpAppContext _context;

        public PostRepository(LinkUpAppContext context) : base(context)
        {
            _context = context;
        }

        // Publicaciones propias del usuario, de mas reciente a mas antigua
        public async Task<List<Post>> GetByUserIdAsync(string userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAtUtc)
                .ToListAsync();
        }

        // Publicaciones de una lista de amigos (pantalla Amigos)
        public async Task<List<Post>> GetByFriendIdsAsync(List<string> friendIds)
        {
            return await _context.Posts
                .Where(p => friendIds.Contains(p.UserId))
                .OrderByDescending(p => p.CreatedAtUtc)
                .ToListAsync();
        }

        // Publicaciones de un amigo especifico (al hacer clic en su nombre)
        public async Task<List<Post>> GetBySpecificFriendAsync(string friendUserId)
        {
            return await _context.Posts
                .Where(p => p.UserId == friendUserId)
                .OrderByDescending(p => p.CreatedAtUtc)
                .ToListAsync();
        }
    }
}