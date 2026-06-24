using MediatR;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Application.Items.DTOs;

namespace MiniInventory.Application.Items.Queries;

public record GetItemByIdQuery(int Id) : IRequest<ItemDto>;

public class GetItemByIdQueryHandler(
    IItemRepository items) : IRequestHandler<GetItemByIdQuery, ItemDto>
{
    public async Task<ItemDto> Handle(GetItemByIdQuery request, CancellationToken ct)
    {
        var item = await items.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Item {request.Id} not found.");

        return new ItemDto(item.Id, item.Name, item.SKU, item.Unit, item.Category, item.CreatedAt);
    }
}
