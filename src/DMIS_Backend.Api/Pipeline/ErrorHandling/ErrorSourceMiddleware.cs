using DMIS_Backend.Application.Kernel.ErrorCodes;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace DMIS_Backend.Api.Pipeline.ErrorHandling;

/// <summary>
/// 錯誤來源追蹤中介軟體
/// 在請求處理前設定錯誤來源上下文，用於追蹤錯誤發生的位置（Controller.Action 或 Minimal API）
/// </summary>
public sealed class ErrorSourceMiddleware
{
  private readonly RequestDelegate _next;

  /// <summary>
  /// 初始化錯誤來源追蹤中介軟體
  /// </summary>
  /// <param name="next">下一個中介軟體</param>
  public ErrorSourceMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  /// <summary>
  /// 執行中介軟體，設定錯誤來源上下文
  /// </summary>
  /// <param name="context">HTTP 上下文</param>
  public async Task InvokeAsync(HttpContext context)
  {
    var endpoint = context.GetEndpoint();

    string source;

    if (endpoint is not null)
    {
      // MVC Controller / Action
      var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

      if (actionDescriptor is not null)
      {
        source = $"{actionDescriptor.ControllerName}.{actionDescriptor.ActionName}";
      }
      else
      {
        // Minimal API（如果有 WithName）
        source = endpoint.DisplayName ?? $"{context.Request.Method} {context.Request.Path}";
      }
    }
    else
    {
      // 極端 fallback（理論上很少）
      source = $"{context.Request.Method} {context.Request.Path}";
    }

    ErrorSourceContext.Set(source);

    try
    {
      await _next(context);
    }
    finally
    {
      ErrorSourceContext.Clear();
    }
  }
}
