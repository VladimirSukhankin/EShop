using EShopFanerum.Domain.Entites;
using EShopFanerum.Domain.Entites.Auth;
using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.Persistance.Contexts;

public class AuthContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
    }
}