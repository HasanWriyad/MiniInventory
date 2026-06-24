using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniInventory.Domain.Entities;

namespace MiniInventory.Infrastructure.Persistence.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Name).IsRequired().HasMaxLength(200);
        builder.Property(i => i.SKU).IsRequired().HasMaxLength(50);
        builder.HasIndex(i => i.SKU).IsUnique();
        builder.Property(i => i.Unit).IsRequired().HasMaxLength(20);
        builder.Property(i => i.Category).IsRequired().HasMaxLength(100);
        builder.Property(i => i.CreatedAt).IsRequired();
        builder.HasMany(i => i.Batches)
               .WithOne(b => b.Item)
               .HasForeignKey(b => b.ItemId);
    }
}
