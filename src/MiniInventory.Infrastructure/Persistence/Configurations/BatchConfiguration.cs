using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniInventory.Domain.Entities;

namespace MiniInventory.Infrastructure.Persistence.Configurations;

public class BatchConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        builder.ToTable("Batches");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.BatchNumber).IsRequired().HasMaxLength(100);
        builder.Property(b => b.Quantity).HasPrecision(18, 3).IsRequired();
        builder.Property(b => b.ExpiryDate).IsRequired();
        builder.Property(b => b.CreatedAt).IsRequired();
        builder.HasMany(b => b.Transactions)
               .WithOne(t => t.Batch)
               .HasForeignKey(t => t.BatchId);
    }
}
