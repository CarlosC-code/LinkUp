using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinkUp.Core.Application.Dtos.AccionBattleship;
using LinkUp.Core.Application.Dtos.BattleshipShip;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Domain.Entities.Battleship;
using LinkUp.Core.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Core.Application.Services
{
    public class BattleshipShipService : GenericService<BattleshipShip, BattleshipShipDto>, IBattleshipShipService
    {
        private readonly IBattleshipShipRepository _shipRepository;
        private readonly IMapper _mapper;

        public BattleshipShipService(IBattleshipShipRepository shipRepository, IMapper mapper)
            : base(shipRepository, mapper)
        {
            _shipRepository = shipRepository;
            _mapper = mapper;
        }

        public async Task<bool> PlaceShip(PlaceShipDto dto)
        {
            try
            {
                var ship = _mapper.Map<BattleshipShip>(dto);

                await _shipRepository.AddAsync(ship);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<BattleshipShipDto>> GetShipsByGame(int gameId, string userId)
        {
            try
            {
                var query = _shipRepository
                    .GetAllQuery()
                    .Where(s => s.GameId == gameId && s.OwnerUserId == userId);

                return await query
                    .ProjectTo<BattleshipShipDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
            catch
            {
                return [];
            }
        }
    }
}
