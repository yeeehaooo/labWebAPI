using System.Reflection;
using DMIS_Backend.Api.Common.Https;
using DMIS_Backend.Application.Kernel.Workflows;
using DMIS_Backend.Application.Kernel.Workflows.IdentifierCodes;
using DMIS_Backend.Domain.Kernel.Attributes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DMIS_Backend.Api.Pipeline.RequestContext;

/// <summary>
/// Workflow Context Action Filter
/// 從 Controller/Action 的 Attributes 建立 WorkflowContext
/// 如果有完整的 Attributes，會覆蓋 Middleware 設定的預設值；否則保留預設值
/// </summary>
public sealed class WorkflowContextActionFilter : IActionFilter
{
  /// <summary>
  /// 在 Action 執行前建立或更新 WorkflowContext
  /// </summary>
  /// <param name="context">Action 執行上下文</param>
  public void OnActionExecuting(ActionExecutingContext context)
  {
    // 如果已經有 WorkflowContext（可能是從 Middleware 設定的預設值），檢查是否需要更新
    var existingWorkflow = context.HttpContext.Items.ContainsKey(HttpContextItemKeys.Workflow)
      ? context.HttpContext.Items[HttpContextItemKeys.Workflow] as WorkflowContext
      : null;

    var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
    if (descriptor == null)
    {
      // Minimal API 或其他情況，保留現有的 WorkflowContext（可能是預設值）
      return;
    }

    // 1️ Module（Controller）
    var module = descriptor.ControllerTypeInfo.GetCustomAttribute<ModuleCodeAttribute>()?.Name;

    // 2️ Operation（Action）
    var operation = descriptor.MethodInfo.GetCustomAttribute<OperationTypeAttribute>()?.Code;

    // 3️ ScopeFunction（Handler）
    var scopeFunction = descriptor.MethodInfo.GetCustomAttribute<ScopeFunctionAttribute>()?.Name;

    // 如果有完整的 Attributes，使用它們建立 WorkflowContext（覆蓋預設值）
    if (module != null && operation != null && scopeFunction != null)
    {
      var workflow = new WorkflowContext(
        Module: module,
        Operation: operation,
        Function: scopeFunction
      );

      // ⭐ 關鍵：放進 HttpContext.Items（覆蓋預設值）
      context.HttpContext.Items[HttpContextItemKeys.Workflow] = workflow;
    }
    // 否則保留現有的 WorkflowContext（可能是 Middleware 設定的預設值）
  }

  /// <summary>
  /// 在 Action 執行後處理（此實作中不需要任何操作）
  /// </summary>
  /// <param name="context">Action 執行上下文</param>
  public void OnActionExecuted(ActionExecutedContext context)
  {
    // 不需要做任何事
  }
}
