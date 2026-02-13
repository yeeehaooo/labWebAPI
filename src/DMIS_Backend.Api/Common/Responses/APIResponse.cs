using System.Diagnostics;
using System.Text.Json.Serialization;

namespace DMIS_Backend.Api.Common.Responses;

public record APIResponse
{
  /// <summary>
  /// 
  /// </summary>
  public string Code { get; internal set; }

  /// <summary>
  /// 回應訊息
  /// </summary>
  public string Message { get; }

  /// <summary>
  /// 追蹤 ID（分散式追蹤用）
  /// </summary>
  public string TraceId { get; }


  public APIResponse(
      string code,
      string message,
      string? traceId = null)
  {
    Code = code;
    Message = message;
    TraceId = traceId ?? Activity.Current?.Id ?? string.Empty;
  }
}
/// <summary>
/// API 統一回傳格式
/// 所有 API 端點都使用此格式回傳，確保回應的一致性
/// </summary>
/// <typeparam name="T">回傳資料類型</typeparam>
public sealed record APIResponse<T> : APIResponse
{
  /// <summary>
  /// 回傳資料（成功時才有）
  /// </summary>
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public T? Data { get; }


  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public object? Meta { get; }

  public APIResponse(
      string code,
      string message,
      T? data = default,
      object? meta = default,
      string? traceId = null)
    : base(code, message, traceId)
  {
    Data = data;
    Meta = meta;
  }

  //public static APIResponse<T> Success(T data)
  //  => new(SystemCode.Success.Value, SystemCode.Success.Message, data);
  //public static APIResponse<T> Success(T data, ErrorCode code, string? message = null)
  //  => new(code.Value, message ?? code.Message, data);

  //public static APIResponse<T> Failure(ErrorCode code, string? message = null, object? meta = null)
  //    => new(code.Value, message ?? code.Message, default, meta);
}

//public sealed record ValidationResponse : APIResponse
//{
//  public Dictionary<string, string[]> Errors { get; }

//  public ValidationResponse(
//      string code,
//      string message,
//      Dictionary<string, string[]> errors,
//      string? traceId = null)
//      : base(code, message, traceId)
//  {
//    Errors = errors;
//  }
//}
