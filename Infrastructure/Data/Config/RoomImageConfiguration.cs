using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class RoomImageConfiguration : IEntityTypeConfiguration<RoomImage>
    {
        public void Configure(EntityTypeBuilder<RoomImage> builder)
        {
            builder.HasKey(ri => ri.ImageID);
            builder.Property(ri => ri.ImageID).ValueGeneratedOnAdd();

            builder.Property(ri => ri.ImageUrl).IsRequired().HasMaxLength(255);

            builder.HasOne(ri => ri.Room)
                .WithMany(r => r.RoomImages)
                .HasForeignKey(ri => ri.RoomID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("RoomImages");
        }
    }

}
