using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace DMIS_Backend.Api.Common;



/// <summary>
/// 例外錯誤詳細資訊
/// </summary>
/// <param name="Type">例外錯誤類型</param>
/// <param name="Title">例外錯誤標題</param>
/// <param name="Detail">例外錯誤詳細資訊</param>
/// <param name="ValidationErrors">驗證錯誤列表</param>
public record ExceptionDetails(string Type, string Title, string Detail, Dictionary<string, string[]>? ValidationErrors = default);

/// <summary>
/// API 統一回傳格式
/// 所有 API 端點都使用此格式回傳，確保回應的一致性
/// </summary>
/// <typeparam name="T">回傳資料類型</typeparam>
public record APIResponse<T>
{
  /// <summary>
  /// 狀態碼（完整工作流程錯誤碼，格式：Module-Operation-Function-ErrorCode）
  /// </summary>
  public string Code { get; }

  /// <summary>
  /// 回應訊息
  /// </summary>
  public string Message { get; }

  /// <summary>
  /// 回傳資料（成功時包含，失敗時為 null）
  /// </summary>
  public T? Data { get; }

  /// <summary>
  /// 例外錯誤詳細資訊（僅在發生錯誤時包含）
  /// </summary>
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public ExceptionDetails? ExceptionDetails { get; }

  /// <summary>
  /// 追蹤 ID（用於日誌追蹤和除錯）
  /// </summary>
  public string TraceId { get; }

  /// <summary>
  /// 初始化 API 回應
  /// </summary>
  /// <param name="Code">狀態碼（完整工作流程錯誤碼）</param>
  /// <param name="Message">回應訊息</param>
  /// <param name="Data">回傳資料（成功時包含）</param>
  /// <param name="ExceptionDetails">例外錯誤詳細資訊（失敗時包含）</param>
  /// <param name="TraceId">追蹤 ID（用於日誌追蹤）</param>
  public APIResponse(
    [Display(Name = "狀態碼")] string Code,
    [Display(Name = "訊息")] string Message,
    [Display(Name = "回傳資料")] T? Data = default,
    [Display(Name = "例外錯誤")] ExceptionDetails? ExceptionDetails = null,
    [Display(Name = "追蹤 ID")] string? TraceId = null
  )
  {
    this.Code = Code;
    this.Message = Message;
    this.Data = Data;
    this.ExceptionDetails = ExceptionDetails;
    this.TraceId = TraceId ?? Activity.Current?.Id ?? "";
  }
}
