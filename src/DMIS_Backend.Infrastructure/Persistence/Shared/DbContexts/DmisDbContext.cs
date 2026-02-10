using Microsoft.EntityFrameworkCore;

namespace DMIS_Backend.Infrastructure.Persistence.Shared.DbContexts;

/// <summary>
/// DMIS 資料庫上下文
/// Entity Framework Core 的 DbContext，用於資料庫操作和遷移
/// </summary>
public class DmisDbContext : DbContext
{
  /// <summary>
  /// 初始化資料庫上下文
  /// </summary>
  /// <param name="options">資料庫上下文選項</param>
  public DmisDbContext(DbContextOptions<DmisDbContext> options)
      : base(options)
  {
  }

  ///// <summary>
  ///// Products 資料表
  ///// </summary>
  //public DbSet<ProductData> Products { get; set; } = null!;

  /// <summary>
  /// 配置資料模型
  /// </summary>
  /// <param name="modelBuilder">模型建構器</param>
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // 套用所有 Entity Configuration
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(DmisDbContext).Assembly);
  }
}
