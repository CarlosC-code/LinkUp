using LinkUp.Core.Domain.Entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkUp.Infrastructure.Persistence.EntityConfigurations
{
    public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            // Fluent API
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Reacciones");
            #endregion

            #region Property configurations
            builder.Property(x => x.PostId).IsRequired();
            builder.Property(x => x.UserId).IsRequired().HasMaxLength(450);
            builder.Property(x => x.Type).IsRequired();
            #endregion

            #region relationships
            builder.HasOne(x => x.Post)
                   .WithMany(x => x.Reactions)
                   .HasForeignKey(x => x.PostId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region indexes
            builder.HasIndex(x => new { x.PostId, x.UserId }).IsUnique();
            #endregion
        }
    }
}
