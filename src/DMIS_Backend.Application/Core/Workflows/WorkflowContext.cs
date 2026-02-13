namespace DMIS_Backend.Application.Core.Workflows;

/// <summary>
/// 工作流程上下文
/// 包含模組、操作和功能代碼，用於組合完整的錯誤碼
/// </summary>
/// <param name="WorkflowCode">WorkflowCode</param>
public sealed class WorkflowContext
{
  public WorkflowCode Code { get; }

  public DateTime CreatedAt { get; }

  public WorkflowContext(WorkflowCode code)
  {
    Code = code;
    CreatedAt = DateTime.Now;
  }

  ///// <summary>
  ///// 組合完整的錯誤碼
  ///// 格式：WorkflowCode-ErrorCode
  ///// </summary>
  ///// <param name="errorCode">錯誤代碼</param>
  ///// <returns>完整的錯誤碼字串</returns>
  //public string Build(string errorCode)
  //  => $"{Code}-{errorCode}";
}
