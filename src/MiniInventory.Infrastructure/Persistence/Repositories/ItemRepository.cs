using Microsoft.EntityFrameworkCore;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Domain.Entities;
using MiniInventory.Infrastructure.Persistence;

namespace MiniInventory.Infrastructure.Persistence.Repositories;

public class ItemRepository(AppDbContext ctx) : IItemRepository
{
    public async Task<Item?> GetByIdAsync(int id) =>
        await ctx.Items.FindAsync(id);

    public async Task<bool> SkuExistsAsync(string sku, int? excludeId = null) =>
        await ctx.Items.AnyAsync(i =>
            i.SKU == sku && (excludeId == null || i.Id != excludeId));

    public async Task<(IEnumerable<Item> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? category, string? search)
    {
        var query = ctx.Items.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(i => i.Category == category);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(i => i.Name.Contains(search) || i.SKU.Contains(search));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(i => i.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task AddAsync(Item item) =>
        await ctx.Items.AddAsync(item);

    public void Update(Item item) =>
        ctx.Items.Update(item);

    public void Delete(Item item) =>
        ctx.Items.Remove(item);
}
