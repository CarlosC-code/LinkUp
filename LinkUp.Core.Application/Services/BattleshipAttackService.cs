using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinkUp.Core.Application.Dtos.BattleshipAttack;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Domain.Entities.Battleship;
using LinkUp.Core.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Core.Application.Services
{
    public class BattleshipAttackService : GenericService<BattleshipAttack, BattleshipAttackDto>, IBattleshipAttackService
    {
        private readonly IBattleshipAttackRepository _repository;
        private readonly IMapper _mapper;

        public BattleshipAttackService(IBattleshipAttackRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<BattleshipAttackDto>> GetByGame(int gameId)
        {
            try
            {
                var query = _repository
                    .GetAllQuery()
                    .Where(a => a.GameId == gameId);

                return await query
                    .ProjectTo<BattleshipAttackDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
            catch
            {
                return [];
            }
        }
    }
}
