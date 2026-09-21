using LinkUp.Core.Domain.Entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkUp.Infrastructure.Persistence.EntityConfigurations
{
    public class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Solicitudes_Amistad");
            #endregion

            #region Property configurations
            
            builder.Property(x => x.SenderUserId).IsRequired().HasMaxLength(450);
            builder.Property(x => x.ReceiverUserId).IsRequired().HasMaxLength(450);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.SentAtUtc).IsRequired();
            #endregion

            #region indexes
            builder.HasIndex(x => x.SenderUserId);
            builder.HasIndex(x => x.ReceiverUserId);
            
            builder.HasIndex(x => new { x.SenderUserId, x.ReceiverUserId, x.Status });
            #endregion
        }
    }
}
