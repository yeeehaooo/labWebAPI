using DMIS_Backend.Domain.Kernel.Guards.Exceptions;
using DMIS_Backend.Domain.Kernel.Primitives;

namespace DMIS_Backend.Application.Core.Results;

public static class ResultFactory
{
  public static Result FromBusiness(BusinessException ex)
      => Result.Failure(ex.ErrorCode, ex.DisplayMessage);

  public static Result<T> FromBusiness<T>(BusinessException ex)
      => Result<T>.Failure(ex.ErrorCode, ex.DisplayMessage);

  public static Result FromSystem(ErrorCode code)
      => Result.Failure(code);

  public static Result<T> FromSystem<T>(ErrorCode code)
      => Result<T>.Failure(code);
}
