using DMIS_Backend.Domain.Kernel.Primitives;

namespace DMIS_Backend.Application.Core.Workflows;

public static class Workflow
{

  private static IWorkflowContextAccessor? _accessor;
  /// <summary>
  /// 在應用啟動時呼叫，橋接 DI
  /// </summary>
  public static void Configure(IWorkflowContextAccessor accessor)
  {
    _accessor = accessor;
  }

  private static IWorkflowContextAccessor Accessor
      => _accessor ?? throw new InvalidOperationException(
          "Workflow not configured");

  public static WorkflowContext? Current
      => Accessor.Current;

  public static void Set(WorkflowCode code)
      => Accessor.Set(new WorkflowContext(code));

  public static void Clear()
      => Accessor.Clear();

  public static string Build(ErrorCode error)
      => Current?.Code.Build(error)
         ?? WorkflowCode.System.Build(error);
}
