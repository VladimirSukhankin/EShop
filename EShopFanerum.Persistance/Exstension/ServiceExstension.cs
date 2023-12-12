using EShopFanerum.Domain.Repositories.Auth;
using EShopFanerum.Persistance.Contexts;
using EShopFanerum.Persistance.Repositories.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EShopFanerum.Persistance.Exstension;

public static class ServiceExtension
{
    public static IServiceCollection AddDbAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("postgresConnection");
        services.AddDbContext<AuthContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
    
    public static IServiceCollection AddDbShop(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("postgresConnection");
        services.AddDbContext<AuthContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}