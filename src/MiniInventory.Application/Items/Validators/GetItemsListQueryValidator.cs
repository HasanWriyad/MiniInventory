using FluentValidation;
using MiniInventory.Application.Items.Queries;

namespace MiniInventory.Application.Items.Validators;

public class GetItemsListQueryValidator : AbstractValidator<GetItemsListQuery>
{
    public GetItemsListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
