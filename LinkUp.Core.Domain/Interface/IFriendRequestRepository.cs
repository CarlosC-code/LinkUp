using LinkUp.Core.Domain.Entities.Social;

namespace LinkUp.Core.Domain.Interface
{
    public interface IFriendRequestRepository : IGenericRepository<FriendRequest>
    {
        Task<List<FriendRequest>> GetPendingReceivedAsync(string userId);
        Task<List<FriendRequest>> GetSentByUserAsync(string userId);
        Task<bool> HasActivePendingRequestAsync(string userA, string userB);
        Task<FriendRequest?> GetRequestBetweenUsersAsync(string senderUserId, string receiverUserId);
        Task<int> GetPendingCountAsync(string userId);
    }
}
