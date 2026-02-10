namespace DMIS_Backend.Api.Modules.Auth.RefreshToken;

/// <summary>
/// 刷新 Token 回應 DTO
/// </summary>
/// <param name="Token">新的 JWT Token</param>
/// <param name="TokenType">Token 類型（通常是 Bearer）</param>
/// <param name="ExpiresIn">過期時間（秒）</param>
public sealed record RefreshTokenResponse(
  string Token,
  string TokenType,
  int ExpiresIn
);
