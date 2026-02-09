namespace DMIS_Backend.Domain.Kernel.SmartEnums.Exceptions;

/// <summary>
/// 當 SmartFlagEnum 包含負值（除了 -1）時拋出的例外
/// </summary>
public class SmartFlagEnumContainsNegativeValueException : Exception
{
  public SmartFlagEnumContainsNegativeValueException(string message)
      : base(message)
  {
  }
}
