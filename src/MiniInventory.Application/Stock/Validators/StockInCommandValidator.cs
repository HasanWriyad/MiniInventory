using FluentValidation;
using MiniInventory.Application.Stock.Commands;

namespace MiniInventory.Application.Stock.Validators;

public class StockInCommandValidator : AbstractValidator<StockInCommand>
{
    public StockInCommandValidator()
    {
        RuleFor(x => x.ItemId).GreaterThan(0);
        RuleFor(x => x.BatchNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.ExpiryDate)
            .Must(date => date > DateTime.UtcNow)
            .WithMessage("Expiry date must be in the future.");
        RuleFor(x => x.PerformedByUserId).GreaterThan(0);
    }
}
