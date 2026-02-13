using DMIS_Backend.Application.Core.Workflows;

namespace DMIS_Backend.Api.Pipeline.Workflows;

/// <summary>
/// Workflow Context Middleware
/// 在請求處理的最早期階段設定預設 WorkflowContext
/// 確保即使請求在到達 Action Filter 之前就失敗（例如路由錯誤、認證失敗），也會有預設值
/// </summary>
public sealed class WorkflowMiddleware
{
  private readonly RequestDelegate _next;

  public WorkflowMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    // 預設 System
    Workflow.Set(WorkflowCode.System);

    //// 預設 workflow（只在不存在時設定）
    //// 這樣即使請求在到達 Action Filter 之前就失敗，也會有預設值
    //if (!context.Items.ContainsKey(HttpContextItemKeys.Workflow))
    //{
    //  context.Items[HttpContextItemKeys.Workflow] = Workflow.Current;
    //}
    try
    {
      await _next(context);
    }
    finally
    {
      Workflow.Clear(); // 🔥 一定要清
    }
  }

}
