using FluentAssertions;
using FluentValidation.TestHelper;
using MiniInventory.Application.Stock.Commands;
using MiniInventory.Application.Stock.Validators;

namespace MiniInventory.UnitTests.Application.Validators;

public class StockInCommandValidatorTests
{
    private readonly StockInCommandValidator _sut = new();

    private static StockInCommand ValidCommand() =>
        new(1, "B001", 10m, DateTime.UtcNow.AddDays(30), 1);

    [Fact]
    public void ValidCommand_ShouldPassValidation()
    {
        var result = _sut.TestValidate(ValidCommand());
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void PastExpiryDate_ShouldFailWithMessage()
    {
        var cmd = ValidCommand() with { ExpiryDate = DateTime.UtcNow.AddDays(-1) };
        var result = _sut.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.ExpiryDate)
              .WithErrorMessage("Expiry date must be in the future.");
    }

    [Fact]
    public void ZeroQuantity_ShouldFail()
    {
        var cmd = ValidCommand() with { Quantity = 0m };
        var result = _sut.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public void NegativeQuantity_ShouldFail()
    {
        var cmd = ValidCommand() with { Quantity = -5m };
        var result = _sut.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public void EmptyBatchNumber_ShouldFail()
    {
        var cmd = ValidCommand() with { BatchNumber = "" };
        var result = _sut.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.BatchNumber);
    }

    [Fact]
    public void ZeroItemId_ShouldFail()
    {
        var cmd = ValidCommand() with { ItemId = 0 };
        var result = _sut.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.ItemId);
    }
}
