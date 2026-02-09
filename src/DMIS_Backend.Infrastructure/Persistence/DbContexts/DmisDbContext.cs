using Microsoft.EntityFrameworkCore;
using DMIS_Backend.Infrastructure.Persistence.Configurations;
using DMIS_Backend.Infrastructure.Persistence.DataModels;

namespace DMIS_Backend.Infrastructure.Persistence.DbContexts;

/// <summary>
/// DMIS 資料庫上下文
/// </summary>
public class DmisDbContext : DbContext
{
  public DmisDbContext(DbContextOptions<DmisDbContext> options)
      : base(options)
  {
  }

  public DbSet<ProductData> Products { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // 套用所有 Entity Configuration
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(DmisDbContext).Assembly);
  }
}
