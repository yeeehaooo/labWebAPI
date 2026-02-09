namespace DMIS_Backend.Api.Common.Workflows;

public static class WorkflowCodeResolver
{
  public static string Resolve(
    HttpContext context,
    string errorCode)
  {
    if (string.IsNullOrEmpty(errorCode))
    {
      return errorCode;
    }

    if (context.Items.TryGetValue(HttpContextItemKeys.Workflow, out var value) &&
        value is WorkflowContext workflow)
    {
      return workflow.Build(errorCode);
    }

    return errorCode;
  }
}
