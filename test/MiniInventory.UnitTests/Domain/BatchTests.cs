using FluentAssertions;
using MiniInventory.Domain.Entities;

namespace MiniInventory.UnitTests.Domain;

public class BatchTests
{
    private static Batch MakeBatch(decimal quantity = 100m) =>
        new(1, "B001", quantity, DateTime.UtcNow.AddDays(30));

    [Fact]
    public void AddStock_ValidQty_IncreasesQuantity()
    {
        var batch = MakeBatch(50m);
        batch.AddStock(25m);
        batch.Quantity.Should().Be(75m);
    }

    [Fact]
    public void AddStock_ZeroQty_ThrowsArgumentException()
    {
        var batch = MakeBatch();
        var act = () => batch.AddStock(0m);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddStock_NegativeQty_ThrowsArgumentException()
    {
        var batch = MakeBatch();
        var act = () => batch.AddStock(-5m);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Issue_ValidQty_DecreasesQuantity()
    {
        var batch = MakeBatch(100m);
        batch.Issue(30m);
        batch.Quantity.Should().Be(70m);
    }

    [Fact]
    public void Issue_ExactQty_ReducesToZero()
    {
        var batch = MakeBatch(50m);
        batch.Issue(50m);
        batch.Quantity.Should().Be(0m);
    }

    [Fact]
    public void Issue_ExceedsQty_ThrowsInvalidOperationException()
    {
        var batch = MakeBatch(10m);
        var act = () => batch.Issue(20m);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Issue_ZeroQty_ThrowsArgumentException()
    {
        var batch = MakeBatch();
        var act = () => batch.Issue(0m);
        act.Should().Throw<ArgumentException>();
    }
}
