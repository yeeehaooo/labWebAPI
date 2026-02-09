using System.Diagnostics;
using DMIS_Backend.Api.Common;
using DMIS_Backend.Api.Common.Workflows;
using DMIS_Backend.Application.Kernel.Abstractions;
using DMIS_Backend.Domain.Kernel.ErrorCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DMIS_Backend.Api.Filters;

/// <summary>
/// Result 包裝 Filter
/// 將 Action 返回的 Result 轉換為 APIResponse
/// 支援強型別的 IHandlerResult<T> 處理
/// 組合完整錯誤碼並設定到 WorkflowError
/// </summary>
public sealed class ResultWrappingFilter : IAsyncResultFilter
{
  private readonly IHostEnvironment _env;

  public ResultWrappingFilter(IHostEnvironment env)
  {
    _env = env;
  }

  public async Task OnResultExecutionAsync(
    ResultExecutingContext context,
    ResultExecutionDelegate next)
  {
    if (context.Result is not ObjectResult obj ||
        obj.Value is not IHandlerResult result)
    {
      await next();
      return;
    }

    // 1️ WorkflowCode（一定存在，至少是 SYS-0-WEB）
    var workflowCode = WorkflowCodeResolver.Resolve(
      context.HttpContext,
      result.Code
    );

    // 2️ TraceId
    var traceId =
      Activity.Current?.Id ??
      context.HttpContext.TraceIdentifier;

    // 3️ ExceptionDetails（依環境裁切）
    var exceptionDetails = BuildExceptionDetails(result);

    // 4️ 包成 APIResponse
    var apiResponse = new APIResponse<object>(
      Code: workflowCode,
      Message: result.Message ?? "系統錯誤",
      Data: result.IsSuccess ? result.Data : null,
      ExceptionDetails: exceptionDetails,
      TraceId: traceId
    );

    // 5️ 回寫 WorkflowCode header（選用）
    if (!string.IsNullOrEmpty(workflowCode))
    {
      context.HttpContext.Response.Headers["X-Workflow-Code"] = workflowCode;
    }

    context.Result = new ObjectResult(apiResponse)
    {
      StatusCode = ResolveStatusCode(result)
    };

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
      _ => StatusCodes.Status400BadRequest
    };
  }
}
