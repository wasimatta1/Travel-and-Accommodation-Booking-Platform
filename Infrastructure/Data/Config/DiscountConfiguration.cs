using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(d => d.DiscountID);
            builder.Property(d => d.DiscountID).ValueGeneratedOnAdd();

            builder.Property(d => d.DiscountPercentage).IsRequired();
            builder.Property(d => d.StartDate).IsRequired();
            builder.Property(d => d.EndDate).IsRequired();

            builder.HasOne(d => d.Room)
                .WithMany(r => r.Discounts)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Discounts");
        }
    }
}
