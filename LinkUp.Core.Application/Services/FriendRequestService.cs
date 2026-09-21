
using AutoMapper;
using LinkUp.Core.Application.Dtos.Social;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Domain.Common.Enums;
using LinkUp.Core.Domain.Entities.Social;
using LinkUp.Core.Domain.Interface;

namespace LinkUp.Core.Application.Services
{
    public class FriendRequestService : GenericService<FriendRequest, FriendRequestDto>, IFriendRequestService
    {
        private readonly IFriendRequestRepository _requests;
        private readonly IFriendshipRepository _friendships;
        private readonly IMapper _mapper;

        public FriendRequestService(
            IFriendRequestRepository requests,
            IFriendshipRepository friendships,
            IMapper mapper)
            : base(requests, mapper)
        {
            _requests = requests;
            _friendships = friendships;
            _mapper = mapper;
        }

        public async Task<List<FriendRequestDto>> GetPendingReceivedAsync(string userId)
            => _mapper.Map<List<FriendRequestDto>>(await _requests.GetPendingReceivedAsync(userId));

        public async Task<List<FriendRequestDto>> GetSentByUserAsync(string userId)
            => _mapper.Map<List<FriendRequestDto>>(await _requests.GetSentByUserAsync(userId));

        public Task<int> GetPendingCountAsync(string userId)
            => _requests.GetPendingCountAsync(userId);

        public async Task<bool> CanRequestAsync(string requesterId, string targetId)
        {
            if (await _friendships.AreFriendsAsync(requesterId, targetId)) return false;
            if (await _requests.HasActivePendingRequestAsync(requesterId, targetId)) return false;
            return true;
        }

        public async Task<int?> CreateAsync(string senderUserId, string receiverUserId)
        {
            if (!await CanRequestAsync(senderUserId, receiverUserId)) return null;

            var entity = new FriendRequest
            {
                SenderUserId = senderUserId,
                ReceiverUserId = receiverUserId,
                Status = FriendRequestStatus.Pending,
                SentAtUtc = DateTime.UtcNow
            };
            await _requests.AddAsync(entity);
            return entity.Id;
        }

        public async Task<bool> AcceptAsync(int requestId, string receiverUserId)
        {
            var req = await _requests.GetById(requestId);
            if (req == null || req.ReceiverUserId != receiverUserId || req.Status != FriendRequestStatus.Pending) return false;

            req.Status = FriendRequestStatus.Accepted;
            req.RespondedAtUtc = DateTime.UtcNow;
            await _requests.UpdateAsync(req);

            var friendship = new Friendship
            {
                UserAId = req.SenderUserId,
                UserBId = req.ReceiverUserId,
                SinceUtc = DateTime.UtcNow
            };
            await _friendships.AddAsync(friendship);
            return true;
        }

        public async Task<bool> RejectAsync(int requestId, string receiverUserId)
        {
            var req = await _requests.GetById(requestId);
            if (req == null || req.ReceiverUserId != receiverUserId || req.Status != FriendRequestStatus.Pending) return false;

            req.Status = FriendRequestStatus.Rejected;
            req.RespondedAtUtc = DateTime.UtcNow;
            await _requests.UpdateAsync(req);
            return true;
        }

        public async Task<bool> DeleteAsync(int requestId, string senderUserId)
        {
            var req = await _requests.GetById(requestId);
            if (req == null || req.SenderUserId != senderUserId) return false;

            await _requests.DeleteAsync(requestId);
            return true;
        }
    }
}
