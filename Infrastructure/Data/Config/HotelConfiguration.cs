using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasKey(h => h.HotelID);
            builder.Property(h => h.HotelID).ValueGeneratedOnAdd();

            builder.Property(h => h.Name).IsRequired().HasMaxLength(50);
            builder.Property(h => h.StarRating).IsRequired();
            builder.Property(h => h.Description).IsRequired().HasMaxLength(255);
            builder.Property(h => h.Address).IsRequired().HasMaxLength(255);
            builder.Property(h => h.ThumbnailURL).IsRequired().HasMaxLength(255);
            builder.Property(h => h.ImageURL).IsRequired().HasMaxLength(255);
            builder.Property(h => h.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(h => h.UpdatedAt).IsRequired(false);

            builder.HasOne(h => h.City)
                .WithMany(c => c.Hotels)
                .HasForeignKey(h => h.CityID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(h => h.Owner)
                .WithMany(u => u.Hotels)
                .HasForeignKey(h => h.OwnerID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Hotels");
        }

    }

}
