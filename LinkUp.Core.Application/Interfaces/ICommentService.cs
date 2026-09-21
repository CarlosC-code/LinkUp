using LinkUp.Core.Application.Dtos.Social;
using LinkUp.Core.Application.Interfaces;

namespace LinkUp.Core.Application.Interfaces
{
    public interface ICommentService : IGenericService<CommentDto>
    {

        Task<CommentDto?> AddAsync(string userId, int postId, string text, int? parentCommentId);
        Task<bool> EditAsync(int id, string userId, string text);
        Task<bool> DeleteAsync(int id, string userId);

    }
}