using FluentValidation;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Application.Items.Commands;

namespace MiniInventory.Application.Items.Validators;

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator(IItemRepository items)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);

        RuleFor(x => x.SKU)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (command, sku, ct) => !await items.SkuExistsAsync(sku, command.Id))
            .WithMessage("SKU already exists.");

        RuleFor(x => x.Unit).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
    }
}
