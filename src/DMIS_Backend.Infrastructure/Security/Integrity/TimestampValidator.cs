using Microsoft.Extensions.Logging;

namespace DMIS_Backend.Infrastructure.Security.Integrity;

/// <summary>
/// 時間戳記驗證服務實作
/// </summary>
public class TimestampValidator : ITimestampValidator
{
  private readonly ILogger<TimestampValidator> _logger;
  private readonly TimeSpan _defaultTolerance = TimeSpan.FromMinutes(5);

  public TimestampValidator(ILogger<TimestampValidator> logger)
  {
    _logger = logger;
  }

  public bool IsValid(long timestamp, TimeSpan? tolerance = null)
  {
    var toleranceValue = tolerance ?? _defaultTolerance;
    var requestTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
    var currentTime = DateTimeOffset.UtcNow.DateTime;
    var timeDifference = currentTime - requestTime;

    // 檢查是否在時間窗內（允許未來時間，考慮時鐘偏差）
    var isValid = timeDifference <= toleranceValue && timeDifference >= -toleranceValue;

    if (!isValid)
    {
      _logger.LogWarning(
        "Timestamp validation failed. Request time: {RequestTime}, Current time: {CurrentTime}, Difference: {Difference}",
        requestTime, currentTime, timeDifference);
    }

    return isValid;
  }

  public long GetCurrentTimestamp()
  {
    return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
  }
}
