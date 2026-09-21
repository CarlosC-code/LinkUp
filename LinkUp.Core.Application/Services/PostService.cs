
using AutoMapper;
using LinkUp.Core.Application.Dtos.Social;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Domain.Common.Enums;
using LinkUp.Core.Domain.Entities.Social;
using LinkUp.Core.Domain.Interface;

namespace LinkUp.Core.Application.Services
{
    public class PostService : GenericService<Post, PostDto>, IPostService
    {
        private readonly IPostRepository _posts;
        private readonly ICommentRepository _comments;
        private readonly IReactionRepository _reactions;
        private readonly IMapper _mapper;

        public PostService(
            IPostRepository posts,
            ICommentRepository comments,
            IReactionRepository reactions,
            IMapper mapper)
            : base(posts, mapper)
        {
            _posts = posts;
            _comments = comments;
            _reactions = reactions;
            _mapper = mapper;
        }

        public async Task<List<PostDto>> GetMyPostsAsync(string userId)
        {
            var list = await _posts.GetByUserIdAsync(userId);
            return await MapWithCommentsAndReactions(list, userId);
        }

        public async Task<List<PostDto>> GetFriendPostsAsync(List<string> friendIds, string currentUserId)
        {
            var list = await _posts.GetByFriendIdsAsync(friendIds);
            return await MapWithCommentsAndReactions(list, currentUserId);
        }

        public async Task<List<PostDto>> GetSpecificFriendPostsAsync(string friendUserId, string currentUserId)
        {
            var list = await _posts.GetBySpecificFriendAsync(friendUserId);
            return await MapWithCommentsAndReactions(list, currentUserId);
        }

        public async Task<PostDto?> CreateAsync(string userId, string content, int mediaType, string? imagePath, string? youTubeUrl)
        {
            if (string.IsNullOrWhiteSpace(content)) return null;
            var mt = (MediaType)mediaType;
            if (mt == MediaType.Image && string.IsNullOrWhiteSpace(imagePath)) return null;
            if (mt == MediaType.YouTube && string.IsNullOrWhiteSpace(youTubeUrl)) return null;

            var entity = new Post
            {
                UserId = userId,
                Content = content,
                MediaType = mt,
                ImagePath = imagePath,
                YouTubeUrl = youTubeUrl,
                PublishedAtUtc = DateTime.UtcNow
            };
            await _posts.AddAsync(entity);
            return _mapper.Map<PostDto>(entity);
        }

        public async Task<PostDto?> EditAsync(int postId, string userId, string content, int mediaType, string? imagePath, string? youTubeUrl)
        {
            var entity = await _posts.GetById(postId);
            if (entity == null || entity.UserId != userId) return null;

            entity.Content = content;
            entity.MediaType = (MediaType)mediaType;
            entity.ImagePath = imagePath;
            entity.YouTubeUrl = youTubeUrl;
            entity.UpdatedAtUtc = DateTime.UtcNow;

            await _posts.UpdateAsync(entity);
            return _mapper.Map<PostDto>(entity);
        }

        public async Task<bool> DeleteAsync(int postId, string userId)
        {
            var entity = await _posts.GetById(postId);
            if (entity == null || entity.UserId != userId) return false;

           
            await _posts.DeleteAsync(postId);
            return true;
        }

        
        private async Task<List<PostDto>> MapWithCommentsAndReactions(List<Post> posts, string currentUserId)
        {
            var dtos = _mapper.Map<List<PostDto>>(posts);

            foreach (var dto in dtos)
            {
                
                var allComments = await _comments.GetAllByPostIdAsync(dto.Id);
                var commentDtos = _mapper.Map<List<CommentDto>>(allComments);
                var lookup = commentDtos.ToLookup(c => c.ParentCommentId);
                List<CommentDto> Build(int? parentId) =>
                    lookup[parentId].Select(c => { c.Replies = Build(c.Id); return c; }).ToList();
                dto.Comments = Build(null);

                var rlist = await _reactions.GetAllList();
                var postReactions = rlist.Where(r => r.PostId == dto.Id).ToList();
                dto.Reactions = _mapper.Map<List<ReactionDto>>(postReactions);
            }
            return dtos;
        }
    }
}


