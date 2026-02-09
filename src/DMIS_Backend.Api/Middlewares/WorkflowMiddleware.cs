using DMIS_Backend.Api.Common;
using DMIS_Backend.Api.Common.Workflows;
using DMIS_Backend.Api.Common.Workflows.IdentifierCodes;

namespace DMIS_Backend.Api.Middlewares;

public class WorkflowMiddleware
{
  private readonly RequestDelegate _next;

  public WorkflowMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    // 預設 workflow（只在不存在時設定）
    if (!context.Items.ContainsKey(HttpContextItemKeys.Workflow))
    {
      context.Items[HttpContextItemKeys.Workflow] = new WorkflowContext(
        Module: WorkflowDefaults.DefaultModule,
        Operation: WorkflowDefaults.DefaultOperation,
        Function: WorkflowDefaults.DefaultFunction
      );
    }

    await _next(context);
  }
}

public static class WorkflowDefaults
{
  public static string DefaultModule => ModuleCode.SYS.Code;
  public static string DefaultOperation => OperationType.Unknow.Name;
  public static string DefaultFunction => ScopeFunction.UNKNOW.Code;
}
