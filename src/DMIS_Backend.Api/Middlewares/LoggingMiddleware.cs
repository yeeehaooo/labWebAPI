using DMIS_Backend.Application.Kernel.ErrorCodes;

namespace DMIS_Backend.Api.Middlewares;

/// <summary>
/// 日誌記錄中介軟體
/// 捕獲並記錄請求處理過程中未處理的例外，並包含錯誤來源資訊
/// </summary>
public sealed class LoggingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<LoggingMiddleware> _logger;

  public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

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
          "Unhandled exception at {Source}",
          ErrorSourceContext.Get() ?? "unknown");

      throw;
    }
  }
}
