using LinkUp.Core.Domain.Entities.Social;

namespace LinkUp.Core.Domain.Interface
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<List<Comment>> GetByPostIdAsync(int postId);
        Task<List<Comment>> GetRepliesByCommentIdAsync(int parentCommentId);
        Task<List<Comment>> GetAllByPostIdAsync(int postId);
    }
}
