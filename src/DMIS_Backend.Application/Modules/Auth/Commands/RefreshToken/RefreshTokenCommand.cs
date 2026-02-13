using DMIS_Backend.Application.Core.Abstractions.Commands;

namespace DMIS_Backend.Application.Modules.Auth.Commands.RefreshToken;

/// <summary>
/// 刷新 Token 命令
/// </summary>
public record RefreshTokenCommand : IUseCaseCommand<RefreshTokenResult>
{
  /// <summary>
  /// 舊的 JWT Token 或 Refresh Token
  /// </summary>
  public string Token { get; init; } = string.Empty;

  /// <summary>
  /// 新 Token 過期時間（分鐘），預設 60 分鐘
  /// </summary>
  public int? ExpirationMinutes { get; init; }
}
