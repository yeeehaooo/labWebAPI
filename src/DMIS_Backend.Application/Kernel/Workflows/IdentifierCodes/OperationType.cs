namespace DMIS_Backend.Application.Kernel.Workflows.IdentifierCodes;

/// <summary>
/// 操作類型定義
/// 用於識別不同的操作類型，例如：查詢、寫入、下載、系統操作等
/// </summary>
public sealed class OperationType : NamespaceCode<OperationType>
{
  private OperationType(int value, string name, string desc = null)
      : base(value, name, desc) { }

  /// <summary>
  /// 操作代碼（數字格式）
  /// </summary>
  public new string Code => Value.ToString(); // ⭐ 改成數字

  // === OperationType Code ===

  /// <summary>
  /// 未知操作類型
  /// </summary>
  public static readonly OperationType Unknow = new(0, nameof(Unknow), "Unknow");
  /// <summary>
  /// 查詢操作
  /// </summary>
  public static readonly OperationType Query = new(1, nameof(Query), "查詢");
  /// <summary>
  /// 寫入操作
  /// </summary>
  public static readonly OperationType Command = new(2, nameof(Command), "寫入");
  /// <summary>
  /// 下載操作
  /// </summary>
  public static readonly OperationType Download = new(3, nameof(Download), "下載");
  /// <summary>
  /// 系統操作
  /// </summary>
  public static readonly OperationType System = new(9, nameof(System), "系統");

}



