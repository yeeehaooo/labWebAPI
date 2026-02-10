using DMIS_Backend.Application.Kernel.Abstractions;
using DMIS_Backend.Application.Kernel.Guards;

namespace DMIS_Backend.Api.Common;

/// <summary>
/// Result 轉換為 APIResponse 的映射器
/// 分離業務層（Result）和表現層（APIResponse）
/// </summary>
public static class ResultToApiResponseMapper
{
  /// <summary>
  /// 將 Result 轉換為 APIResponse
  /// </summary>
  /// <param name="result">應用層的 Result 物件</param>
  /// <param name="workflowError">流程狀態錯誤碼（完整格式：DES-{Type}-{Scope}-{ErrorNo}）</param>
  /// <returns>API 層的 APIResponse 物件</returns>
  public static APIResponse<T> ToApiResponse<T>(this IHandlerResult<T> result)
  {
    if (result.IsSuccess)
    {
      return new APIResponse<T>(
        Code: result.Code ?? "0000",
        Message: result.Message ?? "操作成功",
        Data: result.Data
      );
    }

    return new APIResponse<T>(
      Code: result.Code ?? "9999",
      Message: result.Message ?? "系統錯誤",
      ExceptionDetails: result.Error?.ToExceptionDetails()
    );
  }
}

/// <summary>
/// 錯誤映射擴充方法
/// 將 ErrorDetail 轉換為 ExceptionDetails
/// </summary>
public static class ErrorMappingExtensions
{
  /// <summary>
  /// 將 ErrorDetail 轉換為 ExceptionDetails（包含技術細節）
  /// </summary>
  /// <param name="error">錯誤詳細資訊</param>
  /// <returns>例外錯誤詳細資訊</returns>
  public static ExceptionDetails ToExceptionDetails(this ErrorDetail error)
  {
    return new ExceptionDetails(
      Type: error.ErrorCode.Source.ToString(),
      Title: error.ErrorCode.Description, // 原始錯誤訊息描述
      Detail: error.GuardTrace ?? error.StackTrace ?? "",
      ValidationErrors: error
        .ValidationErrors?.GroupBy(v => v.Field)
        .ToDictionary(g => g.Key, g => g.Select(x => x.Message).ToArray())
    );
  }

  /// <summary>
  /// Prod 用：移除技術細節
  /// </summary>
  public static ExceptionDetails ToExceptionDetailsWithoutTechnicalDetails(this ErrorDetail error)
  {
    return new ExceptionDetails(
      Type: error.ErrorCode.Source.ToString(),
      Title: error.ErrorCode.Description,
      Detail: error.GuardTrace,
      ValidationErrors: error
        .ValidationErrors?.GroupBy(v => v.Field)
        .ToDictionary(g => g.Key, g => g.Select(x => x.Message).ToArray())
    );
  }
}
