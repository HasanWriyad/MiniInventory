using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniInventory.Application.Common.Interfaces;
using MiniInventory.Infrastructure.Persistence;
using MiniInventory.Infrastructure.Persistence.Repositories;
using MiniInventory.Infrastructure.Queries;
using MiniInventory.Infrastructure.Services;

namespace MiniInventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString)));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IBatchRepository, BatchRepository>();
        services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IStockSummaryReader, StockSummaryReader>();

        return services;
    }
}
