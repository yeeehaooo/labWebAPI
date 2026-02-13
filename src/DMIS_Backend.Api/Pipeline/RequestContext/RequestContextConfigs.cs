using DMIS_Backend.Api.Pipeline.Workflows;

namespace DMIS_Backend.Api.Pipeline.RequestContext;

/// <summary>
/// Request Context Core 擴充方法
/// 統一管理所有請求上下文相關的服務註冊和使用
/// </summary>
public static class RequestContextConfigs
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
    services.AddWorkflowService();

    // 未來可擴充：
    // services.AddUserContext();
    // services.AddCorrelation();
    // services.AddTenant();

    return services;
  }
}
