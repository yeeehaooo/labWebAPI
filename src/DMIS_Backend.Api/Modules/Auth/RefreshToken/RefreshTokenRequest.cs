namespace DMIS_Backend.Api.Modules.Auth.RefreshToken;

/// <summary>
/// 刷新 Token 請求 DTO
/// </summary>
/// <param name="Token">舊的 JWT Token</param>
/// <param name="ExpirationMinutes">新 Token 過期時間（分鐘），預設 60 分鐘</param>
public sealed record RefreshTokenRequest(
  string Token,
  int? ExpirationMinutes = null
);
