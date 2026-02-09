using System.Reflection;
using DMIS_Backend.Api.Common;
using DMIS_Backend.Api.Common.Workflows;
using DMIS_Backend.Domain.Kernel.Attributes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DMIS_Backend.Api.Filters;

public sealed class WorkflowContextActionFilter : IActionFilter
{
  public void OnActionExecuting(ActionExecutingContext context)
  {
    var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
    if (descriptor == null)
    {
      return;
    }

    // 1️ Module（Controller）
    var module = descriptor.ControllerTypeInfo
      .GetCustomAttribute<ModuleCodeAttribute>()
      ?.Name;

    // 2️ Operation（Action）
    var operation = descriptor.MethodInfo
      .GetCustomAttribute<OperationTypeAttribute>()
      ?.Code;

    // 3️ ScopeFunction（Handler）
    var scopeFunction = descriptor.MethodInfo
      .GetCustomAttribute<ScopeFunctionAttribute>()
      ?.Name;

    if (module == null || operation == null || scopeFunction == null)
    {
      return;
    }

    var workflow = new WorkflowContext(
      Module: module,
      Operation: operation,
      Function: scopeFunction
    );

    // ⭐ 關鍵：放進 HttpContext.Items
    context.HttpContext.Items[HttpContextItemKeys.Workflow] = workflow;
  }

  public void OnActionExecuted(ActionExecutedContext context)
  {
    // 不需要做任何事
  }
}
