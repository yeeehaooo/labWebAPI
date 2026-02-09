using DMIS_Backend.Api.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DMIS_Backend.Api.Filters;

/// <summary>
/// HTTP 技術授權 Filter
/// 只檢查技術層面的授權（Authentication、Token 有效性等）
/// 不檢查業務權限（Permission），業務權限由 AuthorizationDecorator 處理
/// </summary>
public class AuthenticationFilter : IAsyncAuthorizationFilter
{
  public Task OnAuthorizationAsync(AuthorizationFilterContext context)
  {
    // 如果允許匿名，直接放行
    var endpoint = context.HttpContext.GetEndpoint();
    if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
    {
      return Task.CompletedTask;
    }
    // 檢查是否已驗證（技術層）
    if (context.HttpContext.User.Identity?.IsAuthenticated != true)
    {
      // 建立驗證失敗的 APIResponse
      var apiResponse = new APIResponse<object>(
        Code: "Code.授權失敗",
        Message: "需要身份驗證"
      );

      context.Result = new ObjectResult(apiResponse)
      {
        StatusCode = StatusCodes.Status401Unauthorized
      };

      return Task.CompletedTask;
    }

    return Task.CompletedTask;
  }
}
