using DMIS_Backend.Application.Core.Abstractions.Logging;
using Microsoft.Extensions.Logging;

namespace DMIS_Backend.Infrastructure.Logging;

/// <summary>
/// Serilog 日誌服務實作
/// 提供統一的日誌介面，使用 Microsoft.Extensions.Logging 作為底層實作
/// </summary>
public class SerilogLoggingService : ILoggingService
{
  private readonly ILogger<SerilogLoggingService> _logger;

  public SerilogLoggingService(ILogger<SerilogLoggingService> logger)
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
