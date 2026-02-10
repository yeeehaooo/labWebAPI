namespace DMIS_Backend.Application.Kernel.ErrorCodes;

/// <summary>
/// Use Case 執行期間的錯誤來源上下文（Execution Context）
///
/// 用途：
/// - 在 Handler 進入時設定來源（HandlerSourceDecorator）
/// - 在 ExceptionToResultDecorator / Middleware 讀取
/// - 一個 request / async flow 只會有一個值
///
/// 設計原則：
/// - 不依賴 HTTP
/// - 不依賴 Domain
/// - 非同步安全
/// </summary>
public static class ErrorSourceContext
{
  // AsyncLocal：確保 async/await 不會串錯 request
  private static readonly AsyncLocal<string?> _current = new();

  /// <summary>
  /// 設定目前 Use Case 的錯誤來源
  /// 通常只會在 HandlerSourceDecorator 呼叫
  /// </summary>
  public static void Set(string source)
  {
    _current.Value = source;
  }

  /// <summary>
  /// 取得目前錯誤來源
  /// ExceptionToResultDecorator / Middleware 使用
  /// </summary>
  public static string? Get()
  {
    return _current.Value;
  }

  /// <summary>
  /// 清除來源（避免 context 泄漏）
  /// 一定要在 finally 區塊呼叫
  /// </summary>
  public static void Clear()
  {
    _current.Value = null;
  }
}

