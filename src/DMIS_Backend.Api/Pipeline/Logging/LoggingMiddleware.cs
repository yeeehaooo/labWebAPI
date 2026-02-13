using DMIS_Backend.Application.Core.Workflows;

namespace DMIS_Backend.Api.Pipeline.Logging;

/// <summary>
/// 日誌記錄中介軟體
/// 捕獲並記錄請求處理過程中未處理的例外，並包含錯誤來源資訊
/// </summary>
public sealed class LoggingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<LoggingMiddleware> _logger;

  /// <summary>
  /// 初始化日誌記錄中介軟體
  /// </summary>
  /// <param name="next">下一個中介軟體</param>
  /// <param name="logger">日誌記錄器</param>
  public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  /// <summary>
  /// 執行中介軟體，捕獲並記錄未處理的例外
  /// </summary>
  /// <param name="context">HTTP 上下文</param>
  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      _logger.LogError(
        ex,
        "Unhandled exception at {WorkFlow}",
        Workflow.Current.Code ?? "unknown"
      );

      throw;
    }
  }
}
