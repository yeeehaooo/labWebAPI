using System.Security.Claims;

namespace DMIS_Backend.Infrastructure.Security.Authentication.Jwt;

/// <summary>
/// JWT Token 服務介面
/// </summary>
public interface IJwtService
{
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
}
