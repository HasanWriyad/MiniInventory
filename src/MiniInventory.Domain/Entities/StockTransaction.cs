using MiniInventory.Domain.Enums;

namespace MiniInventory.Domain.Entities;

public class StockTransaction
{
    public int Id { get; private set; }
    public int BatchId { get; private set; }
    public Batch Batch { get; private set; } = null!;
    public TransactionType Type { get; private set; }
    public decimal Quantity { get; private set; }
    public DateTime Timestamp { get; private set; }
    public int PerformedByUserId { get; private set; }
    public User PerformedBy { get; private set; } = null!;

    private StockTransaction() { }

    public StockTransaction(int batchId, TransactionType type, decimal quantity, int performedByUserId)
    {
        BatchId = batchId;
        Type = type;
        Quantity = quantity;
        PerformedByUserId = performedByUserId;
        Timestamp = DateTime.UtcNow;
    }
}
