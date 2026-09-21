
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Domain.Common.Enums;
using LinkUp.Core.Domain.Interface;

namespace LinkUp.Core.Application.Services
{
    public class ReactionService : IReactionService
    {
        private readonly IReactionRepository _reactions;

        public ReactionService(IReactionRepository reactions)
        {
            _reactions = reactions;
        }

        public async Task<int?> ReactAsync(string userId, int postId, int reactionType)
        {
            var existing = await _reactions.GetByUserAndPostAsync(userId, postId);
            if (existing != null)
            {
                existing.Type = (ReactionType)reactionType;
                existing.UpdatedAtUtc = DateTime.UtcNow;
                await _reactions.UpdateAsync(existing);
                return existing.Id;
            }
            var entity = new Core.Domain.Entities.Social.Reaction
            {
                PostId = postId,
                UserId = userId,
                Type = (ReactionType)reactionType
            };
            await _reactions.AddAsync(entity);
            return entity.Id;
        }

        public async Task<bool> RemoveReactionAsync(string userId, int postId)
        {
            await _reactions.DeleteByUserAndPostAsync(userId, postId);
            return true;
        }
    }
}
