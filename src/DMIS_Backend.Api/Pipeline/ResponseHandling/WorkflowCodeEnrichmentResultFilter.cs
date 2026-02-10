using DMIS_Backend.Api.Common;
using DMIS_Backend.Application.Kernel.Workflows;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DMIS_Backend.Api.Pipeline.ResponseHandling;

/// <summary>
/// WorkflowCode 強化 Filter - Stage 2
/// 使用 WorkflowContext + ErrorCode 解析完整的 WorkflowCode
/// 更新 APIResponse.Code 和 Header
/// </summary>
public sealed class WorkflowCodeEnrichmentResultFilter : IAsyncResultFilter
{
  private readonly WorkflowCodeResolver _workflowCodeResolver;

  /// <summary>
  /// 初始化 WorkflowCode 強化 Filter
  /// </summary>
  /// <param name="workflowCodeResolver">工作流程代碼解析器</param>
  public WorkflowCodeEnrichmentResultFilter(WorkflowCodeResolver workflowCodeResolver)
  {
    _workflowCodeResolver = workflowCodeResolver;
  }

  /// <summary>
  /// 在結果執行時強化 WorkflowCode（Stage 2）
  /// </summary>
  /// <param name="context">結果執行上下文</param>
  /// <param name="next">下一個結果執行委派</param>
  public async Task OnResultExecutionAsync(
    ResultExecutingContext context,
    ResultExecutionDelegate next
  )
  {
    // 只處理 APIResponse<object>
    if (context.Result is not ObjectResult obj || obj.Value is not APIResponse<object> apiResponse)
    {
      await next();
      return;
    }

    // 1️ 使用 WorkflowContext + ErrorCode 解析完整的 WorkflowCode
    var workflowCode = _workflowCodeResolver.Resolve(apiResponse.Code);

    // 2️ 建立新的 APIResponse，使用完整的 WorkflowCode
    var enrichedApiResponse = new APIResponse<object>(
      Code: workflowCode,
      Message: apiResponse.Message,
      Data: apiResponse.Data,
      ExceptionDetails: apiResponse.ExceptionDetails,
      TraceId: apiResponse.TraceId
    );

    // 3️ 回寫 WorkflowCode header
    if (!string.IsNullOrEmpty(workflowCode))
    {
      context.HttpContext.Response.Headers["X-Workflow-Code"] = workflowCode;
    }

    // 4️ 替換 Result（使用強化後的 WorkflowCode）
    context.Result = new ObjectResult(enrichedApiResponse) { StatusCode = obj.StatusCode };

    await next();
  }
}
