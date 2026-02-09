using DMIS_Backend.Application.Kernel.Persistence;
using DMIS_Backend.Application.Modules.Products.Queries;
using DMIS_Backend.Domain.Modules.Products;
using DMIS_Backend.Infrastructure.Caching;
using DMIS_Backend.Infrastructure.Logging;
using DMIS_Backend.Infrastructure.Persistence;
using DMIS_Backend.Infrastructure.Persistence.DbContexts;
using DMIS_Backend.Infrastructure.Persistence.ReadModels;
using DMIS_Backend.Infrastructure.Persistence.Repositories;
using DMIS_Backend.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DMIS_Backend.Infrastructure.DependencyInjection;

/// <summary>
/// Infrastructure 層服務註冊
/// </summary>
public static class InfrastructureServiceRegistration
{
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

    // Repositories
    services.AddScoped<IProductRepository, ProductRepository>();

    // Read Models (CQRS Read Model)
    services.AddScoped<IProductReadModel, ProductReadModel>();

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
    services.AddScoped<JwtService>();
    services.AddScoped<EncryptionService>();

    // ============================================
    // Logging (日誌)
    // ============================================

    // Microsoft.Extensions.Logging 適配器
    services.AddScoped<ILoggerAdapter, SerilogAdapter>();

    return services;
  }
}
