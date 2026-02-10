using DMIS_Backend.Domain.Kernel.ErrorCodes;
using DMIS_Backend.Domain.Kernel.Guards;
using DMIS_Backend.Domain.Kernel.Guards.Exceptions;

namespace DMIS_Backend.Application.Kernel.Guards.Exceptions;

/// <summary>
/// Application 層例外
/// 用於應用層業務邏輯錯誤，由 AppGuard 拋出
/// </summary>
public sealed class AppException : GuardException<ErrorCode>
{
  /// <summary>
  /// 初始化 Application 層例外
  /// </summary>
  /// <param name="code">錯誤碼</param>
  /// <param name="ctx">Guard 上下文</param>
  /// <param name="message">錯誤訊息（可選）</param>
  public AppException(ErrorCode code, GuardContext ctx, string? message = null)
      : base(code, ctx, message)
  {
  }
}
