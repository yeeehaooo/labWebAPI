namespace DMIS_Backend.Api.Shared;

/// <summary>
/// HttpContext.Items 的鍵值常數
/// 用於在請求處理過程中儲存和存取上下文資訊
/// </summary>
public static class HttpContextItemKeys
{
  /// <summary>
  /// Workflow 上下文的鍵值
  /// 用於在 HttpContext.Items 中儲存 WorkflowContext
  /// </summary>
  public static readonly object Workflow = new();
}

