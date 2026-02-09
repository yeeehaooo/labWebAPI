namespace DMIS_Backend.Api.Common.Workflows.IdentifierCodes;

public sealed class OperationType : NamespaceCode<OperationType>
{
  private OperationType(int value, string name, string desc = null)
      : base(value, name, desc) { }

  public new string Code => Value.ToString(); // ⭐ 改成數字

  // === OperationType Code ===

  public static readonly OperationType Unknow = new(0, nameof(Unknow), "Unknow");
  public static readonly OperationType Query = new(1, nameof(Query), "查詢");
  public static readonly OperationType Command = new(2, nameof(Command), "寫入");
  public static readonly OperationType Download = new(3, nameof(Download), "下載");
  public static readonly OperationType System = new(9, nameof(System), "系統");

}



