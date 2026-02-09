
namespace DMIS_Backend.Domain.Kernel.Attributes;

/// <summary>
/// ModuleCode Attribute
/// 用於標記 Controller 的功能碼，用於組合完整錯誤碼
/// </summary>
/// <remarks>
/// 範例：
/// <code>
/// [ModuleCode(nameof(ModuleCode.DES))]
/// public class GetProductsController : ControllerBase;
/// {
///     // ...
/// }
/// </code>
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ModuleCodeAttribute : Attribute
{
  /// <summary>
  /// 模組碼（例如 "DES"）
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// 初始化 ModuleCodeAttribute
  /// </summary>
  /// <param name="name">功能碼</param>
  public ModuleCodeAttribute(string name)
  {
    Name = name ?? throw new ArgumentNullException(name);
  }
}
