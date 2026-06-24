using MediatR;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Domain.Entities;
using MiniInventory.Domain.Enums;

namespace MiniInventory.Application.Auth.Commands;

public record RegisterCommand(string Username, string Password, UserRole Role)
    : IRequest<int>;

public class RegisterCommandHandler(
    IUserRepository users,
    IPasswordHasher hasher,
    IUnitOfWork uow) : IRequestHandler<RegisterCommand, int>
{
    public async Task<int> Handle(RegisterCommand request, CancellationToken ct)
    {
        var user = new User(request.Username, hasher.Hash(request.Password), request.Role);
        await users.AddAsync(user);
        await uow.SaveChangesAsync(ct);
        return user.Id;
    }
}
