using DMIS_Backend.Api.Common.Https;
using DMIS_Backend.Application.Kernel.Workflows;

namespace DMIS_Backend.Api.Common.RequestContext.Workflows;

/// <summary>
/// HTTP Workflow Context Accessor 實作
/// 從 HttpContext 取得 WorkflowContext，作為 Web 層與 Application 層之間的橋接
/// </summary>
public sealed class HttpWorkflowContextAccessor : IWorkflowContextAccessor
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  /// <summary>
  /// 初始化 HTTP Workflow Context Accessor
  /// </summary>
  /// <param name="httpContextAccessor">HTTP 上下文存取器</param>
  public HttpWorkflowContextAccessor(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  /// <summary>
  /// 取得當前的 WorkflowContext
  /// 從 HttpContext.Items 中取得由 WorkflowContextMiddleware 或 WorkflowContextActionFilter 設定的上下文
  /// </summary>
  public WorkflowContext? Current =>
    _httpContextAccessor.HttpContext?.Items[HttpContextItemKeys.Workflow] as WorkflowContext;
}
