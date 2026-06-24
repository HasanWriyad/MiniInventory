using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniInventory.Domain.Entities;

namespace MiniInventory.Infrastructure.Persistence.Configurations;

public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
{
    public void Configure(EntityTypeBuilder<StockTransaction> builder)
    {
        builder.ToTable("StockTransactions");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Type).HasConversion<string>().IsRequired();
        builder.Property(t => t.Quantity).HasPrecision(18, 3).IsRequired();
        builder.Property(t => t.Timestamp).IsRequired();
        builder.HasOne(t => t.PerformedBy)
               .WithMany()
               .HasForeignKey(t => t.PerformedByUserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
