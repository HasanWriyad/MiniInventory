using Microsoft.EntityFrameworkCore;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Domain.Entities;
using MiniInventory.Infrastructure.Persistence;

namespace MiniInventory.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext ctx) : IUserRepository
{
    public async Task<User?> GetByIdAsync(int id) =>
        await ctx.Users.FindAsync(id);

    public async Task<User?> GetByUsernameAsync(string username) =>
        await ctx.Users.SingleOrDefaultAsync(u => u.Username == username);

    public async Task<bool> ExistsAsync(string username) =>
        await ctx.Users.AnyAsync(u => u.Username == username);

    public async Task AddAsync(User user) =>
        await ctx.Users.AddAsync(user);
}
