using DMIS_Backend.Application.Core.Abstractions.Results;
using DMIS_Backend.Domain.Kernel.Primitives;

namespace DMIS_Backend.Application.Core.Results;

/// <summary>
/// Result Pattern - UseCase 執行結果
/// 僅描述「成功 / 失敗」與其語意，不包含任何 API / Workflow 資訊
/// </summary>
public sealed class Result<T> : Result, IHandlerResult<T>
{
  private readonly T? _data;

  private Result(bool isSuccess, T? data, ErrorCode error, string? message = null)
        : base(isSuccess, error, message)
  {
    _data = data;
  }

  /// <summary>成功時的資料</summary>
  public T? Data => IsSuccess ? _data : default;

  // ===== Factory Methods =====
  public static Result<T> Success(T value)
        => new(true, value, SystemCode.Success);

  public static new Result<T> Failure(ErrorCode error, string? message = null)
  {
    return new(false, default, error, message);
  }

  // ===== Mapping =====

  public Result<TOut> Map<TOut>(Func<T, TOut> mapper)
  {
    if (IsFailure)
    {
      return Result<TOut>.Failure(Error, CustomMessage);
    }

    return Result<TOut>.Success(mapper(Data!));
  }
}

public class Result : IHandlerResult
{
  protected string? CustomMessage { get; }


  public bool IsSuccess { get; }

  public bool IsFailure => !IsSuccess;

  public string Code => Error.Value;

  public string Message =>
      CustomMessage ?? Error.Message;

  public ErrorCode Error { get; }

  protected Result(bool isSuccess, ErrorCode error, string? message = null)
  {
    IsSuccess = isSuccess;
    Error = error;
    CustomMessage = message;
  }

  public static Result Success()
    => new(true, SystemCode.Success);
  public static Result Failure(ErrorCode error, string message)
  {
    return new(false, error, message);
  }
  public static Result Failure(ErrorCode error)
      => new(false, error);
}
