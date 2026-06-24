using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Application.Items.Commands;
using MiniInventory.Application.Items.Validators;

namespace MiniInventory.UnitTests.Application.Validators;

public class CreateItemCommandValidatorTests
{
    private readonly Mock<IItemRepository> _items = new();
    private readonly CreateItemCommandValidator _sut;

    public CreateItemCommandValidatorTests()
    {
        _items.Setup(r => r.SkuExistsAsync(It.IsAny<string>(), null))
              .ReturnsAsync(false);
        _sut = new CreateItemCommandValidator(_items.Object);
    }

    [Fact]
    public async Task ValidCommand_ShouldPassValidation()
    {
        var cmd = new CreateItemCommand("Widget", "SKU-001", "pcs", "Electronics");
        var result = await _sut.TestValidateAsync(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task EmptyName_ShouldFail()
    {
        var cmd = new CreateItemCommand("", "SKU-001", "pcs", "Electronics");
        var result = await _sut.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task DuplicateSku_ShouldFailWithMessage()
    {
        _items.Setup(r => r.SkuExistsAsync("DUP-001", null)).ReturnsAsync(true);
        var cmd = new CreateItemCommand("Widget", "DUP-001", "pcs", "Electronics");

        var result = await _sut.TestValidateAsync(cmd);

        result.ShouldHaveValidationErrorFor(x => x.SKU)
              .WithErrorMessage("SKU already exists.");
    }

    [Fact]
    public async Task EmptyUnit_ShouldFail()
    {
        var cmd = new CreateItemCommand("Widget", "SKU-001", "", "Electronics");
        var result = await _sut.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Unit);
    }

    [Fact]
    public async Task EmptyCategory_ShouldFail()
    {
        var cmd = new CreateItemCommand("Widget", "SKU-001", "pcs", "");
        var result = await _sut.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Category);
    }
}
