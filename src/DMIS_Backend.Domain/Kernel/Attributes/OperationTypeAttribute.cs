namespace DMIS_Backend.Domain.Kernel.Attributes;

/// <summary>
/// FunctionCode Attribute
/// 用於標記 Endpoint 的操作碼，用於組合完整錯誤碼
/// </summary>
/// <remarks>
/// 範例：
/// <code>
/// [OperationType(nameof(OperationType.Query))]
/// public async Task&lt;IActionResult&gt; Endpoint();
/// {
///     // ...
/// }
/// </code>
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public sealed class OperationTypeAttribute : Attribute
{
  /// <summary>
  /// 功能碼（例如 "1"）
  /// </summary>
  public string Code { get; }

  /// <summary>
  /// 初始化 FunctionCodeAttribute
  /// </summary>
  /// <param name="code">操作碼</param>
  public OperationTypeAttribute(string code)
  {
    Code = code ?? throw new ArgumentNullException(code);
  }
}
