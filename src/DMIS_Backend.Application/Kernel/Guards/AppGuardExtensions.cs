using System.Runtime.CompilerServices;
using DMIS_Backend.Application.Kernel.Guards.Exceptions;
using DMIS_Backend.Domain.Kernel.ErrorCodes;
using DMIS_Backend.Domain.Kernel.Guards;

namespace DMIS_Backend.Application.Kernel.Guards;

public static class AppGuardExtensions
{
  public static void ThrowIf(
      this IAppGuardClause _,
      bool condition,
      ErrorCode errorCode,
      string? message = null,
      [CallerArgumentExpression("condition")] string? argument = null,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
  {
    if (!condition)
    {
      var ctx = new GuardContext(member, file, line, argument);
      throw new AppException(errorCode, ctx, message);
    }
  }
}
