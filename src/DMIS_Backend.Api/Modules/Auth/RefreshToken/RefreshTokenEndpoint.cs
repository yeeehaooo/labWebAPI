using DMIS_Backend.Api.Common;
using DMIS_Backend.Application.Kernel.Abstractions;
using DMIS_Backend.Application.Kernel.Workflows.IdentifierCodes;
using DMIS_Backend.Application.Modules.Auth.Commands.RefreshToken;
using DMIS_Backend.Domain.Kernel.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMIS_Backend.Api.Modules.Auth.RefreshToken;

/// <summary>
/// 刷新 Token 端點
/// 驗證舊 Token 並產生新 Token
/// </summary>
[ApiController]
[Tags("Auth")]
[Route("api/auth")]
[ModuleCode(nameof(ModuleCode.Auth))]
public class RefreshTokenEndpoint : ControllerBase
{
  /// <summary>
  /// 刷新 JWT Token
  /// </summary>
  /// <param name="request">刷新 Token 請求</param>
  /// <param name="handler">刷新 Token 處理器</param>
  /// <param name="cancellationToken">取消令牌</param>
  /// <returns>新的 JWT Token</returns>
  [HttpPost("refresh")]
  [AllowAnonymous]
  [OperationType(nameof(OperationType.Command))]
  [ScopeFunction(nameof(ScopeFunction.UNKNOW))]
  [ProducesResponseType(typeof(APIResponse<RefreshTokenResponse>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Handle(
    [FromBody] RefreshTokenRequest request,
    [FromServices] IUseCaseCommandHandler<RefreshTokenCommand, RefreshTokenResult> handler,
    CancellationToken cancellationToken)
  {
    // 將 API Request 轉換為 Application Command
    var command = request.ToCommand();

    // 呼叫 UseCase Handler
    var result = await handler.HandleAsync(command, cancellationToken);

    // 將 Application Result 轉換為 API Response
    return Ok(result.Map(data => data.ToRefreshTokenResponse()));
  }
}
