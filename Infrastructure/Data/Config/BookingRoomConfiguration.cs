using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class BookingRoomConfiguration : IEntityTypeConfiguration<BookingRoom>
    {
        public void Configure(EntityTypeBuilder<BookingRoom> builder)
        {
            builder.HasKey(br => new { br.RoomID, br.BookingID });

            //builder.HasOne(br => br.Booking)
            //    .WithMany(b => b.BookingRooms)
            //    .HasForeignKey(br => br.BookingID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(br => br.Room)
            //    .WithMany(r => r.BookingRooms)
            //    .HasForeignKey(br => br.RoomID)
            //    .OnDelete(DeleteBehavior.Cascade);
            // i want booking been deleted when room is deleted

            builder.HasOne(br => br.Booking)
                .WithMany(b => b.BookingRooms)
                .HasForeignKey(br => br.BookingID)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(br => br.Room)
                .WithMany(r => r.BookingRooms)
                .HasForeignKey(br => br.RoomID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("BookingRooms");
        }
    }
}
