using DMIS_Backend.Application.Modules.Auth.Commands.RefreshToken;

namespace DMIS_Backend.Api.Modules.Auth.RefreshToken;

/// <summary>
/// RefreshToken 模組的映射擴充方法
/// 提供 API Request → Application Command 和 Application Result → API Response 的轉換
/// </summary>
public static class RefreshTokenMapping
{
  /// <summary>
  /// API RefreshTokenRequest → Application RefreshTokenCommand
  /// </summary>
  public static RefreshTokenCommand ToCommand(this RefreshTokenRequest request) =>
    new()
    {
      Token = request.Token,
      ExpirationMinutes = request.ExpirationMinutes
    };

  /// <summary>
  /// Application RefreshTokenResult → API RefreshTokenResponse
  /// </summary>
  public static RefreshTokenResponse ToRefreshTokenResponse(this RefreshTokenResult result) =>
    new(
      Token: result.Token,
      TokenType: "Bearer",
      ExpiresIn: result.ExpiresIn * 60 // RefreshTokenResult.ExpiresIn 是分鐘，轉換為秒
    );
}
