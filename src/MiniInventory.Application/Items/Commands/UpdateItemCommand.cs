using MediatR;
using MiniInventory.Application.Common.Interfaces;

namespace MiniInventory.Application.Items.Commands;

public record UpdateItemCommand(int Id, string Name, string SKU, string Unit, string Category)
    : IRequest<Unit>;

public class UpdateItemCommandHandler(
    IItemRepository items,
    IUnitOfWork uow) : IRequestHandler<UpdateItemCommand, Unit>
{
    public async Task<Unit> Handle(UpdateItemCommand request, CancellationToken ct)
    {
        var item = await items.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Item {request.Id} not found.");

        item.Name = request.Name;
        item.SKU = request.SKU;
        item.Unit = request.Unit;
        item.Category = request.Category;

        items.Update(item);
        await uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
