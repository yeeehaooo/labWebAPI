namespace DMIS_Backend.Application.Kernel.Workflows;

/// <summary>
/// 工作流程代碼解析器
/// 組合完整的錯誤碼，格式：Module-Operation-Function-ErrorCode
/// </summary>
public class WorkflowCodeResolver
{
  private readonly IWorkflowContextAccessor _accessor;

  /// <summary>
  /// 初始化工作流程代碼解析器
  /// </summary>
  /// <param name="accessor">工作流程上下文存取器</param>
  public WorkflowCodeResolver(IWorkflowContextAccessor accessor)
  {
    _accessor = accessor;
  }

  /// <summary>
  /// 解析錯誤碼，組合完整的工作流程錯誤碼
  /// </summary>
  /// <param name="errorCode">基礎錯誤碼</param>
  /// <returns>完整的錯誤碼字串（格式：Module-Operation-Function-ErrorCode）</returns>
  public string Resolve(string errorCode)
  {
    if (string.IsNullOrEmpty(errorCode))
    {
      return errorCode;
    }

    var workflow = _accessor.Current;
    return workflow?.Build(errorCode) ?? errorCode;
  }
}
