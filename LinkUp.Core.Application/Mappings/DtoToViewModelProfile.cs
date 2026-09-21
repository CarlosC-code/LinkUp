

using AutoMapper;
using LinkUp.Core.Application.Dtos.Battleship;
using LinkUp.Core.Application.Dtos.Social;
using LinkUp.Core.Application.ViewModels.Battleship;
using LinkUp.Core.Application.ViewModels.Social;
using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Application.Mappings
{
    public class DtoToViewModelProfile : Profile
    {

        public DtoToViewModelProfile()
        {
            // Social
            CreateMap<PostDto, PostItemViewModel>()
                .ForMember(d => d.AuthorUserId, m => m.MapFrom(s => s.UserId))
                .ForMember(d => d.AuthorUserName, m => m.MapFrom(s => s.UserName))
                .ForMember(d => d.AuthorProfileImage, m => m.MapFrom(s => s.UserProfileImage))
                .ForMember(d => d.MediaType, m => m.MapFrom(s => (MediaType)s.MediaType))
                .ForMember(d => d.Likes, m => m.MapFrom(s => s.Likes))
                .ForMember(d => d.Dislikes, m => m.MapFrom(s => s.Dislikes));
            CreateMap<CommentDto, CommentThreadViewModel>();

            // Battleship
            CreateMap<BattleshipGameDto, ActiveGameItemViewModel>()
                .ForMember(d => d.GameId, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.OpponentUserId, m => m.MapFrom(s => s.OpponentUserId))
                .ForMember(d => d.OpponentUserName, m => m.MapFrom(s => s.OpponentUserName))
                .ForMember(d => d.OpponentProfileImage, m => m.MapFrom(s => s.OpponentProfileImage))
                .ForMember(d => d.StartedAtUtc, m => m.MapFrom(s => s.StartedAtUtc))
                .ForMember(d => d.IsMyTurn, m => m.MapFrom(s => s.IsMyTurn));

            CreateMap<BattleshipGameDto, HistoryGameItemViewModel>()
                .ForMember(d => d.GameId, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.OpponentUserName, m => m.MapFrom(s => s.OpponentUserName))
                .ForMember(d => d.StartedAtUtc, m => m.MapFrom(s => s.StartedAtUtc))
                .ForMember(d => d.FinishedAtUtc, m => m.MapFrom(s => s.FinishedAtUtc ?? DateTime.UtcNow))
                .ForMember(d => d.IsWinner, m => m.MapFrom(s => s.WinnerUserId != null && s.WinnerUserId == s.CreatorUserId))
                .ForMember(d => d.WinnerText, m => m.MapFrom(s => s.WinnerUserId));
        }

    }
}
