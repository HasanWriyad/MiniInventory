namespace MiniInventory.Application.Reports.DTOs;

public record StockSummaryDto(
    int ItemId,
    string ItemName,
    string SKU,
    string Unit,
    decimal TotalQuantity,
    decimal NearExpiryQuantity,
    DateTime? EarliestExpiry);
