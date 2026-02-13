using DMIS_Backend.Domain.Kernel.Primitives;

namespace DMIS_Backend.Application.Core.Abstractions.Results;

/// <summary>
/// Handler 結果介面（泛型版本）
/// 定義 UseCase Handler 的統一回傳格式，包含強型別的資料
/// </summary>
/// <typeparam name="T">資料類型</typeparam>
public interface IHandlerResult<out T> : IHandlerResult
{
  /// <summary>強型別資料</summary>
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


  string Code { get; }

  string Message { get; }

  ErrorCode? Error { get; }

}

