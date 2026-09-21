using LinkUp.Core.Domain.Entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkUp.Infrastructure.Persistence.EntityConfigurations
{
    public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Amistades");
            #endregion

            #region Property configurations
            
            builder.Property(x => x.UserAId).IsRequired().HasMaxLength(450);
            builder.Property(x => x.UserBId).IsRequired().HasMaxLength(450);
            builder.Property(x => x.SinceUtc).IsRequired();
            #endregion

            #region indexes
          
            builder.HasIndex(x => new { x.UserAId, x.UserBId }).IsUnique();
            builder.HasIndex(x => x.UserAId);
            builder.HasIndex(x => x.UserBId);
            #endregion
        }
    }
}