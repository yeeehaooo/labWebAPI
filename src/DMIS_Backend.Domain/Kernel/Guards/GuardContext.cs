namespace DMIS_Backend.Domain.Kernel.Guards;

/// <summary>
/// Guard 觸發上下文（僅用於診斷與 Log，不屬於業務語意）
/// </summary>
/// <param name="Member"></param>
/// <param name="File"></param>
/// <param name="Line"></param>
/// <param name="ArgumentExpression"></param>
public readonly record struct GuardContext(
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
