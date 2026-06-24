using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
