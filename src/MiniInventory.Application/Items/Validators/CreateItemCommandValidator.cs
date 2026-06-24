using FluentValidation;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Application.Items.Commands;

namespace MiniInventory.Application.Items.Validators;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator(IItemRepository items)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);

        RuleFor(x => x.SKU)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (sku, ct) => !await items.SkuExistsAsync(sku))
            .WithMessage("SKU already exists.");

        RuleFor(x => x.Unit).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
    }
}
