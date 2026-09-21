
using AutoMapper;
using LinkUp.Core.Application.Dtos.Social;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Domain.Entities.Social;
using LinkUp.Core.Domain.Interface;

namespace LinkUp.Core.Application.Services
{
    public class CommentService : GenericService<Comment, CommentDto>, ICommentService
    {
        private readonly ICommentRepository _comments;
        private readonly IPostRepository _posts;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository comments, IPostRepository posts, IMapper mapper)
            : base(comments, mapper)
        {
            _comments = comments;
            _posts = posts;
            _mapper = mapper;
        }

        public async Task<CommentDto?> AddAsync(string userId, int postId, string text, int? parentCommentId)
        {
            var post = await _posts.GetById(postId);
            if (post == null) return null;

            var entity = new Comment
            {
                PostId = postId,
                UserId = userId,
                Text = text,
                ParentCommentId = parentCommentId
            };
            await _comments.AddAsync(entity);
            return _mapper.Map<CommentDto>(entity);
        }

        public async Task<bool> EditAsync(int id, string userId, string text)
        {
            var c = await _comments.GetById(id);
            if (c == null || c.UserId != userId) return false;

            c.Text = text;
            c.UpdatedAtUtc = DateTime.UtcNow;
            await _comments.UpdateAsync(c);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var c = await _comments.GetById(id);
            if (c == null || c.UserId != userId) return false;

            await _comments.DeleteAsync(id);
            return true;
        }
    }
}

