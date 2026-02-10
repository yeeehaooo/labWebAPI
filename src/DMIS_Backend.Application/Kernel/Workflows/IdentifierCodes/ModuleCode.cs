namespace DMIS_Backend.Application.Kernel.Workflows.IdentifierCodes;

/// <summary>
/// 模組代碼定義
/// 用於識別不同的業務模組，例如：系統模組、銷售管理模組等
/// </summary>
public sealed class ModuleCode : NamespaceCode<ModuleCode>
{
  private ModuleCode(int value, string name, string desc = null)
      : base(value, name, desc) { }

  /// <summary>
  /// 系統模組（預設模組）
  /// </summary>
  public static readonly ModuleCode System =
      new(0, "SYS", "系統");

  /// <summary>
  /// 驗證 / 身分認證模組
  /// </summary>
  public static readonly ModuleCode Auth =
      new(0, "AUTH", "驗證");

  /// <summary>
  /// 銷售管理模組
  /// </summary>
  public static readonly ModuleCode Sales =
      new(1, "DES", "銷售管理");
}



