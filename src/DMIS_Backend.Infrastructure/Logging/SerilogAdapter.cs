using Microsoft.Extensions.Logging;

namespace DMIS_Backend.Infrastructure.Logging;
/// <summary>
/// Microsoft.Extensions.Logging 適配器
/// 提供統一的日誌介面
/// </summary>
public class SerilogAdapter : ILoggerAdapter
{
  private readonly ILogger<SerilogAdapter> _logger;

  public SerilogAdapter(ILogger<SerilogAdapter> logger)
  {
    _logger = logger;
  }

  public void LogInformation(string message, params object[] args)
  {
    _logger.LogInformation(message, args);
  }

  public void LogWarning(string message, params object[] args)
  {
    _logger.LogWarning(message, args);
  }

  public void LogError(Exception exception, string message, params object[] args)
  {
    _logger.LogError(exception, message, args);
  }

  public void LogError(string message, params object[] args)
  {
    _logger.LogError(message, args);
  }

  public void LogDebug(string message, params object[] args)
  {
    _logger.LogDebug(message, args);
  }
}

/// <summary>
/// 日誌適配器介面
/// </summary>
public interface ILoggerAdapter
{
  void LogInformation(string message, params object[] args);
  void LogWarning(string message, params object[] args);
  void LogError(Exception exception, string message, params object[] args);
  void LogError(string message, params object[] args);
  void LogDebug(string message, params object[] args);
}
