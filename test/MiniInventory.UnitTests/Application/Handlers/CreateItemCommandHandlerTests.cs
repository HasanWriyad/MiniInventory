using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Application.Items.Commands;
using MiniInventory.Application.Items.Validators;
using MiniInventory.Domain.Entities;

namespace MiniInventory.UnitTests.Application.Handlers;

public class CreateItemCommandHandlerTests
{
    private readonly Mock<IItemRepository> _items = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CreateItemCommandHandler _sut;

    public CreateItemCommandHandlerTests()
    {
        _items.Setup(r => r.AddAsync(It.IsAny<Item>())).Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _sut = new CreateItemCommandHandler(_items.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsItemAndReturnsId()
    {
        var cmd = new CreateItemCommand("Widget", "SKU-001", "pcs", "Electronics");

        var id = await _sut.Handle(cmd, CancellationToken.None);

        _items.Verify(r => r.AddAsync(It.Is<Item>(i =>
            i.Name == "Widget" && i.SKU == "SKU-001")), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        id.Should().Be(0); // EF-generated; 0 in unit test (no DB)
    }

    [Fact]
    public async Task Handle_DuplicateSku_ValidatorRejectsCommand()
    {
        var itemsRepo = new Mock<IItemRepository>();
        itemsRepo.Setup(r => r.SkuExistsAsync("DUP-001", null)).ReturnsAsync(true);
        var validator = new CreateItemCommandValidator(itemsRepo.Object);

        var cmd = new CreateItemCommand("Widget", "DUP-001", "pcs", "Electronics");
        var result = await validator.TestValidateAsync(cmd);

        result.ShouldHaveValidationErrorFor(x => x.SKU)
              .WithErrorMessage("SKU already exists.");
    }
}
