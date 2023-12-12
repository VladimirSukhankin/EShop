using EShopFanerum.Domain.Entites;
using EShopFanerum.Domain.Entites.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShopFanerum.Persistance.Mappings.Auth;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "public");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("ID").IsRequired().ValueGeneratedOnAdd();
        builder.Property(x => x.Username).HasColumnName("Username").IsRequired();
        builder.Property(x => x.FirstName).HasColumnName("FirstName").IsRequired();
        builder.Property(x => x.LastName).HasColumnName("LastName").IsRequired();
        builder.Property(x => x.PasswordHash).HasColumnName("PasswordHash").IsRequired();
        builder.Property(x => x.PasswordSalt).HasColumnName("PasswordSalt").IsRequired();
        
        builder.HasMany(x => x.RefreshTokens)
            .WithOne(c => c.User)
            .HasForeignKey(x => x.UserId)
            .IsRequired(false);
    }
}