using DMIS_Backend.Domain.Kernel.ErrorCodes;

namespace DMIS_Backend.Application.Kernel.Guards;

public sealed class ErrorDetail
{
  ///// <summary>
  ///// 錯誤來源 Domain Application 或 Infrastructure（例如 "Domain"、"Application"、"Infrastructure"）
  ///// </summary>
  //public string Source { get; }

  //public string Message { get; }
  public IErrorCode ErrorCode { get; }
  /// <summary>
  /// 診斷位置
  /// </summary>
  public string? GuardTrace { get; }

  /// <summary>
  /// 技術診斷（Exception StackTrace，dev-only）
  /// </summary>
  public string? StackTrace { get; }

  public IReadOnlyCollection<ValidationError>? ValidationErrors { get; }
  public ErrorDetail(
    IErrorCode errorCode,
      string? guardTrace = null,
      string? stackTrace = null,
      IEnumerable<ValidationError>? validationErrors = null)
  {
    this.ErrorCode = errorCode;
    GuardTrace = guardTrace;
    StackTrace = stackTrace;
    ValidationErrors = validationErrors?.ToList();
  }
  public ErrorDetail WithoutTechnicalDetails()
  => new(ErrorCode, GuardTrace);
}
