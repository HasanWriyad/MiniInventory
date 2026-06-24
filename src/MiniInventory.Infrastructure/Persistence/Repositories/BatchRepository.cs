using Microsoft.EntityFrameworkCore;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Domain.Entities;
using MiniInventory.Infrastructure.Persistence;

namespace MiniInventory.Infrastructure.Persistence.Repositories;

public class BatchRepository(AppDbContext ctx) : IBatchRepository
{
    public async Task<Batch?> GetByIdAsync(int id) =>
        await ctx.Batches.FindAsync(id);

    public async Task<Batch?> GetByItemIdAndBatchNumberAsync(int itemId, string batchNumber) =>
        await ctx.Batches.SingleOrDefaultAsync(b =>
            b.ItemId == itemId && b.BatchNumber == batchNumber);

    public async Task<List<Batch>> GetByItemIdAsync(int itemId) =>
        await ctx.Batches.Where(b => b.ItemId == itemId).ToListAsync();

    public async Task AddAsync(Batch batch) =>
        await ctx.Batches.AddAsync(batch);

    public void Update(Batch batch) =>
        ctx.Batches.Update(batch);
}
