using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Common.Interfaces;

public interface IItemRepository
{
    Task<Item?> GetByIdAsync(int id);
    Task<bool> SkuExistsAsync(string sku, int? excludeId = null);
    Task<(IEnumerable<Item> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? category, string? search);
    Task AddAsync(Item item);
    void Update(Item item);
    void Delete(Item item);
}
