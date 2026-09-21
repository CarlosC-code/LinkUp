using LinkUp.Core.Domain.Entities.Social;
using LinkUp.Core.Domain.Interface;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class ReactionRepository : GenericRepository<Reaction>, IReactionRepository
    {
        private readonly LinkUpAppContext _context;

        public ReactionRepository(LinkUpAppContext context) : base(context)
        {
            _context = context;
        }

        // Obtener reacción de un usuario en una publicacion
        public async Task<Reaction?> GetByUserAndPostAsync(string userId, int postId)
        {
            return await _context.Reactions
                .FirstOrDefaultAsync(r => r.UserId == userId && r.PostId == postId);
        }

        // Verificar si ya existe reaccion
        public async Task<bool> ExistsAsync(string userId, int postId)
        {
            return await _context.Reactions
                .AnyAsync(r => r.UserId == userId && r.PostId == postId);
        }

        // Eliminar reacción de un usuario en una publicacion
        public async Task DeleteByUserAndPostAsync(string userId, int postId)
        {
            var reaction = await GetByUserAndPostAsync(userId, postId);
            if (reaction != null)
            {
                _context.Reactions.Remove(reaction);
                await _context.SaveChangesAsync();
            }
        }
    }
}