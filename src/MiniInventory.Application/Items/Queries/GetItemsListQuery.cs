using MediatR;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Application.Common.Models;
using MiniInventory.Application.Items.DTOs;

namespace MiniInventory.Application.Items.Queries;

public record GetItemsListQuery(
    int Page = 1,
    int PageSize = 20,
    string? Category = null,
    string? Search = null) : IRequest<PagedResult<ItemDto>>;

public class GetItemsListQueryHandler(
    IItemRepository items) : IRequestHandler<GetItemsListQuery, PagedResult<ItemDto>>
{
    public async Task<PagedResult<ItemDto>> Handle(GetItemsListQuery request, CancellationToken ct)
    {
        var (data, total) = await items.GetPagedAsync(
            request.Page, request.PageSize, request.Category, request.Search);

        return new PagedResult<ItemDto>
        {
            Items = data.Select(i => new ItemDto(i.Id, i.Name, i.SKU, i.Unit, i.Category, i.CreatedAt)),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
