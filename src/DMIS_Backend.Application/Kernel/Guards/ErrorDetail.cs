using DMIS_Backend.Domain.Kernel.ErrorCodes;

namespace DMIS_Backend.Application.Kernel.Guards;

/// <summary>
/// 錯誤詳細資訊
/// 包含錯誤碼、診斷位置、堆疊追蹤和驗證錯誤
/// </summary>
public sealed class ErrorDetail
{
  ///// <summary>
  ///// 錯誤來源 Domain Application 或 Infrastructure（例如 "Domain"、"Application"、"Infrastructure"）
  ///// </summary>
  //public string Source { get; }

  /// <summary>
  /// 錯誤碼
  /// </summary>
  public IErrorCode ErrorCode { get; }

  /// <summary>
  /// 診斷位置（Guard 追蹤資訊）
  /// </summary>
  public string? GuardTrace { get; }

  /// <summary>
  /// 技術診斷（Exception StackTrace，僅開發環境使用）
  /// </summary>
  public string? StackTrace { get; }

  /// <summary>
  /// 驗證錯誤列表
  /// </summary>
  public IReadOnlyCollection<ValidationError>? ValidationErrors { get; }

  /// <summary>
  /// 初始化錯誤詳細資訊
  /// </summary>
  /// <param name="errorCode">錯誤碼</param>
  /// <param name="guardTrace">Guard 追蹤資訊</param>
  /// <param name="stackTrace">堆疊追蹤</param>
  /// <param name="validationErrors">驗證錯誤列表</param>
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
  /// <summary>
  /// 建立不包含技術細節的錯誤詳細資訊（用於生產環境）
  /// </summary>
  /// <returns>不包含技術細節的錯誤詳細資訊</returns>
  public ErrorDetail WithoutTechnicalDetails()
  => new(ErrorCode, GuardTrace);
}
