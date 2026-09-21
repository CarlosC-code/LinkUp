

namespace LinkUp.Core.Application.Dtos.Battleship
{
    public class BattleshipGameDto
    {

        public int Id { get; set; }
        public required string CreatorUserId { get; set; }
        public required string OpponentUserId { get; set; }
        public DateTime StartedAtUtc { get; set; }
        public DateTime? FinishedAtUtc { get; set; }
        public int Status { get; set; } 
        public string? CurrentTurnUserId { get; set; }
        public DateTime? LastMoveAtUtc { get; set; }
        public string? WinnerUserId { get; set; }
        public bool CreatorReady { get; set; }
        public bool OpponentReady { get; set; }

       
        public string? OpponentUserName { get; set; }
        public string? OpponentProfileImage { get; set; }
        public double DurationHours =>
            ((FinishedAtUtc ?? DateTime.UtcNow) - StartedAtUtc).TotalHours;
        public bool IsMyTurn { get; set; }
        public bool IAmCreator { get; set; }

    }
}
