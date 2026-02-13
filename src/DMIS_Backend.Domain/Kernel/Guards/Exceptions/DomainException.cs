using System.Runtime.CompilerServices;
using DMIS_Backend.Domain.Kernel.Primitives;

namespace DMIS_Backend.Domain.Kernel.Guards.Exceptions;

/// <summary>
/// 領域規則違反例外
/// </summary>
public sealed class DomainException : BusinessException
{
  public DomainException(
      DomainCode errorCode,
      string? displayMessage = null,
      string? conditionExpression = null,
      [CallerMemberName] string? member = null,
      [CallerFilePath] string? file = null,
      [CallerLineNumber] int line = 0)
      : base(errorCode, displayMessage, conditionExpression, member, file, line)
  {
  }
}
