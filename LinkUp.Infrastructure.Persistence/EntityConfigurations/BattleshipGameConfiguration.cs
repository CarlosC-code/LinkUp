using LinkUp.Core.Domain.Entities.Battleship;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkUp.Infrastructure.Persistence.EntityConfigurations
{
    public class BattleshipGameConfiguration : IEntityTypeConfiguration<BattleshipGame>
    {
        public void Configure(EntityTypeBuilder<BattleshipGame> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("BattleshipGames");
            #endregion

            #region Property configurations
            builder.Property(x => x.CreatorUserId).IsRequired().HasMaxLength(450);
            builder.Property(x => x.OpponentUserId).IsRequired().HasMaxLength(450);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.StartedAtUtc).IsRequired();
            builder.Property(x => x.CurrentTurnUserId).HasMaxLength(450);
            builder.Property(x => x.WinnerUserId).HasMaxLength(450);

            
            builder.Property(x => x.CreatorReady).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.OpponentReady).IsRequired().HasDefaultValue(false);
            #endregion

            #region indexes
            builder.HasIndex(x => x.CreatorUserId);
            builder.HasIndex(x => x.OpponentUserId);
            builder.HasIndex(x => x.Status);
            #endregion
        }
    }
}
