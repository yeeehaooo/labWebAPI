namespace DMIS_Backend.Domain.Kernel.Attributes;

/// <summary>
/// ScopeFunction Attribute
/// 用於標記 Handler 的功能碼，用於組合完整錯誤碼
/// </summary>
/// <remarks>
/// 範例：
/// <code>
/// [ScopeFunction(nameof(ScopeFunction.DES020))]
/// public class CreateProductCommandHandler : IUseCaseCommandHandler&lt;CreateProductCommand, CreateProductResult&gt;
/// {
///     // ...
/// }
/// </code>
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public sealed class ScopeFunctionAttribute : Attribute
{
  /// <summary>
  /// 功能碼（例如 "DES020"）
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// 初始化 ScopeFunctionAttribute
  /// </summary>
  /// <param name="name">功能碼</param>
  public ScopeFunctionAttribute(string name)
  {
    Name = name ?? throw new ArgumentNullException(name);
  }
}
