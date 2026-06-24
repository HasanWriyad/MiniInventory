using MediatR;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Application.Reports.DTOs;

namespace MiniInventory.Application.Reports.Queries;

public record GetStockSummaryQuery : IRequest<IEnumerable<StockSummaryDto>>;

public class GetStockSummaryQueryHandler(IStockSummaryReader reader)
    : IRequestHandler<GetStockSummaryQuery, IEnumerable<StockSummaryDto>>
{
    public Task<IEnumerable<StockSummaryDto>> Handle(
        GetStockSummaryQuery request, CancellationToken ct)
        => reader.GetAsync();
}
