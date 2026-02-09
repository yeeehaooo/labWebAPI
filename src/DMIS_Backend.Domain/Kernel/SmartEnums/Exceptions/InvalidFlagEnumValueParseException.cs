namespace DMIS_Backend.Domain.Kernel.SmartEnums.Exceptions;

/// <summary>
/// 當無法將值解析為整數時拋出的例外
/// </summary>
public class InvalidFlagEnumValueParseException : Exception
{
  public InvalidFlagEnumValueParseException(string message)
      : base(message)
  {
  }
}
