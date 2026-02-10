using System.Runtime.CompilerServices;
using DMIS_Backend.Domain.Kernel.ErrorCodes;
using DMIS_Backend.Domain.Kernel.Guards.Exceptions;

namespace DMIS_Backend.Domain.Kernel.Guards;

/// <summary>
/// Domain Guard 擴充方法
/// 提供領域不變條件驗證的擴充方法
/// </summary>
public static class DomainGuardExtensions
{
  /// <summary>
  /// 驗證條件必須為 true，否則拋出 DomainException
  /// </summary>
  /// <param name="_">Domain Guard 子句（用於擴充方法）</param>
  /// <param name="condition">驗證條件</param>
  /// <param name="errorCode">錯誤碼</param>
  /// <param name="displayMessage">顯示訊息（可選）</param>
  /// <param name="argument">參數表達式（自動取得）</param>
  /// <param name="member">成員名稱（自動取得）</param>
  /// <param name="file">檔案路徑（自動取得）</param>
  /// <param name="line">行號（自動取得）</param>
  public static void Must(
      this IDomainGuardClause _,
      bool condition,
      ErrorCode errorCode,
      string displayMessage = null,
      [CallerArgumentExpression("condition")] string? argument = null,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
  {
    if (!condition)
    {
      var ctx = new GuardContext(member, file, line, argument);
      throw new DomainException(errorCode, ctx, displayMessage);
    }
  }
}
