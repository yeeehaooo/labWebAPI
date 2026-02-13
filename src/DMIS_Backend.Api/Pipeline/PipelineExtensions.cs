using DMIS_Backend.Api.Pipeline.ErrorHandling;
using DMIS_Backend.Api.Pipeline.Security;
using Microsoft.AspNetCore.Mvc;

namespace DMIS_Backend.Api.Pipeline;

/// <summary>
/// API Pipeline 擴充方法
/// 統一管理所有 Middleware 和 Filter 的註冊
/// Pipeline 負責流程處理（Exception、Result、Logging、Auth）
/// </summary>
public static class PipelineExtensions
{
  /// <summary>
  /// 註冊 API Pipeline 相關服務
  /// </summary>
  public static IServiceCollection AddApiPipeline(this IServiceCollection services)
  {
    // 註冊 Exception Handler（必須在 UseExceptionHandler 之前註冊）
    services.AddExceptionHandler<InternalServerExceptionHandler>();
    services.AddProblemDetails();

    // 註冊 MVC Filters
    // MVC Filters 組裝（順序 = 平台規範）
    services.Configure<MvcOptions>(options =>
    {
      // Security（認證檢查）
      options.Filters.Add<AuthenticationFilter>();
    });

    return services;
  }
}
