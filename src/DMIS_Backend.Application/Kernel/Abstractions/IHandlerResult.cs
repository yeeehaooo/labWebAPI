using DMIS_Backend.Application.Kernel.Guards;

namespace DMIS_Backend.Application.Kernel.Abstractions;

public interface IHandlerResult<T> : IHandlerResult
{
  T? Data { get; }
}
/// <summary>
/// Result 基礎介面
/// </summary>
public interface IHandlerResult
{
  bool IsSuccess { get; }
  bool IsFailure => !IsSuccess;

  /// <summary>
  /// 錯誤碼（ErrorNo），例如 0000~9999
  /// </summary>
  string? Code { get; }

  string? Message { get; }

  object? Data { get; }

  ErrorDetail? Error { get; }

  ///// <summary>
  ///// 完整流程錯誤碼（例如 "DES-1-DES020-7001"）
  ///// 此欄位由 API 層組合設定，用於向後相容
  ///// </summary>
  //string? WorkflowCode { get; }
}

