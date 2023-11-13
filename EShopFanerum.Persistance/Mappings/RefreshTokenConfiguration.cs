using EShopFanerum.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShopFanerum.Persistance.Mappings;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshToken", "public");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("ID").IsRequired().ValueGeneratedOnAdd();
        builder.Property(x => x.Token).HasColumnName("Token").IsRequired();
        builder.Property(x => x.Expires).HasColumnName("Expires").IsRequired();
        builder.Property(x => x.Created).HasColumnName("Created").IsRequired();
        builder.Property(x => x.CreatedByIp).HasColumnName("CreatedByIp").IsRequired();
        builder.Property(x => x.Revoked).HasColumnName("Revoked");
        builder.Property(x => x.ReplacedByToken).HasColumnName("ReplacedByToken");
        builder.Property(x => x.RevokedByIp).HasColumnName("RevokedByIp");
        
        builder.HasOne(c => c.User)
            .WithMany(p => p.RefreshTokens)
            .HasForeignKey(e => e.UserId)
            .HasPrincipalKey(e => e!.Id);
    }
}