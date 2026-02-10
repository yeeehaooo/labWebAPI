using DMIS_Backend.Application.Kernel.Guards;

namespace DMIS_Backend.Application.Kernel.Abstractions;

/// <summary>
/// Handler 結果介面（泛型版本）
/// 定義 UseCase Handler 的統一回傳格式，包含強型別的資料
/// </summary>
/// <typeparam name="T">資料類型</typeparam>
public interface IHandlerResult<T> : IHandlerResult
{
  /// <summary>
  /// 強型別的資料（成功時才有值）
  /// </summary>
  T? Data { get; }
}

/// <summary>
/// Handler 結果基礎介面
/// 定義 UseCase Handler 的統一回傳格式，用於統一處理操作的成功或失敗狀態
/// </summary>
public interface IHandlerResult
{
  /// <summary>
  /// 是否成功
  /// </summary>
  bool IsSuccess { get; }
  /// <summary>
  /// 是否失敗
  /// </summary>
  bool IsFailure => !IsSuccess;

  /// <summary>
  /// 錯誤碼（ErrorNo），例如 0000~9999
  /// </summary>
  string? Code { get; }

  /// <summary>
  /// 訊息
  /// </summary>
  string? Message { get; }

  /// <summary>
  /// 資料（成功時才有值）
  /// </summary>
  object? Data { get; }

  /// <summary>
  /// 錯誤詳細資訊（失敗時才有值）
  /// </summary>
  ErrorDetail? Error { get; }

  ///// <summary>
  ///// 完整流程錯誤碼（例如 "DES-1-DES020-7001"）
  ///// 此欄位由 API 層組合設定，用於向後相容
  ///// </summary>
  //string? WorkflowCode { get; }
}

