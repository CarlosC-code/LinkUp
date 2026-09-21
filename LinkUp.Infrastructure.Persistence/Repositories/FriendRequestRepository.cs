using LinkUp.Core.Domain.Common.Enums;
using LinkUp.Core.Domain.Entities.Social;
using LinkUp.Core.Domain.Interface;
using LinkUp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repositories
{
    public class FriendRequestRepository : GenericRepository<FriendRequest>, IFriendRequestRepository
    {
        private readonly LinkUpAppContext _context;

        public FriendRequestRepository(LinkUpAppContext context) : base(context)
        {
            _context = context;
        }

        // Solicitudes recibidas pendientes por el usuario
        public async Task<List<FriendRequest>> GetPendingReceivedAsync(string userId)
        {
            return await _context.FriendRequests
                .Where(r => r.ReceiverUserId == userId && r.Status == FriendRequestStatus.Pending)
                .OrderByDescending(r => r.SentAtUtc)
                .ToListAsync();
        }

        // Solicitudes enviadas por el usuario
        public async Task<List<FriendRequest>> GetSentByUserAsync(string userId)
        {
            return await _context.FriendRequests
                .Where(r => r.SenderUserId == userId)
                .OrderByDescending(r => r.SentAtUtc)
                .ToListAsync();
        }

        // Verificar si ya existe solicitud activa (pendiente) entre dos usuarios en cualquier direccion
        public async Task<bool> HasActivePendingRequestAsync(string userA, string userB)
        {
            return await _context.FriendRequests
                .AnyAsync(r => r.Status == FriendRequestStatus.Pending
                            && ((r.SenderUserId == userA && r.ReceiverUserId == userB)
                             || (r.SenderUserId == userB && r.ReceiverUserId == userA)));
        }

        // Obtener solicitud específica entre dos usuarios
        public async Task<FriendRequest?> GetRequestBetweenUsersAsync(string senderUserId, string receiverUserId)
        {
            return await _context.FriendRequests
                .FirstOrDefaultAsync(r => r.SenderUserId == senderUserId
                                       && r.ReceiverUserId == receiverUserId);
        }

        // Contar solicitudes pendientes recibidas (para el badge del menu)
        public async Task<int> GetPendingCountAsync(string userId)
        {
            return await _context.FriendRequests
                .CountAsync(r => r.ReceiverUserId == userId
                              && r.Status == FriendRequestStatus.Pending);
        }
    }
}