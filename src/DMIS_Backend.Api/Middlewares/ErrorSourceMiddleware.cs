using DMIS_Backend.Application.Kernel.ErrorCodes;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace DMIS_Backend.Api.Middlewares;

/// <summary>
/// 錯誤來源追蹤中介軟體
/// 在請求處理前設定錯誤來源上下文，用於追蹤錯誤發生的位置（Controller.Action 或 Minimal API）
/// </summary>
public sealed class ErrorSourceMiddleware
{
  private readonly RequestDelegate _next;

  public ErrorSourceMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    var endpoint = context.GetEndpoint();

    string source;

    if (endpoint is not null)
    {
      // MVC Controller / Action
      var actionDescriptor =
          endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

      if (actionDescriptor is not null)
      {
        source = $"{actionDescriptor.ControllerName}.{actionDescriptor.ActionName}";
      }
      else
      {
        // Minimal API（如果有 WithName）
        source = endpoint.DisplayName
                 ?? $"{context.Request.Method} {context.Request.Path}";
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
