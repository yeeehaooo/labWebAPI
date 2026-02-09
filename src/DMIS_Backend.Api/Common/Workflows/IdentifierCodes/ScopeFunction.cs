namespace DMIS_Backend.Api.Common.Workflows.IdentifierCodes;

public sealed class ScopeFunction : NamespaceCode<ScopeFunction>
{
  private ScopeFunction(int value, string name, string desc = null)
      : base(value, name, desc) { }

  public static readonly ScopeFunction UNKNOW = new(901, nameof(UNKNOW), "未知");

  // Business Flows
  public static readonly ScopeFunction DES020 = new(20, nameof(DES020), "建立訂單");
  public static readonly ScopeFunction DES110 = new(110, nameof(DES110), "查詢訂單");

  // System Flows (900+)
  public static readonly ScopeFunction SYSHEALTH = new(901, nameof(SYSHEALTH), "健康檢查");
  public static readonly ScopeFunction SYSJOB010 = new(910, nameof(SYSJOB010), "排程任務");
}



