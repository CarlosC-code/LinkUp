using LinkUp.Core.Domain.Entities.Social;

namespace LinkUp.Core.Domain.Interface
{
    public interface IFriendshipRepository : IGenericRepository<Friendship>
    {
        Task<List<string>> GetFriendIdsAsync(string userId);
        Task<bool> AreFriendsAsync(string userA, string userB);
        Task DeleteFriendshipAsync(string userA, string userB);
        Task<int> GetMutualFriendsCountAsync(string userA, string userB);
    }
}
