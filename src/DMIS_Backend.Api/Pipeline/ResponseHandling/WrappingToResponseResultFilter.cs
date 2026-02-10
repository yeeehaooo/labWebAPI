using System.Diagnostics;
using DMIS_Backend.Api.Common;
using DMIS_Backend.Application.Kernel.Abstractions;
using DMIS_Backend.Domain.Kernel.ErrorCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DMIS_Backend.Api.Pipeline.ResponseHandling;

/// <summary>
/// Result 包裝 Filter - Stage 1
/// 將 Action 返回的 IHandlerResult 轉換為 APIResponse
/// 此階段不處理 WorkflowCode，只做基本的結果包裝
/// </summary>
public sealed class WrappingToResponseResultFilter : IAsyncResultFilter
{
  private readonly IHostEnvironment _env;

  /// <summary>
  /// 初始化 Result 包裝 Filter
  /// </summary>
  /// <param name="env">主機環境</param>
  public WrappingToResponseResultFilter(IHostEnvironment env)
  {
    _env = env;
  }

  /// <summary>
  /// 在結果執行時包裝為 APIResponse（Stage 1）
  /// </summary>
  /// <param name="context">結果執行上下文</param>
  /// <param name="next">下一個結果執行委派</param>
  public async Task OnResultExecutionAsync(
    ResultExecutingContext context,
    ResultExecutionDelegate next
  )
  {
    // 只處理 IHandlerResult
    if (context.Result is not ObjectResult obj || obj.Value is not IHandlerResult result)
    {
      await next();
      return;
    }

    // 1️ TraceId
    var traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;

    // 2️ ExceptionDetails（依環境裁切）
    var exceptionDetails = BuildExceptionDetails(result);

    // 3️ 包成 APIResponse（使用原始的 Code，不解析 WorkflowCode）
    var apiResponse = new APIResponse<object>(
      Code: result.Code ?? "0000",
      Message: result.Message ?? "系統錯誤",
      Data: result.IsSuccess ? result.Data : null,
      ExceptionDetails: exceptionDetails,
      TraceId: traceId
    );

    // 4️ 設定 HTTP Status Code
    var statusCode = ResolveStatusCode(result);

    // 5️ 替換 Result（保留原始的 Code，等待 Stage 2 處理 WorkflowCode）
    context.Result = new ObjectResult(apiResponse) { StatusCode = statusCode };

    await next();
  }

  // ===============================
  // ExceptionDetails（環境裁切）
  // ===============================
  private ExceptionDetails? BuildExceptionDetails(IHandlerResult result)
  {
    if (result.Error == null)
    {
      return null;
    }

    return _env.IsDevelopment()
      ? null // result.Error.ToExceptionDetails()
      : result.Error.ToExceptionDetailsWithoutTechnicalDetails();
  }

  // ===============================
  // HTTP Status
  // ===============================
  private static int ResolveStatusCode(IHandlerResult result)
  {
    if (result.IsSuccess)
    {
      return StatusCodes.Status200OK;
    }

    return result.Error?.ErrorCode.Source switch
    {
      ErrorSource.Domain => StatusCodes.Status200OK,
      ErrorSource.App => StatusCodes.Status200OK,
      ErrorSource.System => StatusCodes.Status500InternalServerError,
      ErrorSource.External => StatusCodes.Status502BadGateway,
      _ => StatusCodes.Status400BadRequest,
    };
  }
}
