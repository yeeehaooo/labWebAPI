using DMIS_Backend.Application.Core.Workflows;

namespace DMIS_Backend.Api.Pipeline.Workflows;

/// <summary>
/// Workflow 註冊擴充方法
/// 註冊 Workflow 相關的服務
/// </summary>
public static class WorkflowConfigs
{
  public static IServiceCollection AddWorkflowService(
       this IServiceCollection services)
  {
    services.AddSingleton<IWorkflowContextAccessor,
    WorkflowContextAccessor>();

    return services;
  }
  public static WebApplication UseWorkflowService(
      this WebApplication app)
  {
    // Bridge DI → Static Facade
    Workflow.Configure(
    app.Services.GetRequiredService<IWorkflowContextAccessor>());

    // Middleware 設預設 Workflow
    app.UseMiddleware<WorkflowMiddleware>();

    return app;
  }
}
