namespace DMIS_Backend.Application.Modules.Auth.Commands.Login;

/// <summary>
/// 登入結果
/// </summary>
public record LoginResult
{
  /// <summary>
  /// JWT Token
  /// </summary>
  public string Token { get; init; } = string.Empty;

  /// <summary>
  /// Refresh Token（可選）
  /// </summary>
  public string? RefreshToken { get; init; } = string.Empty;

  /// <summary>
  /// 過期時間（秒）
  /// </summary>
  public int ExpiresIn { get; init; }
}
