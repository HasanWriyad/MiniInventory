using FluentValidation;
using MiniInventory.Application.Auth.Commands;
using MiniInventory.Application.Common.Interfaces;

namespace MiniInventory.Application.Auth.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator(IUserRepository users)
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .MustAsync(async (username, ct) => !await users.ExistsAsync(username))
            .WithMessage("Username is already taken.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}
