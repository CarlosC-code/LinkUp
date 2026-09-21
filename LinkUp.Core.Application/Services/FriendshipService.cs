
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Domain.Interface;

namespace LinkUp.Core.Application.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendships;

        public FriendshipService(IFriendshipRepository friendships)
        {
            _friendships = friendships;
        }

        public Task<List<string>> GetFriendIdsAsync(string userId)
            => _friendships.GetFriendIdsAsync(userId);

        public Task<bool> AreFriendsAsync(string userA, string userB)
            => _friendships.AreFriendsAsync(userA, userB);

        public Task DeleteFriendshipAsync(string userA, string userB)
            => _friendships.DeleteFriendshipAsync(userA, userB);

        public Task<int> GetMutualFriendsCountAsync(string userA, string userB)
            => _friendships.GetMutualFriendsCountAsync(userA, userB);
    }
}
