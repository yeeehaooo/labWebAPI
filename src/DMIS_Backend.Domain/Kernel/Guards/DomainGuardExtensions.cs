using System.Runtime.CompilerServices;
using DMIS_Backend.Domain.Kernel.ErrorCodes;
using DMIS_Backend.Domain.Kernel.Guards.Exceptions;

namespace DMIS_Backend.Domain.Kernel.Guards;

public static class DomainGuardExtensions
{
  public static void Must(
      this IDomainGuardClause _,
      bool condition,
      ErrorCode errorCode,
      string displayMessage = null,
      [CallerArgumentExpression("condition")] string? argument = null,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
  {
    if (!condition)
    {
      var ctx = new GuardContext(member, file, line, argument);
      throw new DomainException(errorCode, ctx, displayMessage);
    }
  }
}
