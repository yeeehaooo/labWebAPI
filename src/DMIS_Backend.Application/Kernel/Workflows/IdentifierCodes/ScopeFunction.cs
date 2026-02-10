namespace DMIS_Backend.Application.Kernel.Workflows.IdentifierCodes;

/// <summary>
/// 功能範圍代碼定義
/// 用於識別不同的業務功能範圍，例如：建立訂單、查詢訂單、健康檢查等
/// </summary>
public sealed class ScopeFunction : NamespaceCode<ScopeFunction>
{
  private ScopeFunction(int value, string name, string desc = null)
      : base(value, name, desc) { }


  /// <summary>
  /// 未知功能範圍
  /// </summary>
  public static readonly ScopeFunction LOGIN = new(901, nameof(LOGIN), "登入");
  // Business Flows
  /// <summary>
  /// 建立訂單功能
  /// </summary>
  public static readonly ScopeFunction DES020 = new(20, nameof(DES020), "建立訂單");
  /// <summary>
  /// 查詢訂單功能
  /// </summary>
  public static readonly ScopeFunction DES110 = new(110, nameof(DES110), "查詢訂單");


  // System Flows (900+)
  /// <summary>
  /// 健康檢查功能
  /// </summary>
  public static readonly ScopeFunction SYSHEALTH = new(901, nameof(SYSHEALTH), "健康檢查");
  /// <summary>
  /// 排程任務功能
  /// </summary>
  public static readonly ScopeFunction SYSJOB010 = new(910, nameof(SYSJOB010), "排程任務");

  /// <summary>
  /// 未知功能範圍
  /// </summary>
  public static readonly ScopeFunction UNKNOW = new(999, nameof(UNKNOW), "未知");
}



