using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.RoomID);
            builder.Property(r => r.RoomID).ValueGeneratedOnAdd();

            builder.Property(r => r.RoomNumber).IsRequired().HasMaxLength(10);
            builder.Property(r => r.RoomType).IsRequired();
            builder.Property(r => r.AdultsCapacity).IsRequired();
            builder.Property(r => r.ChildrenCapacity).IsRequired();
            builder.Property(r => r.PricePerNight).IsRequired();
            builder.Property(r => r.Description).IsRequired().HasMaxLength(255);
            builder.Property(r => r.Availability).IsRequired();
            builder.Property(r => r.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(r => r.UpdatedAt).IsRequired(false);

            builder.HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.bookings)
                .WithMany(b => b.Rooms)
                .UsingEntity<BookingRoom>();

            builder.ToTable("Rooms");
        }
    }

}
