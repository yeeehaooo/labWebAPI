using System.Diagnostics;
using DMIS_Backend.Api.Common;
using DMIS_Backend.Api.Common.Workflows;
using DMIS_Backend.Application.Kernel.ErrorCodes;
using Microsoft.AspNetCore.Diagnostics;

namespace DMIS_Backend.Api.Middlewares;

/// <summary>
/// 內部伺服器例外處理器
/// 統一處理所有未處理的例外，轉換為統一的 APIResponse 格式
/// 支援 Guard 例外、業務例外和系統例外的分類處理
/// </summary>
public sealed class InternalServerExceptionHandler : IExceptionHandler
{
  private readonly ILogger<InternalServerExceptionHandler> _logger;

  public InternalServerExceptionHandler(ILogger<InternalServerExceptionHandler> logger)
  {
    _logger = logger;
  }

  public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken
  )
  {
    var requestSource =
      ErrorSourceContext.Get() ?? $"{httpContext.Request.Method} {httpContext.Request.Path}";

    var workflowCode = WorkflowCodeResolver.Resolve(httpContext, "99999");
    //// ===============================
    //// Guard / Business Exception
    //// ===============================
    //if (exception is DomainException dex)
    //{
    //  var fileName = Path.GetFileName(dex.Context.File);

    //  var location = dex.Context.ArgumentExpression is null
    //    ? $"{dex.Context.Member} ({fileName} at Line:{dex.Context.Line})"
    //    : $"{dex.Context.Member} ({fileName} at Line:{dex.Context.Line}) [{dex.Context.ArgumentExpression}]";

    //  _logger.LogWarning(
    //    exception,
    //    """
    //    Guard failure
    //    Code: {Code}
    //    Location: {Location}
    //    Request: {RequestSource}
    //    """,
    //    dex.ErrorCode.Code,
    //    $"{fileName}:{dex.Context.Member}:{dex.Context.Line}",
    //    requestSource
    //  );

    //  httpContext.Response.StatusCode = StatusCodes.Status200OK;

    //  // 使用 APIResponse 包裝業務錯誤回應（商業邏輯錯誤，不需要 ExceptionDetails）
    //  var apiResponse = new APIResponse<object>(
    //    Code: "9999",
    //    Message: dex.Message
    //  );

    //  await httpContext.Response.WriteAsJsonAsync(apiResponse, cancellationToken);

    //  return true;
    //}

    // ===============================
    // Unexpected / System Error
    // ===============================
    _logger.LogError(
      exception,
      """
      Unhandled exception
      Request: {RequestSource}
      """,
      requestSource
    );

    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

    // 使用 APIResponse 包裝系統錯誤回應（必須包含 ExceptionDetails）
    var exceptionDetails = new ExceptionDetails(
      Type: "internal-server-error",
      Title: "Internal Server Error",
      Detail: exception.Message
    );

    var systemApiResponse = new APIResponse<object>(
      Code: workflowCode,
      Message: "程式內部錯誤",
      ExceptionDetails: exceptionDetails
    );

    await httpContext.Response.WriteAsJsonAsync(systemApiResponse, cancellationToken);

    return true;
  }

  private static ExceptionOrigin? ExtractFirstApplicationFrame(Exception ex)
  {
    var trace = new StackTrace(ex, true);

    foreach (var frame in trace.GetFrames() ?? Array.Empty<StackFrame>())
    {
      var method = frame.GetMethod();
      var declaringType = method?.DeclaringType;

      if (declaringType is null)
      {
        continue;
      }

      // ⭐ 過濾掉系統 / framework
      if (declaringType.Namespace is null)
      {
        continue;
      }

      if (declaringType.Namespace.StartsWith("System"))
      {
        continue;
      }

      if (declaringType.Namespace.StartsWith("Microsoft"))
      {
        continue;
      }

      return new ExceptionOrigin(
        Member: $"{declaringType.Name}.{method!.Name}",
        File: frame.GetFileName(),
        Line: frame.GetFileLineNumber()
      );
    }

    return null;
  }

  private sealed record ExceptionOrigin(string Member, string? File, int Line);
}
