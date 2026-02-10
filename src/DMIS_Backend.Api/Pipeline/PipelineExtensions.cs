using DMIS_Backend.Api.Pipeline.ErrorHandling;
using DMIS_Backend.Api.Pipeline.Governance;
using DMIS_Backend.Api.Pipeline.RequestContext;
using DMIS_Backend.Api.Pipeline.ResponseHandling;
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
      // RequestContext（必須最先執行）
      options.Filters.Add<WorkflowContextActionFilter>();

      // Security（認證檢查）
      options.Filters.Add<AuthenticationFilter>();

      // ResponseHandling - Stage 1：IHandlerResult → APIResponse（不處理 WorkflowCode）
      options.Filters.Add<WrappingToResponseResultFilter>();

      // ResponseHandling - Stage 2：使用 WorkflowContext + ErrorCode → WorkflowCode
      options.Filters.Add<WorkflowCodeEnrichmentResultFilter>();

      // Governance（API 啟動規範）
      options.Conventions.Add(new WorkflowConvention());
    });

    return services;
  }

  /// <summary>
  /// 使用 API Pipeline 中介軟體
  /// </summary>
  public static IApplicationBuilder UseApiPipeline(this IApplicationBuilder app)
  {
    // 最外層：全域錯誤捕捉
    app.UseExceptionHandler();

    // 錯誤來源追蹤（自訂）
    app.UseMiddleware<ErrorSourceMiddleware>();

    return app;
  }
}
