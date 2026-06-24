using MediatR;
using MiniInventory.Application.Common.Interfaces;

namespace MiniInventory.Application.Items.Commands;

public record DeleteItemCommand(int Id) : IRequest<Unit>;

public class DeleteItemCommandHandler(
    IItemRepository items,
    IBatchRepository batches,
    IUnitOfWork uow) : IRequestHandler<DeleteItemCommand, Unit>
{
    public async Task<Unit> Handle(DeleteItemCommand request, CancellationToken ct)
    {
        var item = await items.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Item {request.Id} not found.");

        var itemBatches = await batches.GetByItemIdAsync(request.Id);
        if (itemBatches.Any(b => b.Quantity > 0))
            throw new InvalidOperationException("Cannot delete item with remaining stock.");

        items.Delete(item);
        await uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
