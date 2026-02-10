namespace DMIS_Backend.Domain.Kernel.SmartEnums.Exceptions;

/// <summary>
/// 當 SmartFlagEnum 不包含連續的 2 的冪次值時拋出的例外
/// </summary>
public class SmartFlagEnumDoesNotContainPowerOfTwoValuesException : Exception
{
  public SmartFlagEnumDoesNotContainPowerOfTwoValuesException(string message)
      : base(message)
  {
  }
}
