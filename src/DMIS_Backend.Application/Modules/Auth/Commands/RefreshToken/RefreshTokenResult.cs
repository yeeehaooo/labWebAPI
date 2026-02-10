namespace DMIS_Backend.Application.Modules.Auth.Commands.RefreshToken;

/// <summary>
/// 刷新 Token 結果
/// </summary>
public record RefreshTokenResult
{
  /// <summary>
  /// 新的 JWT Token
  /// </summary>
  public string Token { get; init; } = string.Empty;

  /// <summary>
  /// 過期時間（分）
  /// </summary>
  public int ExpiresIn { get; init; }
}
