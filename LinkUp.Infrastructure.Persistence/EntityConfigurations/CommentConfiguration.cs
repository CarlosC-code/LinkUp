using LinkUp.Core.Domain.Entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkUp.Infrastructure.Persistence.EntityConfigurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            // Fluent API
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Comentarios");
            #endregion

            #region Property configurations
            builder.Property(x => x.PostId).IsRequired();
            builder.Property(x => x.UserId).IsRequired().HasMaxLength(450);
            builder.Property(x => x.Text).IsRequired().HasMaxLength(1000);
            #endregion

            #region relationships
            builder.HasOne(x => x.Post)
                   .WithMany(x => x.Comments)
                   .HasForeignKey(x => x.PostId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ParentComment)
                   .WithMany(x => x.Replies)
                   .HasForeignKey(x => x.ParentCommentId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region indexes
            builder.HasIndex(x => new { x.PostId, x.ParentCommentId });
            builder.HasIndex(x => x.UserId);
            #endregion
        }
    }
}
