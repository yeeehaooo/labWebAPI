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
/// </summary>
/// <typeparam name="T">回傳資料類型</typeparam>
public record APIResponse<T>
{
  public string Code { get; }
  public string Message { get; }
  public T? Data { get; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public ExceptionDetails? ExceptionDetails { get; }


  public string TraceId { get; }

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

