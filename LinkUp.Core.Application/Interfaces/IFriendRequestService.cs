using LinkUp.Core.Application.Dtos.Social;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IFriendRequestService : IGenericService<FriendRequestDto>
    {

        Task<List<FriendRequestDto>> GetPendingReceivedAsync(string userId);
        Task<List<FriendRequestDto>> GetSentByUserAsync(string userId);
        Task<int> GetPendingCountAsync(string userId);
        Task<bool> CanRequestAsync(string requesterId, string targetId);
        Task<int?> CreateAsync(string senderUserId, string receiverUserId);
        Task<bool> AcceptAsync(int requestId, string receiverUserId);
        Task<bool> RejectAsync(int requestId, string receiverUserId);
        Task<bool> DeleteAsync(int requestId, string senderUserId);

    }
}
