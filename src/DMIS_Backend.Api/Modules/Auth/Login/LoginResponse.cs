namespace DMIS_Backend.Api.Modules.Auth.Login;

/// <summary>
/// 登入回應 DTO
/// </summary>
/// <param name="Token">JWT Token</param>
/// <param name="TokenType">Token 類型（通常是 Bearer）</param>
/// <param name="ExpiresIn">過期時間（秒）</param>
/// <param name="Username">使用者名稱</param>
public sealed record LoginResponse(
  string Token,
  string TokenType,
  int ExpiresIn,
  string Username
);
