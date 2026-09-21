using LinkUp.Core.Application.Dtos.Social;
using LinkUp.Core.Application.Interfaces;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IPostService : IGenericService<PostDto>
    {

        Task<List<PostDto>> GetMyPostsAsync(string userId);
        Task<List<PostDto>> GetFriendPostsAsync(List<string> friendIds, string currentUserId);
        Task<List<PostDto>> GetSpecificFriendPostsAsync(string friendUserId, string currentUserId);
        Task<PostDto?> CreateAsync(string userId, string content, int mediaType, string? imagePath, string? youTubeUrl);
        Task<PostDto?> EditAsync(int postId, string userId, string content, int mediaType, string? imagePath, string? youTubeUrl);
        Task<bool> DeleteAsync(int postId, string userId);
    }

}
