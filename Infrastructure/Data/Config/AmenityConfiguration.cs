using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
    {
        public void Configure(EntityTypeBuilder<Amenity> builder)
        {
            builder.HasKey(a => a.AmenitiesID);
            builder.Property(a => a.AmenitiesID).ValueGeneratedOnAdd();

            builder.HasIndex(a => a.Name).IsUnique();

            builder.Property(a => a.Name).IsRequired().HasMaxLength(50);

            builder.ToTable("Amenities");
        }
    }


}
