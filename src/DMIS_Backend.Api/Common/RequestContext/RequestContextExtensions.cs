using DMIS_Backend.Api.Common.RequestContext.Workflows;
using DMIS_Backend.Api.Pipeline.RequestContext;

namespace DMIS_Backend.Api.Common.RequestContext;

/// <summary>
/// Request Context Core 擴充方法
/// 統一管理所有請求上下文相關的服務註冊和使用
/// </summary>
public static class RequestContextExtensions
{
  /// <summary>
  /// 註冊 Request Context Core 服務
  /// 包含 Workflow、User、Tenant、CorrelationId 等請求上下文資訊
  /// </summary>
  public static IServiceCollection AddRequestContextCore(this IServiceCollection services)
  {
    // HttpContext 存取（所有 RequestContext 功能都需要）
    services.AddHttpContextAccessor();

    // Workflow Context（Accessor 和 Resolver）
    services.AddWorkflowContext();

    // 未來可擴充：
    // services.AddUserContext();
    // services.AddCorrelation();
    // services.AddTenant();

    return services;
  }

  /// <summary>
  /// 使用 Request Context Core 中介軟體
  /// 必須在 UseExceptionHandler 之前使用，確保即使請求在到達 Action Filter 之前就失敗，也會有預設值
  /// </summary>
  public static IApplicationBuilder UseRequestContextCore(this IApplicationBuilder app)
  {
    // Workflow Context Middleware（設定預設值）
    app.UseMiddleware<WorkflowContextMiddleware>();

    // 未來可擴充：
    // app.UseMiddleware<UserContextMiddleware>();
    // app.UseMiddleware<CorrelationMiddleware>();
    // app.UseMiddleware<TenantMiddleware>();

    return app;
  }
}
