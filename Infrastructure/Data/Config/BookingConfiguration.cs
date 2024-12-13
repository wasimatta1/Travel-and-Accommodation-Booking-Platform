using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.BookingID);
            builder.Property(b => b.BookingID).ValueGeneratedOnAdd();

            builder.Property(b => b.CheckInDate).IsRequired();
            builder.Property(b => b.CheckOutDate).IsRequired();
            builder.Property(b => b.TotalPrice).IsRequired();


            builder.HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.Cascade);


            builder.ToTable("Bookings");
        }
    }
}
