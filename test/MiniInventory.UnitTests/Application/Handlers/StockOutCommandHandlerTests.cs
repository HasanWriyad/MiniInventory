using FluentAssertions;
using Moq;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Application.Stock.Commands;
using MiniInventory.Domain.Entities;

namespace MiniInventory.UnitTests.Application.Handlers;

public class StockOutCommandHandlerTests
{
    private readonly Mock<IBatchRepository> _batches = new();
    private readonly Mock<IStockTransactionRepository> _transactions = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly StockOutCommandHandler _sut;

    public StockOutCommandHandlerTests()
    {
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _transactions.Setup(t => t.AddRangeAsync(It.IsAny<IEnumerable<StockTransaction>>()))
                     .Returns(Task.CompletedTask);
        _sut = new StockOutCommandHandler(_batches.Object, _transactions.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_SufficientStock_CallsIssueAndSaves()
    {
        var batch = new Batch(1, "B001", 100m, DateTime.UtcNow.AddDays(30));
        _batches.Setup(r => r.GetByItemIdAsync(1)).ReturnsAsync([batch]);
        _batches.Setup(r => r.Update(It.IsAny<Batch>()));

        var cmd = new StockOutCommand(ItemId: 1, Quantity: 40m, PerformedByUserId: 1);
        await _sut.Handle(cmd, CancellationToken.None);

        batch.Quantity.Should().Be(60m);
        _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InsufficientStock_ThrowsInvalidOperationException()
    {
        var batch = new Batch(1, "B001", 5m, DateTime.UtcNow.AddDays(30));
        _batches.Setup(r => r.GetByItemIdAsync(1)).ReturnsAsync([batch]);

        var cmd = new StockOutCommand(ItemId: 1, Quantity: 50m, PerformedByUserId: 1);
        var act = async () => await _sut.Handle(cmd, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("*Insufficient stock*");

        _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
