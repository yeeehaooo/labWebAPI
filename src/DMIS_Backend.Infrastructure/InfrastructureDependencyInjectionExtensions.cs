using DMIS_Backend.Application.Kernel.Abstractions;
using DMIS_Backend.Application.Kernel.Persistence;
using DMIS_Backend.Application.Kernel.Security;
using DMIS_Backend.Infrastructure.Caching;
using DMIS_Backend.Infrastructure.Logging;
using DMIS_Backend.Infrastructure.Persistence.Shared;
using DMIS_Backend.Infrastructure.Persistence.Shared.DbContexts;
using DMIS_Backend.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DMIS_Backend.Infrastructure;

/// <summary>
/// Infrastructure 層服務註冊
/// 統一管理所有基礎設施服務的註冊（資料存取、快取、安全性、日誌等）
/// </summary>
public static class InfrastructureDependencyInjectionExtensions
{
  /// <summary>
  /// 註冊 Infrastructure 層服務
  /// </summary>
  /// <param name="services">服務集合</param>
  /// <param name="configuration">配置物件</param>
  /// <returns>服務集合</returns>
  public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    // ============================================
    // Persistence (資料存取)
    // ============================================

    // DbContext
    services.AddDbContext<DmisDbContext>(options =>
      options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly(typeof(DmisDbContext).Assembly.FullName)
      )
    );

    // Dapper DbSession (Infrastructure 內部技術抽象)
    // 註冊為 Scoped，確保同一個 HTTP Request 使用同一個 Session
    services.AddScoped<IDbSession, DapperDbSession>();

    // Unit of Work (Application 層定義的介面，Infrastructure 層實作)
    // 註冊為 Scoped，確保同一個 HTTP Request 使用同一個 UnitOfWork
    services.AddScoped<IUnitOfWork, DbUnitOfWork>();

    //// Repositories
    //services.AddScoped<IProductRepository, ProductRepository>();

    //// Read Models (CQRS Read Model)
    //services.AddScoped<IProductReadModel, ProductReadModel>();

    // ============================================
    // Caching (快取)
    // ============================================

    // Redis Distributed Cache
    services.AddStackExchangeRedisCache(options =>
    {
      options.Configuration = configuration.GetConnectionString("Redis");
      options.InstanceName = "DMIS_Backend:";
    });

    services.AddScoped<ICacheService, RedisCacheService>();

    // ============================================
    // Security (安全性)
    // ============================================

    services.AddScoped<KeyProvider>();
    services.AddScoped<Security.Token.IJwtService, Security.Token.JwtService>();

    // Auth Service (Application 層定義的介面，Infrastructure 層實作)
    services.AddScoped<IAuthService, AuthService>();

    services.AddScoped<EncryptionService>();

    // ============================================
    // Logging (日誌)
    // ============================================

    // Microsoft.Extensions.Logging 適配器
    services.AddScoped<ILoggingService, SerilogLoggingService>();

    return services;
  }
}
