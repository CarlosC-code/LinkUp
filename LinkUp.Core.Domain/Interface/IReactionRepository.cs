using LinkUp.Core.Domain.Entities.Social;

namespace LinkUp.Core.Domain.Interface
{
    public interface IReactionRepository : IGenericRepository<Reaction>
    {
        Task<Reaction?> GetByUserAndPostAsync(string userId, int postId);
        Task<bool> ExistsAsync(string userId, int postId);
        Task DeleteByUserAndPostAsync(string userId, int postId);

    }
}
