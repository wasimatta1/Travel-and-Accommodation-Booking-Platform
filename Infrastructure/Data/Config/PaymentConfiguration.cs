using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.PaymentID);
            builder.Property(p => p.PaymentID).ValueGeneratedOnAdd();

            builder.Property(p => p.PaymentMethod).IsRequired();
            builder.Property(p => p.TotalPrice).IsRequired();
            builder.Property(p => p.PaymentDate).IsRequired();

            builder.HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Payments");
        }
    }
}
