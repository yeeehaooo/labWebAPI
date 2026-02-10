namespace DMIS_Backend.Domain.Kernel.SmartEnums.Exceptions;

/// <summary>
/// 當值為負數（除了 -1）時拋出的例外
/// </summary>
public class NegativeValueArgumentException : Exception
{
  public NegativeValueArgumentException(string message)
      : base(message)
  {
  }
}
