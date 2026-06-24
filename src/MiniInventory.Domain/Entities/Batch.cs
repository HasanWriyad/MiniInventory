namespace MiniInventory.Domain.Entities;

public class Batch
{
    public int Id { get; private set; }
    public int ItemId { get; private set; }
    public Item Item { get; private set; } = null!;
    public string BatchNumber { get; set; } = string.Empty;
    public decimal Quantity { get; private set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime CreatedAt { get; private set; }

    public ICollection<StockTransaction> Transactions { get; private set; } = new List<StockTransaction>();

    private Batch() { }

    public Batch(int itemId, string batchNumber, decimal quantity, DateTime expiryDate)
    {
        ItemId = itemId;
        BatchNumber = batchNumber;
        Quantity = quantity;
        ExpiryDate = expiryDate;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddStock(decimal qty)
    {
        if (qty <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(qty));
        Quantity += qty;
    }

    public void Issue(decimal qty)
    {
        if (qty <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(qty));
        if (qty > Quantity)
            throw new InvalidOperationException(
                $"Insufficient stock in batch '{BatchNumber}': requested {qty}, available {Quantity}.");
        Quantity -= qty;
    }
}
