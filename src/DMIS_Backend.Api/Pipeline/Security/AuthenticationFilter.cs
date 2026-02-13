using DMIS_Backend.Api.Common.Responses;
using DMIS_Backend.Domain.Kernel.Primitives;
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

      //// 1️ 取得 Workflow
      //var workflow = Workflow.Current;

      //// 2️ 使用 SystemCode.Authentication
      //var finalCode = workflow.Code.Build(SystemCode.Authentication);

      // 3️ 建立 APIResponse（純 DTO）
      var apiResponse = ApiResponseHelper.Failure<object>(SystemCode.Authentication);

      context.Result = new ObjectResult(apiResponse)
      {
        StatusCode = StatusCodes.Status401Unauthorized,
      };

      return Task.CompletedTask;
    }

    return Task.CompletedTask;
  }
}
