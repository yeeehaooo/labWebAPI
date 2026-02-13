using DMIS_Backend.Api.Common.Responses;
using DMIS_Backend.Application.Core.Abstractions.Commands;
using DMIS_Backend.Application.Core.Workflows;
using DMIS_Backend.Application.Modules.Auth.Commands.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMIS_Backend.Api.Modules.Auth.Login;

/// <summary>
/// 登入端點
/// 產生 JWT Token 用於後續 API 認證
/// </summary>
[ApiController]
[Tags("Auth")]
[Route("api/auth")]
public class LoginEndpoint : ControllerBase
{
  /// <summary>
  /// 使用者登入
  /// </summary>
  /// <param name="request">登入請求</param>
  /// <param name="handler">登入處理器</param>
  /// <param name="cancellationToken">取消令牌</param>
  /// <returns>JWT Token 和相關資訊</returns>
  [HttpPost("login")]
  [AllowAnonymous]
  [ProducesResponseType(typeof(APIResponse<LoginResponse>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(APIResponse<object>), StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Handle(
    [FromBody] LoginRequest request,
    [FromServices] IUseCaseCommandHandler<LoginCommand, LoginResult> handler,
    CancellationToken cancellationToken)
  {
    Workflow.Set(WorkflowCode.Login);
    // 將 API Request 轉換為 Application Command
    var command = request.ToCommand();

    // 呼叫 UseCase Handler
    var result = await handler.HandleAsync(command, cancellationToken);

    var response = result.Map(data => data.ToLoginResponse(request)).ToApiResponse();

    return Ok(response);
  }
}
