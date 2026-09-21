using LinkUp.Core.Domain.Entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkUp.Infrastructure.Persistence.EntityConfigurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            // Fluent API
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Publicaciones");
            #endregion

            #region Property configurations
            builder.Property(x => x.UserId).IsRequired().HasMaxLength(450);
            builder.Property(x => x.Content).IsRequired().HasMaxLength(2000);
            builder.Property(x => x.MediaType).IsRequired();
            builder.Property(x => x.ImagePath).HasMaxLength(500);
            builder.Property(x => x.YouTubeUrl).HasMaxLength(500);
            builder.Property(x => x.PublishedAtUtc).IsRequired();
            #endregion

            #region relationships
            builder.HasMany(x => x.Comments)
                   .WithOne(x => x.Post!)
                   .HasForeignKey(x => x.PostId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Reactions)
                   .WithOne(x => x.Post!)
                   .HasForeignKey(x => x.PostId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.PublishedAtUtc);
            #endregion
        }
    }
}
