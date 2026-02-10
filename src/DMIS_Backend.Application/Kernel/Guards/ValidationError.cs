namespace DMIS_Backend.Application.Kernel.Guards;

/// <summary>
/// 驗證錯誤
/// 用於記錄欄位驗證失敗的詳細資訊
/// </summary>
public sealed class ValidationError
{
  /// <summary>
  /// 驗證失敗的欄位名稱
  /// </summary>
  public string Field { get; }

  /// <summary>
  /// 驗證錯誤訊息
  /// </summary>
  public string Message { get; }

  /// <summary>
  /// 初始化驗證錯誤
  /// </summary>
  /// <param name="field">欄位名稱</param>
  /// <param name="message">錯誤訊息</param>
  public ValidationError(string field, string message)
  {
    Field = field;
    Message = message;
  }
}
