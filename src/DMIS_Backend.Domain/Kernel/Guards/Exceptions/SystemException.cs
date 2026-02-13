using DMIS_Backend.Domain.Kernel.Primitives;

namespace DMIS_Backend.Domain.Kernel.Guards.Exceptions;

/// <summary>
/// 系統例外
/// </summary>
public sealed class SystemException : BusinessException
{
  public SystemException(
      SystemCode errorCode,
      string? displayMessage = null)
      : base(errorCode, displayMessage)
  {
  }
}
