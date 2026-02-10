using DMIS_Backend.Application.Modules.Auth.Commands.Login;

namespace DMIS_Backend.Api.Modules.Auth.Login;

/// <summary>
/// Login 模組的映射擴充方法
/// 提供 API Request → Application Command 和 Application Result → API Response 的轉換
/// </summary>
public static class LoginMapping
{
  /// <summary>
  /// API LoginRequest → Application LoginCommand
  /// </summary>
  public static LoginCommand ToCommand(this LoginRequest request) =>
    new()
    {
      Username = request.Username,
      Password = request.Password,
      ExpirationMinutes = request.ExpirationMinutes
    };

  /// <summary>
  /// Application LoginResult + LoginRequest → API LoginResponse
  /// </summary>
  public static LoginResponse ToLoginResponse(this LoginResult result, LoginRequest request) =>
    new(
      Token: result.Token,
      TokenType: "Bearer",
      ExpiresIn: result.ExpiresIn,
      Username: request.Username
    );
}
