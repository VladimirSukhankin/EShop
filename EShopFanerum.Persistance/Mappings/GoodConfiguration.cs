using EShopFanerum.Domain.Entites.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShopFanerum.Persistance.Mappings;

public class GoodConfiguration : IEntityTypeConfiguration<Good>
{
    public void Configure(EntityTypeBuilder<Good> builder)
    {
        builder.ToTable("Goods", "public");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id").IsRequired().ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Description).HasColumnName("Description");
        builder.Property(x => x.Price).HasColumnName("Price").IsRequired();
        builder.Property(x => x.CategoryId).HasColumnName("CategoryId");
        builder.Property(x => x.ImageId).HasColumnName("ImageId");
        builder.Property(x => x.ImageIconId).HasColumnName("ImageIconId");

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .HasPrincipalKey(x => x.Id);
    }
}