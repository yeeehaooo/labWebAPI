using DMIS_Backend.Application.Kernel.Workflows;
using Microsoft.Extensions.DependencyInjection;

namespace DMIS_Backend.Api.Common.RequestContext.Workflows;

/// <summary>
/// Workflow Context 擴充方法
/// 註冊 Workflow 相關的服務（Accessor 和 Resolver）
/// </summary>
public static class WorkflowExtensions
{
  /// <summary>
  /// 註冊 Workflow Context 服務
  /// </summary>
  public static IServiceCollection AddWorkflowContext(this IServiceCollection services)
  {
    // Accessor (Web → Application Bridge)
    services.AddScoped<IWorkflowContextAccessor, HttpWorkflowContextAccessor>();

    // Resolver (Application Service)
    services.AddScoped<WorkflowCodeResolver>();

    return services;
  }
}
