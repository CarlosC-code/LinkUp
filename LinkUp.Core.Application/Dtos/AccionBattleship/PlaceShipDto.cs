using LinkUp.Core.Domain.Common.Enums;

namespace LinkUp.Core.Application.Dtos.AccionBattleship
{
    public class PlaceShipDto
    {
        public required int GameId { get; set; }
        public required string OwnerUserId { get; set; }

        public required int Length { get; set; }

        public required int StartRow { get; set; }
        public required int StartCol { get; set; }

        public required Direction Direction { get; set; }
    }
}
