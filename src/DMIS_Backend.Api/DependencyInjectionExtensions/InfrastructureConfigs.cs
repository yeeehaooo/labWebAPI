using DMIS_Backend.Application.Core.Abstractions.Logging;
using DMIS_Backend.Application.Core.Abstractions.Persistence;
using DMIS_Backend.Application.Core.Abstractions.Security;
using DMIS_Backend.Infrastructure.Caching;
using DMIS_Backend.Infrastructure.Logging;
using DMIS_Backend.Infrastructure.Persistence.Dappers;
using DMIS_Backend.Infrastructure.Security;
using DMIS_Backend.Infrastructure.Security.Authentication;
using DMIS_Backend.Infrastructure.Security.Authentication.Jwt;

namespace DMIS_Backend.Api.DependencyInjectionExtensions;

/// <summary>
/// Infrastructure 層服務註冊
/// 統一管理所有基礎設施服務的註冊（資料存取、快取、安全性、日誌等）
/// </summary>
public static class InfrastructureConfigs
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

    // Dapper DbSession (Infrastructure 內部技術抽象)
    // 註冊為 Scoped，確保同一個 HTTP Request 使用同一個 Session
    services.AddScoped<IDbSession, DapperDbSession>();

    // Unit of Work (Application 層定義的介面，Infrastructure 層實作)
    // 註冊為 Scoped，確保同一個 HTTP Request 使用同一個 UnitOfWork
    // 使用 Dapper 實作，基於 IDbSession 管理交易
    services.AddScoped<IUnitOfWork, DapperUnitOfWork>();

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
    services.AddScoped<IJwtService, JwtService>();

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
