namespace DMIS_Backend.Api.Modules.Auth.Login;

/// <summary>
/// 登入請求 DTO
/// </summary>
/// <param name="Username">使用者名稱</param>
/// <param name="Password">密碼</param>
/// <param name="ExpirationMinutes">Token 過期時間（分鐘），預設 60 分鐘</param>
public sealed record LoginRequest(
  string Username,
  string Password,
  int? ExpirationMinutes = null
);
