using MediatR;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Items.Commands;

public record CreateItemCommand(string Name, string SKU, string Unit, string Category)
    : IRequest<int>;

public class CreateItemCommandHandler(
    IItemRepository items,
    IUnitOfWork uow) : IRequestHandler<CreateItemCommand, int>
{
    public async Task<int> Handle(CreateItemCommand request, CancellationToken ct)
    {
        var item = new Item(request.Name, request.SKU, request.Unit, request.Category);
        await items.AddAsync(item);
        await uow.SaveChangesAsync(ct);
        return item.Id;
    }
}
