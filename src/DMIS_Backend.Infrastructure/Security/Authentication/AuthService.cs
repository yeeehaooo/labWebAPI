using System.Security.Claims;
using DMIS_Backend.Application.Core.Abstractions.Security;
using DMIS_Backend.Infrastructure.Security.Authentication.Jwt;

namespace DMIS_Backend.Infrastructure.Security.Authentication;

/// <summary>
/// 認證服務實作
/// 實作 Application 層定義的 IAuthService 介面
/// 目前階段：使用簡單的硬編碼驗證（測試用）
/// 後續可擴展為查詢 DB、使用 User Repository 等
/// </summary>
public class AuthService : IAuthService
{
  private readonly IJwtService _jwtService;

  /// <summary>
  /// 初始化認證服務
  /// </summary>
  /// <param name="jwtService">JWT 服務</param>
  public AuthService(IJwtService jwtService)
  {
    _jwtService = jwtService;
  }

  /// <summary>
  /// 驗證使用者憑證
  /// 目前階段：簡單的硬編碼驗證（測試用）
  /// 後續可擴展為查詢 DB、使用 User Repository
  /// </summary>
  public Task<AuthUser?> ValidateCredentialsAsync(string username, string password, CancellationToken ct)
  {
    // 暫時實作：簡單驗證（測試用）
    // 後續可擴展為：
    // 1. 查詢 User Repository
    // 2. 驗證密碼（使用加密服務）
    // 3. 檢查使用者狀態（啟用/停用）
    if (username == "admin" && password == "admin123")
    {
      var user = new AuthUser(
        UserId: "admin-user-id",
        Username: "admin",
        Email: "admin@example.com",
        Roles: new List<string> { "Admin" }
      );

      return Task.FromResult<AuthUser?>(user);
    }

    return Task.FromResult<AuthUser?>(null);
  }

  /// <summary>
  /// 產生 JWT Token
  /// </summary>
  public string GenerateToken(IEnumerable<Claim> claims, int expirationMinutes = 60)
  {
    return _jwtService.GenerateToken(claims, expirationMinutes);
  }

  /// <summary>
  /// 驗證 JWT Token
  /// </summary>
  public ClaimsPrincipal? ValidateToken(string token)
  {
    return _jwtService.ValidateToken(token);
  }

  /// <summary>
  /// 驗證 Refresh Token（查詢 DB）
  /// 目前階段：簡單實作，後續可擴展為查詢 RefreshToken Repository
  /// </summary>
  public Task<RefreshTokenInfo?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken ct)
  {
    // 暫時實作：簡單驗證
    // 後續可擴展為：
    // 1. 查詢 RefreshToken Repository
    // 2. 檢查是否已撤銷
    // 3. 檢查是否過期
    // 4. 取得使用者資訊
    return Task.FromResult<RefreshTokenInfo?>(null);
  }

  /// <summary>
  /// 儲存 Refresh Token 到 DB
  /// 目前階段：簡單實作，後續可擴展為儲存到 RefreshToken Repository
  /// </summary>
  public Task SaveRefreshTokenAsync(string userId, string refreshToken, DateTime expiresAt, CancellationToken ct)
  {
    // 暫時實作：不做任何事
    // 後續可擴展為：
    // 1. 儲存到 RefreshToken Repository
    // 2. 撤銷舊的 RefreshToken
    // 3. 記錄建立時間
    return Task.CompletedTask;
  }
}
