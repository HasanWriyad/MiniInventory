using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Common.Interfaces;

public interface IBatchRepository
{
    Task<Batch?> GetByIdAsync(int id);
    Task<Batch?> GetByItemIdAndBatchNumberAsync(int itemId, string batchNumber);
    Task<List<Batch>> GetByItemIdAsync(int itemId);
    Task AddAsync(Batch batch);
    void Update(Batch batch);
}
