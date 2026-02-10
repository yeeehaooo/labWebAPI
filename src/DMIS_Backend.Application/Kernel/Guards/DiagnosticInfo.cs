namespace DMIS_Backend.Application.Kernel.Guards;

/// <summary>
/// 診斷資訊
/// 用於記錄錯誤的診斷資訊，包含詳細的錯誤位置和上下文
/// </summary>
public sealed class DiagnosticInfo
{
  /// <summary>
  /// 診斷訊息（包含錯誤位置和上下文資訊）
  /// </summary>
  public string? DiagnosticMessage { get; }

  /// <summary>
  /// 初始化診斷資訊
  /// </summary>
  /// <param name="diagnosticInfo">診斷資訊字串</param>
  public DiagnosticInfo(string? diagnosticInfo)
  {
    DiagnosticMessage = diagnosticInfo;
  }
}
