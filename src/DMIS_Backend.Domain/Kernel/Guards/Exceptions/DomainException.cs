using DMIS_Backend.Domain.Kernel.ErrorCodes;

namespace DMIS_Backend.Domain.Kernel.Guards.Exceptions;

public sealed class DomainException
    : GuardException<ErrorCode>
{
  public DomainException(
      ErrorCode errorCode,
      GuardContext context,
      string? message = null)
      : base(errorCode, context, message) { }
}
