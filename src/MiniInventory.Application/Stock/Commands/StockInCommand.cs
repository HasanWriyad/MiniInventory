using MediatR;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Domain.Entities;
using MiniInventory.Domain.Enums;

namespace MiniInventory.Application.Stock.Commands;

public record StockInCommand(
    int ItemId,
    string BatchNumber,
    decimal Quantity,
    DateTime ExpiryDate,
    int PerformedByUserId) : IRequest<Unit>;

public class StockInCommandHandler(
    IItemRepository items,
    IBatchRepository batches,
    IStockTransactionRepository transactions,
    IUnitOfWork uow) : IRequestHandler<StockInCommand, Unit>
{
    public async Task<Unit> Handle(StockInCommand request, CancellationToken ct)
    {
        var item = await items.GetByIdAsync(request.ItemId)
            ?? throw new KeyNotFoundException($"Item {request.ItemId} not found.");

        var batch = await batches.GetByItemIdAndBatchNumberAsync(item.Id, request.BatchNumber);

        if (batch is not null)
        {
            batch.AddStock(request.Quantity);
            batches.Update(batch);
        }
        else
        {
            batch = new Batch(item.Id, request.BatchNumber, request.Quantity, request.ExpiryDate);
            await batches.AddAsync(batch);
        }

        await uow.SaveChangesAsync(ct);

        var transaction = new StockTransaction(
            batch.Id, TransactionType.StockIn, request.Quantity, request.PerformedByUserId);
        await transactions.AddAsync(transaction);
        await uow.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
