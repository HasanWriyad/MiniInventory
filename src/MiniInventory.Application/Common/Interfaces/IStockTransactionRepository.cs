using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Common.Interfaces;

public interface IStockTransactionRepository
{
    Task AddAsync(StockTransaction transaction);
    Task AddRangeAsync(IEnumerable<StockTransaction> transactions);
}
