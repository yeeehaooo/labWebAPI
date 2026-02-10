namespace DMIS_Backend.Application.Kernel.Abstractions;

/// <summary>
/// 日誌服務介面
/// 定義應用層的日誌記錄契約，由 Infrastructure 層實作
/// </summary>
public interface ILoggingService
{
  /// <summary>
  /// 記錄資訊層級的日誌
  /// </summary>
  void LogInformation(string message, params object[] args);

  /// <summary>
  /// 記錄警告層級的日誌
  /// </summary>
  void LogWarning(string message, params object[] args);

  /// <summary>
  /// 記錄錯誤層級的日誌（包含例外）
  /// </summary>
  void LogError(Exception exception, string message, params object[] args);

  /// <summary>
  /// 記錄錯誤層級的日誌
  /// </summary>
  void LogError(string message, params object[] args);

  /// <summary>
  /// 記錄除錯層級的日誌
  /// </summary>
  void LogDebug(string message, params object[] args);
}
