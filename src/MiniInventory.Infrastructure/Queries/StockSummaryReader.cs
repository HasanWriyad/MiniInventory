using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Application.Reports.DTOs;

namespace MiniInventory.Infrastructure.Queries;

public class StockSummaryReader(IConfiguration config) : IStockSummaryReader
{
    private const string Sql = """
        SELECT
            i.Id           AS ItemId,
            i.Name         AS ItemName,
            i.SKU,
            i.Unit,
            COALESCE(SUM(b.Quantity), 0)                                    AS TotalQuantity,
            COALESCE(SUM(CASE
                WHEN b.ExpiryDate <= DATE_ADD(NOW(), INTERVAL 30 DAY)
                THEN b.Quantity ELSE 0 END), 0)                             AS NearExpiryQuantity,
            MIN(b.ExpiryDate)                                               AS EarliestExpiry
        FROM Items i
        LEFT JOIN Batches b ON b.ItemId = i.Id AND b.Quantity > 0
        GROUP BY i.Id, i.Name, i.SKU, i.Unit
        ORDER BY i.Name;
        """;

    public async Task<IEnumerable<StockSummaryDto>> GetAsync()
    {
        await using var conn = new MySqlConnection(config.GetConnectionString("Default"));
        return await conn.QueryAsync<StockSummaryDto>(Sql);
    }
}
