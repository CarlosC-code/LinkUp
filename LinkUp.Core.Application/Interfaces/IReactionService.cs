using LinkUp.Core.Application.Dtos.Social;
using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IReactionService 
    {

        Task<int?> ReactAsync(string userId, int postId, int reactionType);
        Task<bool> RemoveReactionAsync(string userId, int postId);

    }
}
