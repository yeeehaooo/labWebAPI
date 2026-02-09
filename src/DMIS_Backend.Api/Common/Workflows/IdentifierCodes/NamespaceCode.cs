using DMIS_Backend.Domain.Kernel.SmartEnums;

namespace DMIS_Backend.Api.Common.Workflows.IdentifierCodes;

public abstract class NamespaceCode<T> : SmartEnum<T>
    where T : SmartEnum<T>
{
  public string Description { get; }
  public string Code => Name;

  protected NamespaceCode(int value, string name, string description = null)
      : base(value, name)
  {
    Description = description;
  }
}



