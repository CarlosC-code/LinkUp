using LinkUp.Core.Domain.Entities.Battleship;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkUp.Infrastructure.Persistence.EntityConfigurations
{
    public class BattleshipAttackConfiguration : IEntityTypeConfiguration<BattleshipAttack>
    {
        public void Configure(EntityTypeBuilder<BattleshipAttack> builder)
        {
            // Fluent API
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("BattleshipAttacks");
            #endregion

            #region Property configurations
            builder.Property(x => x.GameId).IsRequired();
            builder.Property(x => x.AttackerUserId).IsRequired().HasMaxLength(450);
            builder.Property(x => x.TargetRow).IsRequired();
            builder.Property(x => x.TargetCol).IsRequired();
            builder.Property(x => x.IsHit).IsRequired();
            builder.Property(x => x.PerformedAtUtc).IsRequired();
            #endregion

            #region relationships
           
            #endregion

            #region indexes
            builder.HasIndex(x => new { x.GameId, x.AttackerUserId, x.TargetRow, x.TargetCol })
                   .IsUnique();
            builder.HasIndex(x => new { x.GameId, x.AttackerUserId });
            #endregion
        }
    }
}
