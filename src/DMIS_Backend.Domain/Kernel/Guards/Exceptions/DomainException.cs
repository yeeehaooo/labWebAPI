using DMIS_Backend.Domain.Kernel.ErrorCodes;

namespace DMIS_Backend.Domain.Kernel.Guards.Exceptions;

/// <summary>
/// Domain 層例外
/// 用於領域業務規則違反，由 DomainGuard 拋出
/// </summary>
public sealed class DomainException
    : GuardException<ErrorCode>
{
  /// <summary>
  /// 初始化 Domain 層例外
  /// </summary>
  /// <param name="errorCode">錯誤碼</param>
  /// <param name="context">Guard 上下文</param>
  /// <param name="message">錯誤訊息（可選）</param>
  public DomainException(
      ErrorCode errorCode,
      GuardContext context,
      string? message = null)
      : base(errorCode, context, message) { }
}
