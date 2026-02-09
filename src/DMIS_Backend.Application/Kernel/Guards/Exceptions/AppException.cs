using DMIS_Backend.Domain.Kernel.ErrorCodes;
using DMIS_Backend.Domain.Kernel.Guards;
using DMIS_Backend.Domain.Kernel.Guards.Exceptions;

namespace DMIS_Backend.Application.Kernel.Guards.Exceptions;

public sealed class AppException : GuardException<ErrorCode>
{
  public AppException(ErrorCode code, GuardContext ctx, string? message = null)
      : base(code, ctx, message)
  {
  }
}
