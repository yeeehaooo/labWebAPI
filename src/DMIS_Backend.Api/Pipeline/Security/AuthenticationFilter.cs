using DMIS_Backend.Api.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DMIS_Backend.Api.Pipeline.Security;

/// <summary>
/// HTTP 技術授權 Filter
/// 只檢查技術層面的授權（Authentication、Token 有效性等）
/// 不檢查業務權限（Permission），業務權限由 AuthorizationDecorator 處理
/// </summary>
public class AuthenticationFilter : IAsyncAuthorizationFilter
{
  /// <summary>
  /// 執行授權檢查
  /// </summary>
  /// <param name="context">授權過濾器上下文</param>
  /// <returns>授權任務</returns>
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
      // 建立驗證失敗的 APIResponse（使用基礎錯誤碼，讓 Stage 2 Filter 來解析完整的 WorkflowCode）
      var apiResponse = new APIResponse<object>(Code: "AUTH001", Message: "需要身份驗證");

      context.Result = new ObjectResult(apiResponse)
      {
        StatusCode = StatusCodes.Status401Unauthorized,
      };

      return Task.CompletedTask;
    }

    return Task.CompletedTask;
  }
}
