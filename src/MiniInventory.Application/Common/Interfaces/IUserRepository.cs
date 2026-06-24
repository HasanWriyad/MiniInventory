using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ExistsAsync(string username);
    Task AddAsync(User user);
}
