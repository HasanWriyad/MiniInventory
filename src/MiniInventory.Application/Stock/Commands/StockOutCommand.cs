using MediatR;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Application.Stock.Services;
using MiniInventory.Domain.Entities;
using MiniInventory.Domain.Enums;

namespace MiniInventory.Application.Stock.Commands;

public record StockOutCommand(
    int ItemId,
    decimal Quantity,
    int PerformedByUserId) : IRequest<Unit>;

public class StockOutCommandHandler(
    IBatchRepository batches,
    IStockTransactionRepository transactions,
    IUnitOfWork uow) : IRequestHandler<StockOutCommand, Unit>
{
    public async Task<Unit> Handle(StockOutCommand request, CancellationToken ct)
    {
        var itemBatches = await batches.GetByItemIdAsync(request.ItemId);

        var plan = FefoService.Plan(
            itemBatches,
            request.Quantity,
            DateOnly.FromDateTime(DateTime.UtcNow));

        if (!plan.IsSuccess)
            throw new InvalidOperationException(plan.Error);

        var newTransactions = new List<StockTransaction>();

        foreach (var entry in plan.Value!)
        {
            var batch = itemBatches.First(b => b.Id == entry.BatchId);
            batch.Issue(entry.Quantity);
            batches.Update(batch);
            newTransactions.Add(
                new StockTransaction(entry.BatchId, TransactionType.StockOut, entry.Quantity, request.PerformedByUserId));
        }

        await transactions.AddRangeAsync(newTransactions);
        await uow.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
