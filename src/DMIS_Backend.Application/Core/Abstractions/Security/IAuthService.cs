using System.Security.Claims;

namespace DMIS_Backend.Application.Core.Abstractions.Security;

/// <summary>
/// 認證服務抽象介面
/// 定義在 Application 層，由 Infrastructure 層實作
/// 提供認證、Token 產生和驗證等核心功能
/// </summary>
public interface IAuthService
{
  /// <summary>
  /// 驗證使用者憑證
  /// </summary>
  /// <param name="username">使用者名稱</param>
  /// <param name="password">密碼</param>
  /// <param name="ct">取消令牌</param>
  /// <returns>認證成功返回使用者資訊，失敗返回 null</returns>
  Task<AuthUser?> ValidateCredentialsAsync(string username, string password, CancellationToken ct);

  /// <summary>
  /// 產生 JWT Token
  /// </summary>
  /// <param name="claims">要包含在 Token 中的 Claims</param>
  /// <param name="expirationMinutes">過期時間（分鐘），預設 60 分鐘</param>
  /// <returns>JWT Token 字串</returns>
  string GenerateToken(IEnumerable<Claim> claims, int expirationMinutes = 60);

  /// <summary>
  /// 驗證 JWT Token
  /// </summary>
  /// <param name="token">要驗證的 JWT Token</param>
  /// <returns>驗證成功返回 ClaimsPrincipal，失敗返回 null</returns>
  ClaimsPrincipal? ValidateToken(string token);

  /// <summary>
  /// 驗證 Refresh Token（查詢 DB）
  /// </summary>
  /// <param name="refreshToken">Refresh Token</param>
  /// <param name="ct">取消令牌</param>
  /// <returns>驗證成功返回 RefreshToken 資訊，失敗返回 null</returns>
  Task<RefreshTokenInfo?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken ct);

  /// <summary>
  /// 儲存 Refresh Token 到 DB
  /// </summary>
  /// <param name="userId">使用者 ID</param>
  /// <param name="refreshToken">Refresh Token</param>
  /// <param name="expiresAt">過期時間</param>
  /// <param name="ct">取消令牌</param>
  Task SaveRefreshTokenAsync(string userId, string refreshToken, DateTime expiresAt, CancellationToken ct);
}

/// <summary>
/// 認證使用者資訊
/// </summary>
/// <param name="UserId">使用者 ID</param>
/// <param name="Username">使用者名稱</param>
/// <param name="Email">電子郵件</param>
/// <param name="Roles">角色列表</param>
public record AuthUser(
  string UserId,
  string Username,
  string Email,
  List<string> Roles
);

/// <summary>
/// Refresh Token 資訊
/// </summary>
/// <param name="UserId">使用者 ID</param>
/// <param name="Token">Token 字串</param>
/// <param name="ExpiresAt">過期時間</param>
/// <param name="IsRevoked">是否已撤銷</param>
public record RefreshTokenInfo(
  string UserId,
  string Token,
  DateTime ExpiresAt,
  bool IsRevoked
);
