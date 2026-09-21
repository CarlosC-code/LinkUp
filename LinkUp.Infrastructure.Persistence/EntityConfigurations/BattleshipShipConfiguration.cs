using LinkUp.Core.Domain.Entities.Battleship;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkUp.Infrastructure.Persistence.EntityConfigurations
{
    public class BattleshipShipConfiguration : IEntityTypeConfiguration<BattleshipShip>
    {
        public void Configure(EntityTypeBuilder<BattleshipShip> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("BattleshipShips");
            #endregion

            #region Property configurations
            builder.Property(x => x.GameId).IsRequired();
            builder.Property(x => x.OwnerUserId).IsRequired().HasMaxLength(450);
            builder.Property(x => x.Length).IsRequired();
            builder.Property(x => x.StartRow).IsRequired();
            builder.Property(x => x.StartCol).IsRequired();
            builder.Property(x => x.Direction).IsRequired();
            builder.Property(x => x.PlacedAtUtc).IsRequired();

            
            builder.Property(x => x.IsSunk).IsRequired().HasDefaultValue(false);
            #endregion

            #region indexes
            builder.HasIndex(x => new { x.GameId, x.OwnerUserId });
            #endregion
        }
    }
}