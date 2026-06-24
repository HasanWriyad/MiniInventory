using MediatR;
using MiniInventory.Application.Auth.DTOs;
using MiniInventory.Application.Common.Interfaces;

namespace MiniInventory.Application.Auth.Commands;

public record LoginCommand(string Username, string Password)
    : IRequest<LoginResultDto>;

public class LoginCommandHandler(
    IUserRepository users,
    IPasswordHasher hasher,
    IJwtTokenService jwt) : IRequestHandler<LoginCommand, LoginResultDto>
{
    public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await users.GetByUsernameAsync(request.Username)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");

        if (!hasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");

        return new LoginResultDto(jwt.GenerateToken(user), user.Username, user.Role.ToString());
    }
}
