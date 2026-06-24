using MiniInventory.Application.Common.Models;
using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Stock.Services;

public static class FefoService
{
    public record ConsumptionEntry(int BatchId, decimal Quantity);

    public static Result<IReadOnlyList<ConsumptionEntry>> Plan(
        IEnumerable<Batch> batches,
        decimal requestedQty,
        DateOnly today)
    {
        var candidates = batches
            .Where(b => b.Quantity > 0 && DateOnly.FromDateTime(b.ExpiryDate) >= today)
            .OrderBy(b => b.ExpiryDate)
            .ThenBy(b => b.CreatedAt)
            .ThenBy(b => b.Id)
            .ToList();

        var totalAvailable = candidates.Sum(b => b.Quantity);
        if (totalAvailable < requestedQty)
            return Result<IReadOnlyList<ConsumptionEntry>>.Failure(
                $"Insufficient stock: requested {requestedQty}, available {totalAvailable}.");

        var plan = new List<ConsumptionEntry>();
        var remaining = requestedQty;
        foreach (var batch in candidates)
        {
            if (remaining <= 0) break;
            var take = Math.Min(batch.Quantity, remaining);
            plan.Add(new ConsumptionEntry(batch.Id, take));
            remaining -= take;
        }

        return Result<IReadOnlyList<ConsumptionEntry>>.Success(plan.AsReadOnly());
    }
}
