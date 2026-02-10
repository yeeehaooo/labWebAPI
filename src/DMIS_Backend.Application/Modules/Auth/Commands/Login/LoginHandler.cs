using System.Security.Claims;
using DMIS_Backend.Application.Kernel.Abstractions;
using DMIS_Backend.Application.Kernel.Guards;
using DMIS_Backend.Application.Kernel.Results;
using DMIS_Backend.Application.Kernel.Security;
using DMIS_Backend.Domain.Kernel.ErrorCodes;

namespace DMIS_Backend.Application.Modules.Auth.Commands.Login;

/// <summary>
/// 登入處理器
/// 負責處理使用者登入業務邏輯，包含憑證驗證和 Token 產生
/// </summary>
public class LoginHandler : IUseCaseCommandHandler<LoginCommand, LoginResult>
{
  private readonly IAuthService _authService;

  /// <summary>
  /// 初始化登入處理器
  /// </summary>
  /// <param name="authService">認證服務</param>
  public LoginHandler(IAuthService authService)
  {
    _authService = authService;
  }

  /// <summary>
  /// 處理登入命令
  /// </summary>
  /// <param name="command">登入命令</param>
  /// <param name="ct">取消令牌</param>
  /// <returns>處理結果</returns>
  public async Task<Result<LoginResult>> HandleAsync(LoginCommand command, CancellationToken ct)
  {
    try
    {
      // 驗證輸入
      if (string.IsNullOrWhiteSpace(command.Username) || string.IsNullOrWhiteSpace(command.Password))
      {
        return Result<LoginResult>.Failure(
          code: "AUTH001",
          message: "使用者名稱或密碼不能為空",
          error: new ErrorDetail(ErrorCode.TokenAuthError, null, null)
        );
      }

      // 驗證使用者憑證（會查詢 DB）
      var user = await _authService.ValidateCredentialsAsync(
        command.Username,
        command.Password,
        ct
      );

      if (user == null)
      {
        return Result<LoginResult>.Failure(
          code: "AUTH002",
          message: "使用者名稱或密碼錯誤",
          error: new ErrorDetail(ErrorCode.TokenAuthError, null, null)
        );
      }

      // 建立 Claims
      var claims = new List<Claim>
      {
        new(ClaimTypes.NameIdentifier, user.UserId),
        new(ClaimTypes.Name, user.Username),
        new(ClaimTypes.Email, user.Email),
        new("sub", user.UserId)
      };

      foreach (var role in user.Roles)
      {
        claims.Add(new Claim(ClaimTypes.Role, role));
      }

      // 產生 JWT Token
      var expirationMinutes = command.ExpirationMinutes ?? 60;
      var token = _authService.GenerateToken(claims, expirationMinutes);

      // 產生 Refresh Token（如果需要）
      var refreshToken = Guid.NewGuid().ToString();
      await _authService.SaveRefreshTokenAsync(
        user.UserId,
        refreshToken,
        DateTime.UtcNow.AddDays(7),
        ct
      );

      // 建立結果
      var result = new LoginResult
      {
        Token = token,
        ExpiresIn = expirationMinutes * 60,
        RefreshToken = refreshToken
      };

      return Result<LoginResult>.Success(result);
    }
    catch (Exception ex)
    {
      return Result<LoginResult>.Failure(ErrorCode.ApplicationLayerError, ex);
    }
  }
}
