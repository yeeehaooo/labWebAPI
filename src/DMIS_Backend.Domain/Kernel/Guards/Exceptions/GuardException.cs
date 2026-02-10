using DMIS_Backend.Domain.Kernel.ErrorCodes;

namespace DMIS_Backend.Domain.Kernel.Guards.Exceptions;

/// <summary>
/// Guard 例外基類
/// 所有 Guard 驗證失敗時拋出的例外都繼承自此類別
/// </summary>
/// <typeparam name="TErrorCode">錯誤碼類型</typeparam>
public abstract class GuardException<TErrorCode> : Exception
    where TErrorCode : IErrorCode
{
  /// <summary>
  /// 規則識別碼（協議層會用到）
  /// </summary>
  public TErrorCode ErrorCode { get; }

  /// <summary>
  /// 規則發生位置（診斷用）
  /// </summary>
  public GuardContext Context { get; }

  /// <summary>
  /// 對外訊息（顯示給使用者的錯誤訊息）
  /// </summary>
  public string DisplayMessage { get; }

  /// <summary>
  /// 初始化 Guard 例外
  /// </summary>
  /// <param name="errorCode">錯誤碼</param>
  /// <param name="context">Guard 上下文</param>
  /// <param name="customMessage">自訂錯誤訊息（可選）</param>
  /// <param name="inner">內部例外（可選）</param>
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
