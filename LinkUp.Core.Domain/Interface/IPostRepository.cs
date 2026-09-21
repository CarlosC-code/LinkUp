using LinkUp.Core.Domain.Entities.Social;

namespace LinkUp.Core.Domain.Interface
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetByUserIdAsync(string userId);
        Task<List<Post>> GetByFriendIdsAsync(List<string> friendIds);
        Task<List<Post>> GetBySpecificFriendAsync(string friendUserId);
    }
}
