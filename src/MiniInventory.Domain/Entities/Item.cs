namespace MiniInventory.Domain.Entities;

public class Item
{
    public int Id { get; private set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public ICollection<Batch> Batches { get; private set; } = new List<Batch>();

    private Item() { }

    public Item(string name, string sku, string unit, string category)
    {
        Name = name;
        SKU = sku;
        Unit = unit;
        Category = category;
        CreatedAt = DateTime.UtcNow;
    }
}
