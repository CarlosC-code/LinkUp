using LinkUp.Core.Domain.Entities.Social;
using LinkUp.Core.Domain.Interface;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class FriendshipRepository : GenericRepository<Friendship>, IFriendshipRepository
    {
        private readonly LinkUpAppContext _context;

        public FriendshipRepository(LinkUpAppContext context) : base(context)
        {
            _context = context;
        }

        // Obtener todos los amigos de un usuario (devuelve los IDs del otro usuario)
        public async Task<List<string>> GetFriendIdsAsync(string userId)
        {
            return await _context.Friendships
                .Where(f => f.UserAId == userId || f.UserBId == userId)
                .Select(f => f.UserAId == userId ? f.UserBId : f.UserAId)
                .ToListAsync();
        }

        // Verificar si dos usuarios son amigos
        public async Task<bool> AreFriendsAsync(string userA, string userB)
        {
            return await _context.Friendships
                .AnyAsync(f => (f.UserAId == userA && f.UserBId == userB)
                            || (f.UserAId == userB && f.UserBId == userA));
        }

        // Eliminar amistad entre dos usuarios
        public async Task DeleteFriendshipAsync(string userA, string userB)
        {
            var friendship = await _context.Friendships
                .FirstOrDefaultAsync(f => (f.UserAId == userA && f.UserBId == userB)
                                       || (f.UserAId == userB && f.UserBId == userA));
            if (friendship != null)
            {
                _context.Friendships.Remove(friendship);
                await _context.SaveChangesAsync();
            }
        }

        // Contar amigos en común entre dos usuarios
        public async Task<int> GetMutualFriendsCountAsync(string userA, string userB)
        {
            var friendsOfA = await GetFriendIdsAsync(userA);
            var friendsOfB = await GetFriendIdsAsync(userB);
            return friendsOfA.Intersect(friendsOfB).Count();
        }
    }
}