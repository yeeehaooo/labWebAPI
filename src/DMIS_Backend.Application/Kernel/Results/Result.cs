using DMIS_Backend.Application.Kernel.Abstractions;
using DMIS_Backend.Application.Kernel.Guards;
using DMIS_Backend.Domain.Kernel.ErrorCodes;
using DMIS_Backend.Domain.Kernel.Guards.Exceptions;

namespace DMIS_Backend.Application.Kernel.Results;

/// <summary>
/// Result Pattern - 結果模式
/// 用於統一處理操作的成功或失敗狀態，避免使用異常進行控制流程
/// </summary>
public class Result<T> : IHandlerResult
{
  private readonly T? _data;
  protected Result(bool success, string? code, string message, T? data, ErrorDetail? error)
  {
    IsSuccess = success;
    Code = code;
    _data = data;
    Message = message;
    Error = error;
  }
  // ⭐ 非泛型視角（給 Filter）
  public object? Data => IsSuccess ? _data : null;
  public T? Value => IsSuccess ? _data! : default;
  public bool IsSuccess { get; }
  public bool IsFailure => !IsSuccess;
  public string? Code { get; }
  public string? Message { get; }
  public ErrorDetail? Error { get; }

  ///// <summary>
  ///// 完整流程錯誤碼（例如 "DES-1-DES020-7001"）
  ///// 此欄位由 API 層組合設定，用於向後相容
  ///// </summary>
  //public string? WorkflowCode { get; internal set; }

  public static Result<T> Success() => new(true, "0000", string.Empty, default, null);
  public static Result<T> Success(T data) => new(true, "0000", string.Empty, data, null);
  /// <summary>
  /// 建立失敗結果
  /// </summary>
  public static Result<T> Failure(string code, string message, ErrorDetail error)
    => new(false, code, message, default, error);

  //public static Result Failure(ErrorCode errorCode)
  //{
  //  var error = new ErrorDetail(
  //      errorCode: errorCode
  //  );

  //  return new Result(false, errorCode.Code, errorCode.Description, error);
  //}
  public static Result<T> Failure(GuardException<IErrorCode> ex)
  {
    var error = new ErrorDetail(
        errorCode: ex.ErrorCode,
        guardTrace: ex.Context.DiagnosticInfo
    );

    return new Result<T>(
        success: false,
        code: ex.ErrorCode.Code,
        data: default,
        message: ex.DisplayMessage,
        error: error
    );
  }
  public static Result<T> Failure(IErrorCode errorCode, Exception ex)
  {
    var error = new ErrorDetail(
        errorCode: errorCode,
        guardTrace: null,
        stackTrace: null //太多 ex.StackTrace
    );

    return new Result<T>(
        success: false,
        code: errorCode.Code,
        data: default,
        message: ex.Message,
        error: error
    );
  }
  /// <summary>
  /// 隱式轉換：從 ErrorDetail 轉換為 Result
  /// </summary>
  public static implicit operator Result<T>(GuardException<IErrorCode> exception) => Failure(exception);


  public Result<TOut> Map<TOut>(Func<T, TOut> mapper)
  {
    if (IsFailure)
    {
      return Result<TOut>.Failure(Code!, Message, Error!);
    }

    var newValue = mapper(_data!);
    return Result<TOut>.Success(newValue);
  }
}

