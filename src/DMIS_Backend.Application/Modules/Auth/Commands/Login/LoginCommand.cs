using DMIS_Backend.Application.Kernel.Abstractions;

namespace DMIS_Backend.Application.Modules.Auth.Commands.Login;

/// <summary>
/// 登入命令
/// </summary>
public record LoginCommand : IUseCaseCommand<LoginResult>
{
  /// <summary>
  /// 使用者名稱
  /// </summary>
  public string Username { get; init; } = string.Empty;

  /// <summary>
  /// 密碼
  /// </summary>
  public string Password { get; init; } = string.Empty;

  /// <summary>
  /// Token 過期時間（分鐘），預設 60 分鐘
  /// </summary>
  public int? ExpirationMinutes { get; init; }
}
