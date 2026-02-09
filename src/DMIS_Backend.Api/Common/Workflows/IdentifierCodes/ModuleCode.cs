namespace DMIS_Backend.Api.Common.Workflows.IdentifierCodes;

public sealed class ModuleCode : NamespaceCode<ModuleCode>
{
  private ModuleCode(int value, string name, string desc = null)
      : base(value, name, desc) { }

  public static readonly ModuleCode SYS = new(0, nameof(SYS), "API");

  // === Module Code ===
  public static readonly ModuleCode DES = new(1, nameof(DES), "銷售管理");
}



