using FluentValidation;
using MiniInventory.Application.Stock.Commands;

namespace MiniInventory.Application.Stock.Validators;

public class StockOutCommandValidator : AbstractValidator<StockOutCommand>
{
    public StockOutCommandValidator()
    {
        RuleFor(x => x.ItemId).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.PerformedByUserId).GreaterThan(0);
    }
}
