using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.ReviewId);
            builder.Property(r => r.ReviewId).ValueGeneratedOnAdd();

            builder.Property(r => r.Content).IsRequired(false).HasMaxLength(526);
            builder.Property(r => r.Rating).IsRequired();
            builder.Property(r => r.ReviewDate).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(r => r.Hotel)
                .WithMany(r => r.Reviews)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.User)
                .WithMany(r => r.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Reviews");
        }
    }
}
