namespace DMIS_Backend.Domain.Kernel.Guards;

/// <summary>
/// Exception 觸發上下文（僅用於診斷與 Log，不屬於業務語意）
/// 記錄 Exception 驗證失敗時的位置資訊，用於錯誤追蹤和日誌記錄
/// </summary>
/// <param name="Member">觸發 Exception 的成員名稱（方法或屬性）</param>
/// <param name="File">觸發 Exception 的檔案路徑</param>
/// <param name="Line">觸發 Exception 的行號</param>
/// <param name="ArgumentExpression">觸發 Exception 的參數表達式（可選）</param>
public readonly record struct TraceContext(
    string Member,
    string File,
    int Line,
    string? ArgumentExpression = null)
{
  private static string ShortFile(string path)
      => System.IO.Path.GetFileName(path);

  /// <summary>
  /// 安全診斷資訊（不包含完整路徑）（通常用於 Guard 失敗的錯誤訊息）
  /// </summary>
  /// <returns></returns>
  public string DiagnosticInfo
      => ArgumentExpression is null
          ? $"{Member} ({ShortFile(File)}:{Line})"
          : $"{Member} ({ShortFile(File)}:{Line}) [{ArgumentExpression}]";

  /// <summary>
  /// 安全診斷資訊（含完整路徑）
  /// </summary>
  public string FullDiagnosticInfo =>
      ArgumentExpression is null
          ? $"{Member} ({File}:{Line})"
          : $"{Member} ({File}:{Line}) [{ArgumentExpression}]";

  public override string ToString() => DiagnosticInfo;

}

