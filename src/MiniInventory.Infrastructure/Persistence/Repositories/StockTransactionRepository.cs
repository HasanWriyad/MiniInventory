using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Domain.Entities;
using MiniInventory.Infrastructure.Persistence;

namespace MiniInventory.Infrastructure.Persistence.Repositories;

public class StockTransactionRepository(AppDbContext ctx) : IStockTransactionRepository
{
    public async Task AddAsync(StockTransaction transaction) =>
        await ctx.StockTransactions.AddAsync(transaction);

    public async Task AddRangeAsync(IEnumerable<StockTransaction> transactions) =>
        await ctx.StockTransactions.AddRangeAsync(transactions);
}
