using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DMIS_Backend.Infrastructure.Persistence.DataModels;

namespace DMIS_Backend.Infrastructure.Persistence.Configurations;

/// <summary>
/// Product 實體的 EF Core 配置
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<ProductData>
{
  public void Configure(EntityTypeBuilder<ProductData> builder)
  {
    builder.ToTable("Products");

    builder.HasKey(p => p.Id);

    builder.Property(p => p.Id)
        .ValueGeneratedOnAdd();

    builder.Property(p => p.ProductId)
        .IsRequired();

    builder.HasIndex(p => p.ProductId)
        .IsUnique();

    builder.Property(p => p.Name)
        .IsRequired()
        .HasMaxLength(200);

    builder.Property(p => p.Description)
        .HasMaxLength(1000);

    builder.Property(p => p.BasePrice)
        .HasColumnType("decimal(18,2)")
        .IsRequired();

    builder.Property(p => p.CreatedAt)
        .IsRequired();

    builder.Property(p => p.UpdatedAt);
  }
}
