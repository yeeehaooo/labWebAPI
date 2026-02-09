using DMIS_Backend.Domain.Kernel.ErrorCodes;

namespace DMIS_Backend.Domain.Kernel.Guards.Exceptions;

public abstract class GuardException<TErrorCode> : Exception
    where TErrorCode : IErrorCode
{
  /// 規則識別碼（協議層會用到）
  public TErrorCode ErrorCode { get; }

  /// 規則發生位置（診斷用）
  public GuardContext Context { get; }

  /// 對外訊息
  public string DisplayMessage { get; }

  protected GuardException(
      TErrorCode errorCode,
      GuardContext context,
      string? customMessage = null,
      Exception? inner = null)
      : base(errorCode.Description, inner)
  {
    ErrorCode = errorCode;
    Context = context;
    DisplayMessage = customMessage ?? errorCode.Description;
  }
}

