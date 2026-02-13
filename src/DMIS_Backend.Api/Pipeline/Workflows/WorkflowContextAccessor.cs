using DMIS_Backend.Application.Core.Workflows;

namespace DMIS_Backend.Api.Pipeline.Workflows;

/// <summary>
/// HTTP Workflow Context Accessor 實作
/// 從 HttpContext 取得 WorkflowContext，作為 Web 層與 Application 層之間的橋接
/// </summary>
public class WorkflowContextAccessor : IWorkflowContextAccessor
{
  private static readonly AsyncLocal<WorkflowContextHolder> _current
      = new AsyncLocal<WorkflowContextHolder>();

  public WorkflowContext? Current
      => _current.Value?.Context;

  public void Set(WorkflowContext context)
  {
    _current.Value?.Context = null;

    if (context != null)
    {
      _current.Value = new WorkflowContextHolder
      {
        Context = context
      };
    }
  }

  public void Clear()
  {
    if (_current.Value != null)
    {
      _current.Value.Context = null;
    }

    _current.Value = null;
  }

  private sealed class WorkflowContextHolder
  {
    public WorkflowContext? Context;
  }
}
