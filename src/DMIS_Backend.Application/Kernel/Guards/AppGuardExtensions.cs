using System.Runtime.CompilerServices;
using DMIS_Backend.Application.Kernel.Guards.Exceptions;
using DMIS_Backend.Domain.Kernel.ErrorCodes;
using DMIS_Backend.Domain.Kernel.Guards;

namespace DMIS_Backend.Application.Kernel.Guards;

/// <summary>
/// Application Guard 擴充方法
/// 提供應用層驗證的擴充方法，用於拋出 AppException
/// </summary>
public static class AppGuardExtensions
{
  /// <summary>
  /// 如果條件為 false，則拋出 AppException
  /// </summary>
  /// <param name="_">Application Guard 子句（用於擴充方法）</param>
  /// <param name="condition">驗證條件</param>
  /// <param name="errorCode">錯誤碼</param>
  /// <param name="message">自訂錯誤訊息（可選）</param>
  /// <param name="argument">參數表達式（自動取得）</param>
  /// <param name="member">成員名稱（自動取得）</param>
  /// <param name="file">檔案路徑（自動取得）</param>
  /// <param name="line">行號（自動取得）</param>
  public static void Must(
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
  /// <summary>
  /// 如果條件為 true ，則拋出 AppException
  /// </summary>
  /// <param name="_">Application Guard 子句（用於擴充方法）</param>
  /// <param name="condition">驗證條件</param>
  /// <param name="errorCode">錯誤碼</param>
  /// <param name="message">自訂錯誤訊息（可選）</param>
  /// <param name="argument">參數表達式（自動取得）</param>
  /// <param name="member">成員名稱（自動取得）</param>
  /// <param name="file">檔案路徑（自動取得）</param>
  /// <param name="line">行號（自動取得）</param>
  public static void Throw(
      this IAppGuardClause _,
      bool condition,
      ErrorCode errorCode,
      string? message = null,
      [CallerArgumentExpression("condition")] string? argument = null,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
  {
    if (condition)
    {
      var ctx = new GuardContext(member, file, line, argument);
      throw new AppException(errorCode, ctx, message);
    }
  }
}
