using System.Runtime.CompilerServices;
using DMIS_Backend.Domain.Kernel.Primitives;

namespace DMIS_Backend.Domain.Kernel.Guards.Exceptions;

/// <summary>
/// 業務語意例外基底
/// </summary>
public abstract class BusinessException : Exception
{
  /// <summary>
  /// 錯誤碼定義
  /// </summary>
  public ErrorCode ErrorCode { get; }

  /// <summary>
  /// 顯示訊息（可由 UseCase 覆寫）
  /// </summary>
  public string DisplayMessage { get; }

  /// <summary>
  /// 條件字串（由 CallerArgumentExpression 傳入）
  /// </summary>
  public string? ConditionExpression { get; }

  public TraceContext Context { get; }

  protected BusinessException(
      ErrorCode errorCode,
      string? displayMessage = null,
      string? conditionExpression = null,
      [CallerMemberName] string? member = null,
      [CallerFilePath] string? file = null,
      [CallerLineNumber] int line = 0)
      : base(displayMessage ?? errorCode.Message)
  {
    ErrorCode = errorCode;
    DisplayMessage = displayMessage ?? errorCode.Message;
    ConditionExpression = conditionExpression;
    Context = new TraceContext(member, file, line);
  }
}
