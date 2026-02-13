using System.Diagnostics;
using DMIS_Backend.Api.Common.Responses;
using DMIS_Backend.Application.Core.Workflows;
using DMIS_Backend.Domain.Kernel.Primitives;
using Microsoft.AspNetCore.Diagnostics;

namespace DMIS_Backend.Api.Pipeline.ErrorHandling;

/// <summary>
/// 內部伺服器例外處理器
/// 統一處理所有未處理的例外，轉換為統一的 APIResponse 格式
/// 支援 Guard 例外、業務例外和系統例外的分類處理
/// </summary>
public sealed class InternalServerExceptionHandler : IExceptionHandler
{
  private readonly ILogger<InternalServerExceptionHandler> _logger;

  /// <summary>
  /// 初始化內部伺服器例外處理器
  /// </summary>
  /// <param name="logger">日誌記錄器</param>
  public InternalServerExceptionHandler(ILogger<InternalServerExceptionHandler> logger)
  {
    _logger = logger;
  }

  /// <summary>
  /// 嘗試處理例外
  /// </summary>
  /// <param name="httpContext">HTTP 上下文</param>
  /// <param name="exception">例外物件</param>
  /// <param name="cancellationToken">取消令牌</param>
  /// <returns>如果已處理例外則返回 true，否則返回 false</returns>
  public async ValueTask<bool> TryHandleAsync(
       HttpContext httpContext,
       Exception exception,
       CancellationToken cancellationToken)
  {
    var requestSource =
        $"{httpContext.Request.Method} {httpContext.Request.Path}";

    _logger.LogError(
        exception,
        """
            Unhandled exception
            Request: {RequestSource}
            """,
        requestSource);

    httpContext.Response.StatusCode =
        StatusCodes.Status500InternalServerError;

    // 1️ 取得 Workflow
    var workflow = Workflow.Current;

    // 2️ 使用 SystemCode.InternalError
    var finalCode = workflow.Code.Build(SystemCode.InternalError);

    // 3️ 建立 APIResponse（純 DTO）
    var response = new APIResponse(
        code: finalCode,
        message: SystemCode.InternalError.Message);

    await httpContext.Response
        .WriteAsJsonAsync(response, cancellationToken);

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
