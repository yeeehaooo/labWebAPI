using System.Security.Claims;
using DMIS_Backend.Application.Core.Abstractions.Commands;
using DMIS_Backend.Application.Core.Abstractions.Security;
using DMIS_Backend.Application.Core.Results;
using DMIS_Backend.Domain.Kernel.Guards;
using DMIS_Backend.Domain.Kernel.Primitives;

namespace DMIS_Backend.Application.Modules.Auth.Commands.RefreshToken;

/// <summary>
/// 刷新 Token 處理器
/// 負責驗證舊 Token 並產生新 Token
/// </summary>
public class RefreshTokenHandler : IUseCaseCommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
  private readonly IAuthService _authService;

  /// <summary>
  /// 初始化刷新 Token 處理器
  /// </summary>
  /// <param name="authService">認證服務</param>
  public RefreshTokenHandler(IAuthService authService)
  {
    _authService = authService;
  }

  /// <summary>
  /// 處理刷新 Token 命令
  /// </summary>
  /// <param name="command">刷新 Token 命令</param>
  /// <param name="ct">取消令牌</param>
  /// <returns>處理結果</returns>
  public async Task<Result<RefreshTokenResult>> HandleAsync(RefreshTokenCommand command, CancellationToken ct)
  {
    try
    {
      // 驗證輸入
      Guard.Application.Must(string.IsNullOrWhiteSpace(command.Token), ApplicationCode.ValidationFailed, "token empty.");

      // 驗證舊 Token（目前只驗證 JWT Token，後續可擴展為驗證 RefreshToken）
      var principal = _authService.ValidateToken(command.Token);
      if (principal == null)
      {
        // 如果 JWT Token 驗證失敗，嘗試驗證 RefreshToken（未來擴展）
        var refreshTokenInfo = await _authService.ValidateRefreshTokenAsync(command.Token, ct);
        // 從 RefreshToken 取得使用者資訊（未來擴展）
        // 目前階段：如果 RefreshToken 有效，但無法取得 Claims，返回錯誤
        Guard.Application.Must(refreshTokenInfo == null || refreshTokenInfo.IsRevoked, ApplicationCode.InvalidState, "Token 無效或已過期.");
      }

      // 從舊 Token 中取得 Claims
      var claims = principal?.Claims.ToList() ?? new List<Claim>();

      // 產生新 Token
      var expirationMinutes = command.ExpirationMinutes ?? 60;
      var newToken = _authService.GenerateToken(claims, expirationMinutes);

      // 建立結果
      var result = new RefreshTokenResult
      {
        Token = newToken,
        ExpiresIn = expirationMinutes
      };

      return Result<RefreshTokenResult>.Success(result);
    }
    catch //(Exception ex)
    {
      throw;
      //return Result<RefreshTokenResult>.Failure(ErrorCode.ApplicationLayerError, ex);
    }
  }
}
