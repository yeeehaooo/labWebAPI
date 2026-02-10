namespace DMIS_Backend.Application.Kernel.Workflows;

/// <summary>
/// 工作流程上下文
/// 包含模組、操作和功能代碼，用於組合完整的錯誤碼
/// </summary>
/// <param name="Module">模組代碼</param>
/// <param name="Operation">操作類型</param>
/// <param name="Function">功能範圍</param>
public sealed record WorkflowContext(
  string Module,
  string Operation,
  string Function)
{
  /// <summary>
  /// 組合完整的錯誤碼
  /// 格式：Module-Operation-Function-ErrorCode
  /// </summary>
  /// <param name="errorCode">錯誤代碼</param>
  /// <returns>完整的錯誤碼字串</returns>
  public string Build(string errorCode)
    => $"{Module}-{Operation}-{Function}-{errorCode}";
}
