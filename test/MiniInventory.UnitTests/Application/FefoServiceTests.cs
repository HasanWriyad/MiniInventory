using System.Reflection;
using FluentAssertions;
using MiniInventory.Application.Stock.Services;
using MiniInventory.Domain.Entities;

namespace MiniInventory.UnitTests.Application;

public class FefoServiceTests
{
    private static readonly DateOnly Today = new(2026, 1, 1);

    private static Batch MakeBatch(int id, decimal qty, DateTime expiry, DateTime? createdAt = null)
    {
        var batch = new Batch(1, $"B{id:D3}", qty, expiry);
        SetProp(batch, "Id", id);
        if (createdAt.HasValue)
            SetProp(batch, "CreatedAt", createdAt.Value);
        return batch;
    }

    private static void SetProp(object obj, string name, object value)
    {
        var prop = obj.GetType().GetProperty(name,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;
        prop.SetValue(obj, value);
    }

    [Fact]
    public void Plan_SingleBatch_ExactMatch()
    {
        var expiry = new DateTime(2026, 6, 1);
        var batch = MakeBatch(1, 50m, expiry);

        var result = FefoService.Plan([batch], 50m, Today);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value![0].BatchId.Should().Be(1);
        result.Value![0].Quantity.Should().Be(50m);
    }

    [Fact]
    public void Plan_SpilloverAcrossMultipleBatches()
    {
        var b1 = MakeBatch(1, 30m, new DateTime(2026, 3, 1));
        var b2 = MakeBatch(2, 40m, new DateTime(2026, 6, 1));

        var result = FefoService.Plan([b1, b2], 50m, Today);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value![0].Should().Be(new FefoService.ConsumptionEntry(1, 30m));
        result.Value![1].Should().Be(new FefoService.ConsumptionEntry(2, 20m));
    }

    [Fact]
    public void Plan_InsufficientStock_ReturnsFailure()
    {
        var batch = MakeBatch(1, 10m, new DateTime(2026, 6, 1));

        var result = FefoService.Plan([batch], 20m, Today);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Insufficient stock");
    }

    [Fact]
    public void Plan_EqualExpiry_TieBreakByCreatedAt()
    {
        var expiry = new DateTime(2026, 6, 1);
        var earlier = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var later   = new DateTime(2026, 1, 1, 1, 0, 0, DateTimeKind.Utc);

        var b1 = MakeBatch(1, 50m, expiry, later);
        var b2 = MakeBatch(2, 50m, expiry, earlier);

        var result = FefoService.Plan([b1, b2], 30m, Today);

        result.IsSuccess.Should().BeTrue();
        result.Value![0].BatchId.Should().Be(2, "earlier CreatedAt should be consumed first");
    }

    [Fact]
    public void Plan_EqualExpiryAndCreatedAt_TieBreakById()
    {
        var expiry     = new DateTime(2026, 6, 1);
        var createdAt  = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var b5 = MakeBatch(5, 50m, expiry, createdAt);
        var b3 = MakeBatch(3, 50m, expiry, createdAt);

        var result = FefoService.Plan([b5, b3], 30m, Today);

        result.IsSuccess.Should().BeTrue();
        result.Value![0].BatchId.Should().Be(3, "lower Id should be consumed first");
    }

    [Fact]
    public void Plan_ExpiredBatch_Skipped()
    {
        var expired = MakeBatch(1, 100m, new DateTime(2025, 12, 31));
        var valid   = MakeBatch(2, 10m,  new DateTime(2026, 6, 1));

        var result = FefoService.Plan([expired, valid], 10m, Today);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value![0].BatchId.Should().Be(2);
    }

    [Fact]
    public void Plan_ZeroBatch_Skipped()
    {
        var zeroBatch = MakeBatch(1, 0m,  new DateTime(2026, 6, 1));
        var valid     = MakeBatch(2, 20m, new DateTime(2026, 6, 1));

        var result = FefoService.Plan([zeroBatch, valid], 10m, Today);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value![0].BatchId.Should().Be(2);
    }
}
