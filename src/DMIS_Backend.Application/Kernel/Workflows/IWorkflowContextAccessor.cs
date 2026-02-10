namespace DMIS_Backend.Application.Kernel.Workflows;

/// <summary>
/// Workflow Context 存取器介面
/// 定義如何取得當前請求的工作流程上下文
/// </summary>
public interface IWorkflowContextAccessor
{
  /// <summary>
  /// 取得當前的工作流程上下文
  /// </summary>
  WorkflowContext? Current { get; }
}
