using MiniInventory.Application.Reports.DTOs;

namespace MiniInventory.Application.Common.Interfaces;

public interface IStockSummaryReader
{
    Task<IEnumerable<StockSummaryDto>> GetAsync();
}
