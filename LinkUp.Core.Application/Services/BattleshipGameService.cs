using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinkUp.Core.Application.Dtos.AccionBattleship;
using LinkUp.Core.Application.Dtos.BattleshipGame;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Domain.Common.Enums;
using LinkUp.Core.Domain.Entities.Battleship;
using LinkUp.Core.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Core.Application.Services
{
    public class BattleshipGameService : GenericService<BattleshipGame, BattleshipGameDto>, IBattleshipGameService
    {
        private readonly IBattleshipGameRepository _gameRepository;
        private readonly IMapper _mapper;

        public BattleshipGameService(IBattleshipGameRepository gameRepository, IMapper mapper)
            : base(gameRepository, mapper)
        {
            _gameRepository = gameRepository;
            _mapper = mapper;
        }

        public async Task<BattleshipGameDto?> CreateGame(CreateBattleshipGameDto dto)
        {
            try
            {
                var game = new BattleshipGame
                {
                    CreatorUserId = dto.CreatorUserId,
                    OpponentUserId = dto.OpponentUserId,
                    Status = GameStatus.InProgress
                };

                var result = await _gameRepository.AddAsync(game);

                return _mapper.Map<BattleshipGameDto>(result);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> SurrenderGame(int gameId, string userId)
        {
            try
            {
                var game = await _gameRepository.GetById(gameId);

                if (game == null) return false;

                game.Status = GameStatus.Finished;
                game.WinnerUserId = game.CreatorUserId == userId
                    ? game.OpponentUserId
                    : game.CreatorUserId;

                await _gameRepository.UpdateAsync(gameId, game);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
