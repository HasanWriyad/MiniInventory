namespace MiniInventory.Application.Items.DTOs;

public record ItemDto(int Id, string Name, string SKU, string Unit, string Category, DateTime CreatedAt);
