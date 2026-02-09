namespace DMIS_Backend.Infrastructure.Encryption;

/// <summary>
/// 時間戳記驗證服務
/// 用於防止重放攻擊
/// </summary>
public interface ITimestampValidator
{
  /// <summary>
  /// 驗證時間戳記是否在有效時間窗內
  /// </summary>
  /// <param name="timestamp">請求時間戳記（Unix timestamp，毫秒）</param>
  /// <param name="tolerance">時間容差（預設 5 分鐘）</param>
  /// <returns>是否有效</returns>
  bool IsValid(long timestamp, TimeSpan? tolerance = null);

  /// <summary>
  /// 取得當前時間戳記（Unix timestamp，毫秒）
  /// </summary>
  long GetCurrentTimestamp();
}
