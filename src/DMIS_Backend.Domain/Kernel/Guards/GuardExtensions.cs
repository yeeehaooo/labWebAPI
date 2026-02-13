using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using DMIS_Backend.Domain.Kernel.Guards.Exceptions;
using DMIS_Backend.Domain.Kernel.Primitives;

namespace DMIS_Backend.Domain.Kernel.Guards;

/// <summary>
/// Guard 擴充方法<br />
/// 用於檢查前置條件
/// </summary>
public static class GuardExtensions
{
  /// <summary>
  /// 驗證條件為 false，拋出 DomainException
  /// </summary>
  /// <param name="_">Domain Guard 子句（用於擴充方法語法）</param>
  /// <param name="condition">驗證條件</param>
  /// <param name="errorCode">錯誤碼定義</param>
  /// <param name="displayMessage">對外顯示訊息（可選）</param>
  /// <param name="argument">條件表達式（自動取得）</param>
  /// <param name="member">成員名稱（自動取得）</param>
  /// <param name="file">檔案路徑（自動取得）</param>
  /// <param name="line">行號（自動取得）</param>
  public static void Must(
      this IDomainGuardClause _,
      [DoesNotReturnIf(false)] bool condition,
      DomainCode errorCode,
      string? displayMessage = null,
      [CallerArgumentExpression("condition")] string? argument = null,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
  {
    if (!condition)
    {
      throw new DomainException(
       errorCode,
       displayMessage,
       argument,
       member,
       file,
       line);
    }
  }

  /// <summary>
  /// 驗證條件為 false，拋出 AppException
  /// </summary>
  /// <param name="_">App Guard 子句（用於擴充方法語法）</param>
  /// <param name="condition">驗證條件</param>
  /// <param name="errorCode">錯誤碼定義</param>
  /// <param name="displayMessage">對外顯示訊息（可選）</param>
  /// <param name="argument">條件表達式（自動取得）</param>
  /// <param name="member">成員名稱（自動取得）</param>
  /// <param name="file">檔案路徑（自動取得）</param>
  /// <param name="line">行號（自動取得）</param>
  public static void Must(
      this IApplicationGuardClause _,
      [DoesNotReturnIf(false)] bool condition,
      ApplicationCode errorCode,
      string? displayMessage = null,
      [CallerArgumentExpression("condition")] string? argument = null,
      [CallerMemberName] string member = "",
      [CallerFilePath] string file = "",
      [CallerLineNumber] int line = 0)
  {
    if (!condition)
    {
      throw new AppException(
       errorCode,
       displayMessage,
       argument,
       member,
       file,
       line);
    }
  }

}
