using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.AuthService.Data;

public class UsersDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {   
    }
}