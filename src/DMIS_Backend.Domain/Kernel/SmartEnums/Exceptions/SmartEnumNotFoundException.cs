namespace DMIS_Backend.Domain.Kernel.SmartEnums.Exceptions;

/// <summary>
/// 當找不到指定的 SmartEnum 時拋出的例外
/// </summary>
public class SmartEnumNotFoundException : Exception
{
  public SmartEnumNotFoundException(string message)
      : base(message)
  {
  }

  public SmartEnumNotFoundException(string message, Exception innerException)
      : base(message, innerException)
  {
  }
}
