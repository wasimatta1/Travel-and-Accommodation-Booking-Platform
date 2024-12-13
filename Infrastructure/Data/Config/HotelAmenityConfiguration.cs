using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class HotelAmenityConfiguration : IEntityTypeConfiguration<HotelAmenity>
    {
        public void Configure(EntityTypeBuilder<HotelAmenity> builder)
        {
            builder.HasKey(ha => new { ha.HotelID, ha.AmenityID });

            builder.HasOne(ha => ha.Hotel)
                .WithMany(h => h.HotelAmenities)
                .HasForeignKey(ha => ha.HotelID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ha => ha.Amenity)
                .WithMany(a => a.HotelAmenities)
                .HasForeignKey(ha => ha.AmenityID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("HotelAmenities");
        }
    }


}
